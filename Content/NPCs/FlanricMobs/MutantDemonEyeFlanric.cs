using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using FM.Content.Items.Materials;
using static Terraria.ModLoader.ModContent;
using FM.Content.Buffs.Debuff;

namespace FM.Content.NPCs.FlanricMobs
{
    public class MutantDemonEyeFlanric : ModNPC
    {
        public ref float AITimer => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DemonEye);
            NPC.damage = 32;
            NPC.defense = 20;
            NPC.lifeMax = 120;
            NPC.value = Item.buyPrice(0, 0, 24, 0);
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
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), Main.rand.Next(60, 180));
        }
        public override bool PreAI()
        {
            Player player = Main.player[NPC.target];
            if (Main.rand.NextBool(100) && NPC.DistanceSQ(player.Center) < 400 * 400)
            {
                NPC.ai[2] = 1;
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] != 0)
            {
                //NPC.velocity *= 0.96f;
                NPC.rotation = NPC.DirectionTo(player.Center).ToRotation();
                if (AITimer++ >= 20 && NPC.velocity.Length() < 1)
                {
                    NPC.velocity = NPC.DirectionTo(player.Center) * 11;
                    AITimer = 0;
                    NPC.ai[2] = 0;
                    NPC.netUpdate = true;
                }
                return false;
            }
            return true;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 14; i++)
                {
                    int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                    Main.dust[num].velocity *= 3f;
				    Main.dust[num].noGravity = true;
                }
                    
				int Eye1 = Mod.Find<ModGore>("MutantDemonEyeFlanricGore1").Type;
				int Eye2 = Mod.Find<ModGore>("MutantDemonEyeFlanricGore2").Type;
                int Eye3 = Mod.Find<ModGore>("MutantDemonEyeFlanricGore3").Type;
                int Eye4 = Mod.Find<ModGore>("MutantDemonEyeFlanricGore4").Type;

				var entitySource = NPC.GetSource_Death();
                {
					Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye2);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye3);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye3);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye4);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Eye4);
				}
            }
            for (int i = 0; i < 6; i++)
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
                return SpawnCondition.OverworldNightMonster.Chance * 0.44f;//Cavern = пешеры //Underworld = АД //Crimson = да //OverworldNightMonster = ночь // OverworldDaySlime = день
            }
            return 0f;
		}
    }

}