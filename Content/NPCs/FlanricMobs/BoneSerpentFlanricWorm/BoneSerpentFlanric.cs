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
using FM.NPCs;
using static Terraria.ModLoader.ModContent;
using FM.Content.Items.Materials;
using FM.Content.Buffs.Debuff;

namespace FM.Content.NPCs.FlanricMobs.BoneSerpentFlanricWorm
{
    internal class BoneSerpentFlanricHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<BoneSerpentFlanricBody>();
		public override int TailType => ModContent.NPCType<BoneSerpentFlanricTail>();

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flanric bone serpent");
        }

        public override void SetDefaults() 
		{
			NPC.CloneDefaults(NPCID.BoneSerpentHead);
			NPC.aiStyle = -1;
            NPC.width = 76;
			NPC.height = 76;
            NPC.value = Item.buyPrice(0, 0, 88, 0);
            NPC.defense = 20;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.lifeMax = 400;
            NPC.lavaImmune = true;
            NPC.damage = 56;
		}

        public override void Init() 
		{
			MinSegmentLength = 26;
			MaxSegmentLength = 26;
			CommonWormInit(this);
		}

        internal static void CommonWormInit(Worm worm) 
        {
			worm.MoveSpeed = 10.5f;
			worm.Acceleration = 0.145f;
		}
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), Main.rand.Next(120, 360));
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                    Main.dust[num].velocity *= 4f;
				    Main.dust[num].noGravity = true;
                }
                int Head1 = Mod.Find<ModGore>("BoneSerpentFlanricGore1").Type;
				int Head2 = Mod.Find<ModGore>("BoneSerpentFlanricGore2").Type;
                int Head3 = Mod.Find<ModGore>("BoneSerpentFlanricGore3").Type;
                int Head4 = Mod.Find<ModGore>("BoneSerpentFlanricGore4").Type;
                int Head5 = Mod.Find<ModGore>("BoneSerpentFlanricGore5").Type;

				var entitySource = NPC.GetSource_Death();
                {
					Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head1);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head2);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head3);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head3);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head4);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head4);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head5);
				}
                    
            }
            for (int i = 0; i < 8; i++)
            {
                int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
				Main.dust[num].noGravity = true;
                Main.dust[num].scale = 0.6f;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) 
		{
            if(Main.hardMode)
            {
                return SpawnCondition.Underworld.Chance * 0.3f;//Cavern = пешеры //Underworld = АД //Crimson = да //OverworldNightMonster = ночь // OverworldDaySlime = день
            }
            return 0f;
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<FlanricCrystal>(), 1, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ItemType<ScarletFragment>(), 1, 2, 12));
        }
    }

    internal class BoneSerpentFlanricBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flanric bone serpent");
        }
		public override void SetDefaults() 
		{
			NPC.CloneDefaults(NPCID.BoneSerpentBody);
			NPC.aiStyle = -1;
            NPC.width = 20;
			NPC.height = 20;
            NPC.lifeMax = 400;
            NPC.defense = 40;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.lavaImmune = true;
            NPC.damage = 40;
		}
		public override void Init() 
        {
			BoneSerpentFlanricHead.CommonWormInit(this);
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    return;
                }
                for (int i = 0; i < 6; i++)
                {
                    int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                    Main.dust[num].velocity *= 4f;
				    Main.dust[num].noGravity = true;
                }
                
                int Body1 = Mod.Find<ModGore>("BoneSerpentFlanricBodyGore1").Type;
				int Body2 = Mod.Find<ModGore>("BoneSerpentFlanricBodyGore2").Type;

				var entitySource = NPC.GetSource_Death();
                {
					Gore.NewGore(entitySource, NPC.position, NPC.velocity, Body1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Body2);
				}
                    
            }
            for (int i = 0; i < 8; i++)
            {
                int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
				Main.dust[num].noGravity = true;
                Main.dust[num].scale = 0.6f;
            }
        }
    }

    internal class BoneSerpentFlanricTail : WormTail
	{
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flanric bone serpent");
        }
		public override void SetDefaults() 
        {
			NPC.CloneDefaults(NPCID.BoneSerpentTail);
			NPC.aiStyle = -1;
            NPC.width = 20;
			NPC.height = 20;
            NPC.lifeMax = 400;
            NPC.defense = 0;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.lavaImmune = true;
            NPC.damage = 32;
		}

		public override void Init() 
        {
			BoneSerpentFlanricHead.CommonWormInit(this);
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                    Main.dust[num].velocity *= 4f;
				    Main.dust[num].noGravity = true;
                }
                int Tail1 = Mod.Find<ModGore>("BoneSerpentFlanricTailGore1").Type;

				var entitySource = NPC.GetSource_Death();
                {
					Gore.NewGore(entitySource, NPC.position, NPC.velocity, Tail1);
				}
                    
            }
            for (int i = 0; i < 8; i++)
            {
                int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
				Main.dust[num].noGravity = true;
                Main.dust[num].scale = 0.6f;
            }
        }
	}
    
}