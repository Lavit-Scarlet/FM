using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using Terraria.UI;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.UI;
using System;
using FM.Content.Items.Materials;
using static Terraria.ModLoader.ModContent;
using FM.Content.Buffs.Debuff;

namespace FM.Content.NPCs.FlanricMobs
{
    public class CrimeraFlanric : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DemonEye);
            NPC.damage = 44;
            NPC.defense = 20;
            NPC.lifeMax = 220;
            NPC.value = Item.buyPrice(0, 0, 22, 0);
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = 2;
            AIType = NPCID.DemonEye;
            AnimationType = NPCID.DemonEye;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<FlanricCrystal>(), 2));
            npcLoot.Add(ItemDropRule.Common(ItemType<ScarletFragment>(), 1, 1, 4));
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), Main.rand.Next(120, 360));
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 14; i++)
                {
                    int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                    Main.dust[num].velocity *= 4f;
				    Main.dust[num].noGravity = true;
                }
                    
				int Eye1 = Mod.Find<ModGore>("CrimeraFlanricGore1").Type;
				int Eye2 = Mod.Find<ModGore>("CrimeraFlanricGore2").Type;
                int Eye3 = Mod.Find<ModGore>("CrimeraFlanricGore3").Type;
                int Eye4 = Mod.Find<ModGore>("CrimeraFlanricGore4").Type;
                int Eye5 = Mod.Find<ModGore>("CrimeraFlanricGore5").Type;
                int Eye6 = Mod.Find<ModGore>("CrimeraFlanricGore6").Type;

				var entitySource = NPC.GetSource_Death();
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye2);
                        Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye6);
                    }
					Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye3);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye4);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye5);
				}
            }
            for (int i = 0; i < 10; i++)
            {
                int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) 
		{
            if(Main.hardMode)
            {
                return SpawnCondition.Crimson.Chance * 0.26f;
            }
            return 0f;
		}
    }

}