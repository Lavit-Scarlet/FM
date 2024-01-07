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

namespace FM.Content.NPCs.FlanricMobs.FlanricCavernWorm
{
    internal class FlanricWormHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<FlanricWormBody>();
		public override int TailType => ModContent.NPCType<FlanricWormTail>();

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flanric Worm");
        }

        public override void SetDefaults() 
		{
			NPC.CloneDefaults(NPCID.DiggerHead);
			NPC.aiStyle = -1;
            //NPC.width = 76;
			//NPC.height = 76;
            NPC.value = Item.buyPrice(0, 0, 55, 0);
            NPC.defense = 25;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.lifeMax = 240;
            NPC.lavaImmune = true;
            NPC.damage = 50;
		}
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), Main.rand.Next(120, 360));
        }
        public override void Init() 
		{
			MinSegmentLength = 12;
			MaxSegmentLength = 24;
			CommonWormInit(this);
		}

        internal static void CommonWormInit(Worm worm) 
        {
			worm.MoveSpeed = 6.5f;
			worm.Acceleration = 0.075f;
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
                int Head1 = Mod.Find<ModGore>("FlanricWormHeadGore").Type;
				int Head2 = Mod.Find<ModGore>("FlanricWormHeadGore2").Type;

				var entitySource = NPC.GetSource_Death();
                {
					Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, Head2);
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
                return SpawnCondition.Cavern.Chance * 0.34f;//Cavern = пешеры //Underworld = АД //Crimson = да //OverworldNightMonster = ночь // OverworldDaySlime = день
            }
            return 0f;
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<FlanricCrystal>(), 1, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ItemType<ScarletFragment>(), 1, 1, 6));
        }
    }

    internal class FlanricWormBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flanric Worm");
        }
		public override void SetDefaults() 
		{
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.aiStyle = -1;
            //NPC.width = 20;
			//NPC.height = 20;
            NPC.lifeMax = 240;
            NPC.defense = 40;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.lavaImmune = true;
            NPC.damage = 30;
		}
		public override void Init() 
        {
			FlanricWormHead.CommonWormInit(this);
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
                
                int Body1 = Mod.Find<ModGore>("FlanricWormBodyGore").Type;
				int Body2 = Mod.Find<ModGore>("FlanricWormBodyGore2").Type;

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

    internal class FlanricWormTail : WormTail
	{
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flanric Worm");
        }
		public override void SetDefaults() 
        {
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.aiStyle = -1;
            //NPC.width = 20;
			//NPC.height = 20;
            NPC.lifeMax = 240;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.lavaImmune = true;
            NPC.damage = 25;
		}

		public override void Init() 
        {
			FlanricWormHead.CommonWormInit(this);
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
                int Tail1 = Mod.Find<ModGore>("FlanricWormTailGore").Type;

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