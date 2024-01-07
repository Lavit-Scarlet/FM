using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Content.NPCs.Bosses.Armagem.ArmagemProjs;
using static Terraria.ModLoader.ModContent;
using FM.Content.Items.BossBags;
using FM.Content.Items.Materials;
using FM.Content.Items.Weapons.Magic;
using FM.Content.Items.Weapons.Magic.Books;
using FM.Content.Buffs.Debuff;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.NPCs.Bosses.Armagem
{
    [AutoloadBossHead]
    public class ArmagemHead : ModNPC
    {
        public override string Texture => "FM/Content/NPCs/Bosses/Armagem/ArmagemHead";
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D glowMask = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;
            var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Rectangle rectangle = NPC.frame;
            Vector2 origin2 = rectangle.Size() / 2f;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0f);
            spriteBatch.Draw(glowMask, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frozen] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Electric>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<FlanricDisease>()] = true;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 26f;
            NPC.height = 88;
            NPC.width = 88;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.damage = 80;
            NPC.defense = 60;
            NPC.lifeMax = 150000;
            NPC.value = Item.buyPrice(0, 28, 0, 0);
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            //NPC.behindTiles = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Mobs/ArmagemDead")
			{
				Volume = 1f,
				Pitch = 0f
			};
            NPC.boss = true;
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Armagem");
        }
        public override void BossLoot(ref string name, ref int potionType) 
        {
            potionType = ItemID.SuperHealingPotion;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<ArmagemBag>()));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PureSoulofPower>(), 1, 1, 6));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ScarletFragment>(), 1, 100, 300));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FlanricCrystal>(), 1, 10, 30));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<VeryOldWeapon>(), 8));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<GrimoireofAnotherDimension>(), 8));
        }
        public override bool CheckActive()
        {
            return false;
        }
        int timer = 0;
        public override bool PreAI()
        {
            Player player = Main.player[NPC.target];
            NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;
            NPC.ai[1]++;
            if (NPC.ai[1] >= 1200)
                NPC.ai[1] = 0;
            NPC.TargetClosest(true);
            if (!Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
                if (!Main.player[NPC.target].active || Main.dayTime)
                {
                    NPC.ai[3]++;
                    NPC.velocity.Y = NPC.velocity.Y + 0.11f;
                    if (NPC.ai[3] >= 300)
                    {
                        NPC.active = false;
                    }
                }
                else
                {
                    NPC.ai[3] = 0;
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.realLife = NPC.whoAmI;
                    int latestNPC = NPC.whoAmI;

                    for (int i = 0; i < 24; ++i)
                    {
                        latestNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ArmagemBody>(), NPC.whoAmI, 0, latestNPC);
                        Main.npc[latestNPC].realLife = NPC.whoAmI;
                        Main.npc[latestNPC].ai[3] = NPC.whoAmI;
                    }

                    latestNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ArmagemTail>(), NPC.whoAmI, 0, latestNPC);

                    Main.npc[latestNPC].realLife = NPC.whoAmI;
                    Main.npc[latestNPC].ai[3] = NPC.whoAmI;
                    
                    NPC.ai[0] = 1;
                    NPC.netUpdate = true;
                }
            }

            int minTilePosX = (int)(NPC.position.X / 16.0) - 1;
            int maxTilePosX = (int)((NPC.position.X + NPC.width) / 16.0) + 2;
            int minTilePosY = (int)(NPC.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((NPC.position.Y + NPC.height) / 16.0) + 2;
            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;

            bool collision = true;

            float speed = 15f;
            float acceleration = 0.1f;

            Vector2 NPCCenter = new(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float targetXPos = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2);
            float targetYPos = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2);

            float targetRoundedPosX = (int)(targetXPos / 16.0) * 16;
            float targetRoundedPosY = (int)(targetYPos / 16.0) * 16;
            NPCCenter.X = (int)(NPCCenter.X / 16.0) * 16;
            NPCCenter.Y = (int)(NPCCenter.Y / 16.0) * 16;
            float dirX = targetRoundedPosX - NPCCenter.X;
            float dirY = targetRoundedPosY - NPCCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            if (!collision)
            {
                NPC.TargetClosest(true);
                NPC.velocity.Y = NPC.velocity.Y + 0.11f;
                if (NPC.velocity.Y > speed)
                    NPC.velocity.Y = speed;
                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.4)
                {
                    if (NPC.velocity.X < 0.0)
                        NPC.velocity.X = NPC.velocity.X - acceleration * 1.1f;
                    else
                        NPC.velocity.X = NPC.velocity.X + acceleration * 1.1f;
                }
                else if (NPC.velocity.Y == speed)
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + acceleration;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - acceleration;
                }
                else if (NPC.velocity.Y > 4.0)
                {
                    if (NPC.velocity.X < 0.0)
                        NPC.velocity.X = NPC.velocity.X + acceleration * 0.9f;
                    else
                        NPC.velocity.X = NPC.velocity.X - acceleration * 0.9f;
                }
            }
            else
            {
                if (NPC.soundDelay == 0)
                {
                    float num1 = length / 40f;
                    if (num1 < 10.0)
                        num1 = 10f;
                    if (num1 > 20.0)
                        num1 = 20f;
                    NPC.soundDelay = (int)num1;
                }
                float absDirX = Math.Abs(dirX);
                float absDirY = Math.Abs(dirY);
                float newSpeed = speed / length;
                dirX *= newSpeed;
                dirY *= newSpeed;
                if (NPC.velocity.X > 0.0 && dirX > 0.0 || NPC.velocity.X < 0.0 && dirX < 0.0 || NPC.velocity.Y > 0.0 && dirY > 0.0 || NPC.velocity.Y < 0.0 && dirY < 0.0)
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + acceleration;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - acceleration;
                    if (NPC.velocity.Y < dirY)
                        NPC.velocity.Y = NPC.velocity.Y + acceleration;
                    else if (NPC.velocity.Y > dirY)
                        NPC.velocity.Y = NPC.velocity.Y - acceleration;
                    if (Math.Abs(dirY) < speed * 0.2 && (NPC.velocity.X > 0.0 && dirX < 0.0 || NPC.velocity.X < 0.0 && dirX > 0.0))
                    {
                        if (NPC.velocity.Y > 0.0)
                            NPC.velocity.Y = NPC.velocity.Y + acceleration * 2f;
                        else
                            NPC.velocity.Y = NPC.velocity.Y - acceleration * 2f;
                    }
                    if (Math.Abs(dirX) < speed * 0.2 && (NPC.velocity.Y > 0.0 && dirY < 0.0 || NPC.velocity.Y < 0.0 && dirY > 0.0))
                    {
                        if (NPC.velocity.X > 0.0)
                            NPC.velocity.X = NPC.velocity.X + acceleration * 2f;
                        else
                            NPC.velocity.X = NPC.velocity.X - acceleration * 2f;
                    }
                }
                else if (absDirX > absDirY)
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + acceleration * 1.1f;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - acceleration * 1.1f;
                    if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
                    {
                        if (NPC.velocity.Y > 0.0)
                            NPC.velocity.Y = NPC.velocity.Y + acceleration;
                        else
                            NPC.velocity.Y = NPC.velocity.Y - acceleration;
                    }
                }
                else
                {
                    if (NPC.velocity.Y < dirY)
                        NPC.velocity.Y = NPC.velocity.Y + acceleration * 1.1f;
                    else if (NPC.velocity.Y > dirY)
                        NPC.velocity.Y = NPC.velocity.Y - acceleration * 1.1f;
                    if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
                    {
                        if (NPC.velocity.X > 0.0)
                            NPC.velocity.X = NPC.velocity.X + acceleration;
                        else
                            NPC.velocity.X = NPC.velocity.X - acceleration;
                    }
                }
            }
            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;

            if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 12000f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 12000f || Main.dayTime)
            {
                NPC.velocity.Y = NPC.velocity.Y + 1f;
                if (NPC.position.Y > Main.rockLayer * 16.0)
                {
                    NPC.velocity.Y = NPC.velocity.Y + 1f;
                    speed = 30f;
                }
                if (NPC.position.Y > Main.rockLayer * 16.0)
                {
                    for (int num957 = 0; num957 < 200; num957++)
                    {
                        if (Main.npc[num957].aiStyle == NPC.aiStyle)
                        {
                            Main.npc[num957].active = false;
                        }
                    }
                }
            }

            if (collision)
            {
                if (NPC.localAI[0] != 1)
                    NPC.netUpdate = true;
                NPC.localAI[0] = 1f;
            }
            else
            {
                if (NPC.localAI[0] != 0.0)
                    NPC.netUpdate = true;
                NPC.localAI[0] = 0.0f;
            }
            if ((NPC.velocity.X > 0.0 && NPC.oldVelocity.X < 0.0 || NPC.velocity.X < 0.0 && NPC.oldVelocity.X > 0.0 || NPC.velocity.Y > 0.0 && NPC.oldVelocity.Y < 0.0 || NPC.velocity.Y < 0.0 && NPC.oldVelocity.Y > 0.0) && !NPC.justHit)
            {
                NPC.netUpdate = true;
            }
            timer++;
            if (timer == 160 || timer == 170 || timer == 180 || timer == 190 || timer == 200 || timer == 205 || timer == 210 || timer == 215 || timer == 220 || timer == 225 || timer == 230 || timer == 235 || timer == 240 || timer == 245 || timer == 250)
            {
                SoundEngine.PlaySound(SoundID.Item125, NPC.position);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
             	    direction.Normalize();
             	    direction.X *= 10f;
             	    direction.Y *= 10f;
             	    
             	    float A = (float)Main.rand.Next(-10, 10) * 0.01f;
             	    float B = (float)Main.rand.Next(-10, 10) * 0.01f;
                    int damage = Main.expertMode ? 20 : 26;
             	    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<ArmagemBigLaserProj>(), damage, 1, Main.myPlayer, 0, 0);
                }
            }
            else if (timer == 300 || timer == 310 || timer == 320 || timer == 330 || timer == 340 || timer == 350 || timer == 360 || timer == 370 || timer == 380 || timer == 390 || timer == 400 || timer == 410  || timer == 420 || timer == 430 || timer == 440  || timer == 450 || timer == 460 || timer == 470 || timer == 480)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
        			float theta = (float)Main.rand.NextDouble() * 3.14f * 2;
        			float mag = Main.rand.Next(600, 800);

                    theta = (float)Main.rand.NextDouble() * 3.14f * 2;
         		    mag = Main.rand.Next(600, 800);
                    int damage = Main.expertMode ? 18 : 25;
        		    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + (int)(mag * Math.Cos(theta)), player.Center.Y + (int)(mag * Math.Sin(theta)), -10 * (float)Math.Cos(theta), -10 * (float)Math.Sin(theta), ProjectileType<ArmagemBorderProj>(), damage, 2, Main.myPlayer);
                }
            }
            if (timer == 600 || timer == 720 || timer == 840 || timer == 960 || timer == 1080 || timer == 1200)
            {
                for (int i = 0; i < 3; ++i)
                {
        			float theta = (float)Main.rand.NextDouble() * 3.14f * 2;
        			float mag = Main.rand.Next(400, 800);

                    theta = (float)Main.rand.NextDouble() * 3.14f * 2;
         		    mag = Main.rand.Next(400, 800);
                    int damage = Main.expertMode ? 20 : 30;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
        			    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + (int)(mag * Math.Cos(theta)), player.Center.Y + (int)(mag * Math.Sin(theta)), -10 * (float)Math.Cos(theta), -10 * (float)Math.Sin(theta), ProjectileType<ArmagemBorderLaserProj>(), damage, 2, Main.myPlayer);
                }
            }
            else if (timer == 1320 || timer == 1380 || timer == 1440 || timer == 1500)
            {
                int n = 0;
                if (NPC.life <= NPC.lifeMax * 1f)
                    n = 3;
                if (NPC.life <= NPC.lifeMax * .8f)
                    n = 4;
                if (NPC.life <= NPC.lifeMax * .6f)
                    n = 5;
                if (NPC.life <= NPC.lifeMax * .4f)
                    n = 6;
                if (NPC.life <= NPC.lifeMax * .25f)
                    n = 7;
                if (NPC.life <= NPC.lifeMax * .15f)
                    n = 8;
                if (NPC.life <= NPC.lifeMax * .1f)
                    n = 9;
                if (NPC.life <= NPC.lifeMax * .05f)
                    n = 10;
                for (int i = 0; i < n; ++i)
                {
        	    	float theta = (float)Main.rand.NextDouble() * 3.14f * 3;
        	    	float mag = Main.rand.Next(600, 800);

                    theta = (float)Main.rand.NextDouble() * 3.14f * 3;
         	        mag = Main.rand.Next(600, 800);
                    int damage = Main.expertMode ? 12 : 18;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
        			    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + (int)(mag * Math.Cos(theta)), player.Center.Y + (int)(mag * Math.Sin(theta)), -10 * (float)Math.Cos(theta), -10 * (float)Math.Sin(theta), ProjectileType<ArmagemBorderProj>(), damage, 2, Main.myPlayer);
                }
            }

            if (timer == 1560)
                FMHelper.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 4f, .0f);;
            if (timer == 1680 || timer == 1690 || timer == 1700 || timer == 1710 || timer == 1720 || timer == 1730 || timer == 1740 || timer == 1750 || timer == 1760 || timer == 1770)
            {
                
                if (NPC.life <= NPC.lifeMax * .12f)
                {
                    SoundEngine.PlaySound(SoundID.Item125, NPC.position);
                    int n = 4;
                    int deviation = Main.rand.Next(-180, 180);
                    for (int i = 0; i < n; i++)
                    {
                        float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                        Vector2 perturbedSpeed = new Vector2(-180, 180).RotatedBy(rotation);
                        perturbedSpeed.Normalize();
                        perturbedSpeed.X *= 5f;
                        perturbedSpeed.Y *= 5f;
                        int damage = Main.expertMode ? 18 : 25;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(),  NPC.Center, perturbedSpeed, ProjectileType<ArmagemOrbProj>(), damage, 2, Main.myPlayer, 0, 0);
                    }
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item125, NPC.position);
                    int n = 24;
                    int deviation = Main.rand.Next(-180, 180);
                    for (int i = 0; i < n; i++)
                    {
                        float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                        Vector2 perturbedSpeed = new Vector2(-180, 180).RotatedBy(rotation);
                        perturbedSpeed.Normalize();
                        perturbedSpeed.X *= 5f;
                        perturbedSpeed.Y *= 5f;
                        int damage = Main.expertMode ? 18 : 25;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(),  NPC.Center, perturbedSpeed, ProjectileType<ArmagemBigLaserProj>(), damage, 2, Main.myPlayer, 0, 0);
                    }
                }
            }
            if (timer >= 1850)
				timer = 0;
            return false;
        }
        public override bool CheckDead()
        {
            Player player = Main.LocalPlayer;
			FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
			modPlayer.Shake += 12;
			Vector2 goreVel = new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("ArmagemHeadGore1").Type, NPC.scale);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("ArmagemHeadGore2").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("ArmagemHeadGore2").Type, NPC.scale);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("ArmagemHeadGore3").Type, NPC.scale);
            ParticleManager.NewParticle<HollowCircle_Misty>(NPC.Center, Vector2.Zero, Color.DarkRed, 1f, 0, 0);
            for (int i = 0; i < 90; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(NPC.Center, Main.rand.NextVector2Circular(30f, 30f), Color.DarkRed, Main.rand.NextFloat(1f, 5f) * NPC.scale, 0, 0);
			}
			for (int i = 0; i < 40; i++)
			{
				ParticleManager.NewParticle<BloomCircle_FadingIn>(NPC.Center, new Vector2(Main.rand.NextFloat(-60, 60), Main.rand.NextFloat(-75f, -10f)), Color.DarkRed, Main.rand.NextFloat(1f, 2f), 0, 0);
				ParticleManager.NewParticle<LineStreak_Long_Impact>(NPC.Center, new Vector2(Main.rand.NextFloat(-30f, 30f), Main.rand.NextFloat(-30f, 30f)), Color.DarkRed, Main.rand.NextFloat(1f, 3f), 0, 0);
			}
            return true;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    return;
                }
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                }
            }
        }
    }

    public class ArmagemBody : ArmagemHead
    {
        public override string Texture => "FM/Content/NPCs/Bosses/Armagem/ArmagemBody";
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 84;
            NPC.height = 84;
            NPC.dontCountMe = true;
            NPC.damage = 75;
            NPC.defense = 20000;
            NPC.boss = true;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void PostAI()
        {
            NPC.timeLeft = 10000000;
        }
        int timerB;
        public override bool PreAI()
        {
            Vector2 chasePosition = Main.npc[(int)NPC.ai[1]].Center;
            Vector2 directionVector = chasePosition - NPC.Center;
            NPC.spriteDirection = (directionVector.X > 0f) ? 1 : -1;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[3]].type != ModContent.NPCType<ArmagemHead>())
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
                }
            }

            if (NPC.ai[1] < (double)Main.maxNPCs)
            {
                Vector2 NPCCenter = new(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + Main.npc[(int)NPC.ai[1]].width / 2 - NPCCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + Main.npc[(int)NPC.ai[1]].height / 2 - NPCCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - NPC.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;

                if (dirX < 0f)
                {
                    NPC.spriteDirection = 1;

                }
                else
                {
                    NPC.spriteDirection = -1;
                }

                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
            }

            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active)
                NPC.TargetClosest(true);
            NPC.netUpdate = true;
            return false;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    return;
                }
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                }
                int ABody1 = Mod.Find<ModGore>("ArmagemBodyGore1").Type;
                int ABody2 = Mod.Find<ModGore>("ArmagemBodyGore2").Type;
                int ABody3 = Mod.Find<ModGore>("ArmagemBodyGore3").Type;
                int ABody4 = Mod.Find<ModGore>("ArmagemBodyGore4").Type;
                int ABody5 = Mod.Find<ModGore>("ArmagemBodyGore5").Type;

                var entitySource = NPC.GetSource_Death();
                {
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ABody1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ABody2);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ABody3);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ABody3);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ABody4);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ABody4);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ABody5);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ABody5);
                }
            }
        }
    }

    public class ArmagemTail : ArmagemHead//TAIL
    {
        public override string Texture => "FM/Content/NPCs/Bosses/Armagem/ArmagemTail";
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 86;
            NPC.height = 86;
            NPC.dontCountMe = true;
            NPC.damage = 57;
            NPC.defense = 20;
            NPC.boss = true;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void PostAI()
        {
            NPC.timeLeft = 10000000;
        }
        public override bool PreAI()
        {
            Vector2 chasePosition = Main.npc[(int)NPC.ai[1]].Center;
            Vector2 directionVector = chasePosition - NPC.Center;
            NPC.spriteDirection = (directionVector.X > 0f) ? 1 : -1;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[3]].type != ModContent.NPCType<ArmagemHead>())
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
                }
            }

            if (NPC.ai[1] < (double)Main.maxNPCs)
            {
                Vector2 NPCCenter = new(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + Main.npc[(int)NPC.ai[1]].width / 2 - NPCCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + Main.npc[(int)NPC.ai[1]].height / 2 - NPCCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - NPC.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;

                if (dirX < 0f)
                    NPC.spriteDirection = 1;
                else
                    NPC.spriteDirection = -1;

                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
            }

            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active)
            {
                NPC.TargetClosest(true);
            }
            NPC.netUpdate = true;
            if (NPC.life <= NPC.lifeMax * .15f)
			{
				if (Main.expertMode && Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (Main.rand.NextBool(250) && Main.netMode != NetmodeID.MultiplayerClient)
					{   
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            float theta = (float)Main.rand.NextDouble() * 3.14f * 2;
        		        	float mag = Main.rand.Next(400, 800);
                            theta = (float)Main.rand.NextDouble() * 3.14f * 2;
         		        	mag = Main.rand.Next(400, 800);
                            int damage = Main.expertMode ? 1 : 2;
        		            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + (int)(mag * Math.Cos(theta)), player.Center.Y + (int)(mag * Math.Sin(theta)), -10 * (float)Math.Cos(theta), -10 * (float)Math.Sin(theta), ProjectileType<ArmagemBorderMinionProj>(), damage, 2, Main.myPlayer);
                        }
                        
                        int n = 4;
                        int deviation = Main.rand.Next(-180, 180);
                        for (int i = 0; i < n; i++)
                        {
                            float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                            Vector2 perturbedSpeed = new Vector2(-180, 180).RotatedBy(rotation);
                            perturbedSpeed.Normalize();
                            perturbedSpeed.X *= 5f;
                            perturbedSpeed.Y *= 5f;
                            int damage = Main.expertMode ? 18 : 25;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(),  NPC.Center, perturbedSpeed, ProjectileType<ArmagemOrbProj>(), damage, 2, Main.myPlayer, 0, 0);
                        }
					}
				}
			}
            return false;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    return;
                }
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                }
                int ATail1 = Mod.Find<ModGore>("ArmagemTailGore1").Type;
                int ATail2 = Mod.Find<ModGore>("ArmagemTailGore2").Type;
                int ATail3 = Mod.Find<ModGore>("ArmagemTailGore3").Type;

                var entitySource = NPC.GetSource_Death();
                {
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ATail1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ATail2);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ATail3);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, ATail3);
                }
            }
        }
    }
}