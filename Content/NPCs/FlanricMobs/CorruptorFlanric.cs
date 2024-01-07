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
    public class CorruptorFlanric : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }
        public ref float AITimer => ref NPC.ai[0];
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DemonEye);
            NPC.damage = 44;
            NPC.defense = 20;
            NPC.lifeMax = 280;
            NPC.value = Item.buyPrice(0, 0, 26, 0);
            NPC.knockBackResist = 0f;
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
        public override void AI()
        {
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
                    Main.dust[num].velocity *= 5f;
				    Main.dust[num].noGravity = true;
                }
                    
				int CF1 = Mod.Find<ModGore>("CorruptorGore1").Type;
				int CF2 = Mod.Find<ModGore>("CorruptorGore2").Type;
                int CF3 = Mod.Find<ModGore>("CorruptorGore3").Type;
                int CF4 = Mod.Find<ModGore>("CorruptorGore4").Type;
                int CF5 = Mod.Find<ModGore>("CorruptorGore5").Type;

				var entitySource = NPC.GetSource_Death();
                {
					Gore.NewGore(entitySource, NPC.position, NPC.velocity, CF1);
                    
                    for (int i = 0; i < 2; i++)
                    {
                        Gore.NewGore(entitySource, NPC.position, NPC.velocity, CF3);
                        Gore.NewGore(entitySource, NPC.position, NPC.velocity, CF2);
                    }
                    for (int u = 0; u < 5; u++)
                    {
                        Gore.NewGore(entitySource, NPC.position, NPC.velocity, CF4);
                    }                    
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CF5);
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
                return SpawnCondition.Corruption.Chance * 0.34f;//Cavern = пешеры //Underworld = АД //Crimson = да //OverworldNightMonster = ночь // OverworldDaySlime = день
            }
            return 0f;
		}
    }

}