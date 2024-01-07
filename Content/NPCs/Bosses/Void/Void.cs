using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.IO;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;
using FM.Globals;
using ParticleLibrary;
using FM.Particles;
using FM.Content.Items.BossBags;
using FM.Content.Buffs.Debuff;
using FM.Content.NPCs.Bosses.Void.VoidProjs;

namespace FM.Content.NPCs.Bosses.Void
{
    public class Void : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;

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
			NPC.width = 98;
			NPC.height = 98;
			NPC.damage = 32;
			NPC.defense = 10;
			NPC.lifeMax = 11400;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.boss = true;
			NPC.npcSlots = 20f;
			NPC.aiStyle = -1;
			NPC.lavaImmune = true;
            NPC.alpha = 255;
			NPC.DeathSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Mobs/VoidBossSound/VoidDeath")
			{
				Volume = .8f,
				Pitch = 0f
			};
			NPC.HitSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Mobs/VoidBossSound/VoidHit")
			{
				Volume = .8f,
				Pitch = 0f
			};
		}
		bool dash = false;
		bool OriginalColor = false;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Rectangle rectangle = NPC.frame;
            Vector2 origin2 = rectangle.Size() / 2f;
			Color colorOr;
			if (OriginalColor)
			{
				colorOr = NPC.GetAlpha(Color.Lerp(Color.Black, Color.Violet, 0.5f));
			}
			else
			{
				colorOr = NPC.GetAlpha(Color.White);
			}
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, colorOr, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
            Color white = Color.White;
            float colorLerpAmt = 0.5f;
            int afterimageAmt = 7;
			Color color = Color.Lerp(Color.Black, Color.Violet, 0.5f);

			if (dash)
            {
                for (int i = 1; i < afterimageAmt; i += 2)
                {
                    Color color1 = drawColor;
                    color1 = Color.Lerp(color1, white, colorLerpAmt);
                    color1 = NPC.GetAlpha(color1);
                    color1 *= (float)(afterimageAmt - i) / 15f;
                    Vector2 offset = NPC.oldPos[i] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - screenPos;
                    offset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
                    offset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, offset, NPC.frame, color1, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
			}
            return false;
        }
        int attackTeleport = 0;
        int attackTeleportCharges = 0;
        int dashTeleport = 0;
        int dashTeleportCharges = 0;
        int AttackTimer = 0;
        int type = 0;

        int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 300f;
        public override void AI()
        {
            NPC.netUpdate = true;
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
            bool expertMode = Main.expertMode; 
			Player player = Main.player[NPC.target];
			if (player.dead && Main.dayTime)
			{
				NPC.alpha += 5;
				NPC.rotation = (player.Center - NPC.Center).ToRotation() - (float)Math.PI / 2;
				NPC.velocity.Y += 0.5f;
				NPC.velocity.X *= 0.8f;
				for (int i = 0; i < 4; i++)
                {
					float num627 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
					float num628 = Main.rand.NextFloat();
					Vector2 position7 = NPC.Center + new Vector2(0, -0).RotatedBy(NPC.rotation) + num627.ToRotationVector2() * (80f + 80f * num628);
					Vector2 vector154 = (num627 + (float)Math.PI).ToRotationVector2() * (8f + 8f * Main.rand.NextFloat() + 8f * num628);
                    ParticleManager.NewParticle<BloomCircle_FadingIn>(position7, vector154 * 0.3f, Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(1f, 2f), 0, 0);
                }
				NPC.EncourageDespawn(10);
				return;
			}
            
            switch (State)
            {
                case AttackState.AttackPattern1:
                {
                    NPC.alpha -= 3;
					if (NPC.alpha <= 0)
					{
						AttackTimer++;
						if (AttackTimer >= 20)
						{
                            State = AttackState.AttackPattern3;
						}
					}    
                }
                break;
                case AttackState.AttackPattern2://2
                {
                    NPC.rotation = NPC.velocity.X * 0f;
                    NPC.velocity *= 0.22f;
                    AttackTimer++;
					if (AttackTimer == 2)
					{
						FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Mobs/VoidBossSound/VoidTeleport"), (int)NPC.Center.X, (int)NPC.Center.Y, 1f);
					}
                    if (AttackTimer <= 60)
                    {
                        NPC.alpha += 4;
                        for (int i = 0; i < 4; i++)
                        {
							float num627 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
							float num628 = Main.rand.NextFloat();
							Vector2 position7 = NPC.Center + new Vector2(0, -0).RotatedBy(NPC.rotation) + num627.ToRotationVector2() * (80f + 80f * num628);
							Vector2 vector154 = (num627 + (float)Math.PI).ToRotationVector2() * (8f + 8f * Main.rand.NextFloat() + 8f * num628);
                            ParticleManager.NewParticle<BloomCircle_FadingIn>(position7, vector154 * 0.3f, Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(1f, 2f), 0, 0);
                        }
                    }
                    if (AttackTimer == 80)
                    {
                        attackTeleport++;
                        NPC.alpha = 0;
                        float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                        Vector2 spawnPlace = player.Center + Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 400f;
                        NPC.Center = spawnPlace;
						NPC.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Item82, spawnPlace);
						for (int i = 0; i < 40; i++)
                            ParticleManager.NewParticle<BloomCircle>(spawnPlace, new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f)), Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(1f, 2f), 0, 0);
                    }
                    if (AttackTimer == 120)
                    {
						if(Main.netMode != NetmodeID.MultiplayerClient)
						{
							FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/LaserB"), (int)NPC.Center.X, (int)NPC.Center.Y, .34f, .6f);
							if (NPC.life <= NPC.lifeMax * .4f)
							{
								int n = 8;
                                int deviation = Main.rand.Next(-180, 180);
                                for (int i = 0; i < n; i++)
                                {
                                    float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                                    Vector2 perturbedSpeed = new Vector2(120, 120).RotatedBy(rotation);
                                    perturbedSpeed.Normalize();
                                    perturbedSpeed.X *= 8f;
                                    perturbedSpeed.Y *= 8f;
	                		    	int damage = Main.expertMode ? 15 : 18;
	                		    	if (Main.netMode != NetmodeID.MultiplayerClient)
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ProjectileType<VoidBigBolt>(), damage, 1, Main.myPlayer, 0, 0);
                                }
							}
							else
							{
								Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
            				    direction.Normalize();
            					direction.X *= 8f;
            					direction.Y *= 8f;
            				    {
            					    float A = (float)Main.rand.Next(-50, 50) * 0.01f;
            					    float B = (float)Main.rand.Next(-50, 50) * 0.01f;
									int damage = Main.expertMode ? 14 : 18;
            					    if (Main.netMode != NetmodeID.MultiplayerClient)
            					        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<VoidBigBolt>(), damage, 1, Main.myPlayer, 0, 0);
                    			}
							}
						}
                    }
                    if (AttackTimer > 180)
                    {
                        if (NPC.life <= NPC.lifeMax * .4f)
						{
							attackTeleportCharges = 3;
						}
                        else
                        {
                            attackTeleportCharges = 4;
                        }

                        if (attackTeleport >= attackTeleportCharges)
                        {
                            State = AttackState.AttackPattern3;
                        }
                        else
                        {
                            AttackTimer = 0;
                        }
                    }
                }
                break;
                case AttackState.AttackPattern3://3
                {
                    AttackTimer = 0;
                    attackTeleport = 0;
                    dashTeleport = 0;
					dash = false;
					OriginalColor = true;
                    int pattern = Main.rand.Next(1, 12);
					switch (pattern)
					{
						case 1:
							State = AttackState.AttackPattern2;//Teleport Shoot
							break;
						case 2:
                            NPC.TargetClosest();
							State = AttackState.AttackPattern4;//Move Shoot
							break;
						case 3:
                            NPC.TargetClosest();
							State = AttackState.AttackPattern4;//Move Shoot
							break;
						case 4:
                            NPC.TargetClosest();
							State = AttackState.AttackPattern5;//Teleport Dash
							FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Mobs/VoidBossSound/VoidPreDash"), (int)NPC.Center.X, (int)NPC.Center.Y, 1f);
							break;
						case 5:
                            NPC.TargetClosest();
							State = AttackState.AttackPattern6;//Sky Laser
							break;
						case 6:
                            NPC.TargetClosest();
							State = AttackState.AttackPattern6;//Sky Laser
							break;
						case 7:
                            NPC.TargetClosest();
							State = AttackState.AttackPattern7;//Spam Laser
							break;
						case 8:
                            NPC.TargetClosest();
							State = AttackState.AttackPattern7;//Spam Laser
							break;
						case 9:
                            NPC.TargetClosest();
							State = AttackState.AttackPattern7;//Spam Laser
							break;
						case 10:
							State = AttackState.AttackPattern8;//Fast Teleport Shoot
							break;
						case 11:
							State = AttackState.AttackPattern8;//Fast Teleport Shoot
							break;
					}
                }
                break;
                case AttackState.AttackPattern4:
                {
					NPC.rotation = NPC.velocity.X * 0.03f;

					if (NPC.Center.X >= player.Center.X && moveSpeed >= -120)
						moveSpeed--;
					else if (NPC.Center.X <= player.Center.X && moveSpeed <= 120)
						moveSpeed++;

					NPC.velocity.X = moveSpeed * 0.10f;

					if (NPC.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30)
					{
						moveSpeedY--;
						HomeY = 350f;
					}
					else if (NPC.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
						moveSpeedY++;

					NPC.velocity.Y = moveSpeedY * 0.13f;

                    AttackTimer++;
                    if (AttackTimer % 20 == 0 && AttackTimer <= 180)
                    {
                        Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
            			direction.Normalize();
            			direction.X *= 8f;
            			direction.Y *= 8f;
                        Vector2 RockPos;
                        RockPos.Y = Main.rand.NextFloat(NPC.Center.Y + 120, NPC.Center.Y - 120 + 1);
                        RockPos.X = Main.rand.NextFloat(NPC.Center.X + 120, NPC.Center.X - 120 + 1);
						if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), RockPos.X, RockPos.Y, direction.X, direction.Y, ProjectileType<VoidMonster>(), 20, 1, Main.myPlayer, 0, 0);
                    }
                    if (AttackTimer > 240)
                    {
                        NPC.velocity *= 0.9f;
                        AttackTimer = 0;
                        State = AttackState.AttackPattern3;
                    }
                }
                break;
                case AttackState.AttackPattern5:
                {
                    NPC.rotation = NPC.velocity.X * .036f;
                    NPC.velocity *= .96f;
                    AttackTimer++;
					if (AttackTimer >= 1)
					{
						dash = false;
					}
                    if (AttackTimer <= 35)
                    {
                        NPC.alpha += 6;
                        for (int i = 0; i < 4; i++)
                        {
							float num627 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
							float num628 = Main.rand.NextFloat();
							Vector2 position7 = NPC.Center + new Vector2(0, -0).RotatedBy(NPC.rotation) + num627.ToRotationVector2() * (80f + 80f * num628);
							Vector2 vector154 = (num627 + (float)Math.PI).ToRotationVector2() * (8f + 8f * Main.rand.NextFloat() + 8f * num628);
                            ParticleManager.NewParticle<BloomCircle_FadingIn>(position7, vector154 * 0.3f, Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(1f, 2f), 0, 0);
                        }
                    }
                    if (AttackTimer == 60)
                    {
                        NPC.alpha = 0;
                        dashTeleport++;
                        float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                        Vector2 spawnPlace = player.Center + Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 220f;
                        NPC.Center = spawnPlace;
						NPC.netUpdate = true;
						for (int i = 0; i < 40; i++)
                            ParticleManager.NewParticle<BloomCircle>(spawnPlace, new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f)), Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(1f, 2f), 0, 0);
                    }
					if (AttackTimer >= 90)
					{
						dash = true;
					}
                    if (AttackTimer == 110)
                    {
                        NPC.Dash(22, true, CustomSounds.VoidRoar, player.Center);
                    }
					if (AttackTimer == 130)
					{
                        if (NPC.life <= NPC.lifeMax * .5f)
						{
							dashTeleportCharges = 6;
						}
                        else
                        {
                            dashTeleportCharges = 5;
                        }
                        if (dashTeleport >= dashTeleportCharges)
                        {
                            State = AttackState.AttackPattern3;
                        }
                        else
                        {
                            AttackTimer = -10;
                        }
					}
                }
                break;
                case AttackState.AttackPattern6:
                {
                    if (NPC.life <= NPC.lifeMax * .5f)
                    {
                        Vector2 dist = player.Center - NPC.Center;
			            Vector2 direction = player.Center - NPC.Center;
			            NPC.rotation = NPC.velocity.X * 0.03f;
			            NPC.netUpdate = true;
			            if (Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) < 9000) 
			            {
			            	float speed = expertMode ? 12f : 10f;
			            	float acceleration = expertMode ? 0.09f : 0.08f;
			            	Vector2 vector2 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
			            	float xDir = player.position.X + (player.width / 2) - vector2.X;
			            	float yDir = (float)(player.position.Y + (player.height / 2) - 400) - vector2.Y;
			            	float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);
			            	if (length > 400f) 
			            	{
			            		++speed;
			            		acceleration += 0.08f;
			            		if (length > 600f) 
			            		{
			            			++speed;
			            			acceleration += 0.08f;
			            			if (length > 800f) 
			            			{
			            				++speed;
			            				acceleration += 0.08f;
			            			}
			            		}
			            	}
			            	float num10 = speed / length;
			            	xDir *= num10;
			            	yDir *= num10;

			            	if (NPC.velocity.X < xDir) 
			            	{
			            		NPC.velocity.X = NPC.velocity.X + acceleration;
			            		if (NPC.velocity.X < 0 && xDir > 0)
			            			NPC.velocity.X = NPC.velocity.X + acceleration;
			            	}
			            	else if (NPC.velocity.X > xDir) 
			            	{
			            		NPC.velocity.X = NPC.velocity.X - acceleration;
			            		if (NPC.velocity.X > 0 && xDir < 0)
			            			NPC.velocity.X = NPC.velocity.X - acceleration;
			            	}

			            	if (NPC.velocity.Y < yDir) 
			            	{
			            		NPC.velocity.Y = NPC.velocity.Y + acceleration;
			            		if (NPC.velocity.Y < 0 && yDir > 0)
			            			NPC.velocity.Y = NPC.velocity.Y + acceleration;
			            	}
			            	else if (NPC.velocity.Y > yDir) 
			            	{
			            		NPC.velocity.Y = NPC.velocity.Y - acceleration;
			            		if (NPC.velocity.Y > 0 && yDir < 0)
			            			NPC.velocity.Y = NPC.velocity.Y - acceleration;
			            	}
			            }
                        AttackTimer++;

                        if (AttackTimer % 60 == 0 && AttackTimer <= 240)
                        {
                            int C = 0;
                            int V = -1200;
							if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + C, player.Center.Y + V, 0, 1, ProjectileType<VoidLaser>(), 20, 1, Main.myPlayer, 0, 0);
                            for (int i = 0; i < 8; i++)
                            {
                                int A = Main.rand.Next(-600, 600);
                                int B = -1200;
								if (Main.netMode != NetmodeID.MultiplayerClient)
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + A, player.Center.Y + B, 0, 1, ProjectileType<VoidLaser>(), 20, 1, Main.myPlayer, 0, 0);
                            }
                        }
                        if (AttackTimer > 300)
                        {
                            State = AttackState.AttackPattern3;
                        }
                    }
                    else
                    {
                        State = AttackState.AttackPattern3;
                    }
                }
                break;
                case AttackState.AttackPattern7:
                {
                    if (NPC.life <= NPC.lifeMax * .3f)
                    {
                        Vector2 dist = player.Center - NPC.Center;
			            Vector2 direction = player.Center - NPC.Center;
			            NPC.rotation = NPC.velocity.X * 0.03f;
			            NPC.netUpdate = true;
			            if (Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) < 9000) 
			            {
			            	float speed = expertMode ? 12f : 10f;
			            	float acceleration = expertMode ? 0.09f : 0.08f;
			            	Vector2 vector2 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
			            	float xDir = player.position.X + (player.width / 2) - vector2.X;
			            	float yDir = (float)(player.position.Y + (player.height / 2) - 400) - vector2.Y;
			            	float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);
			            	if (length > 400f) 
			            	{
			            		++speed;
			            		acceleration += 0.08F;
			            		if (length > 600f) 
			            		{
			            			++speed;
			            			acceleration += 0.08F;
			            			if (length > 800f) 
			            			{
			            				++speed;
			            				acceleration += 0.08F;
			            			}
			            		}
			            	}
			            	float num10 = speed / length;
			            	xDir *= num10;
			            	yDir *= num10;
			            	if (NPC.velocity.X < xDir) 
			            	{
			            		NPC.velocity.X = NPC.velocity.X + acceleration;
			            		if (NPC.velocity.X < 0 && xDir > 0)
			            			NPC.velocity.X = NPC.velocity.X + acceleration;
			            	}
			            	else if (NPC.velocity.X > xDir) 
			            	{
			            		NPC.velocity.X = NPC.velocity.X - acceleration;
			            		if (NPC.velocity.X > 0 && xDir < 0)
			            			NPC.velocity.X = NPC.velocity.X - acceleration;
			            	}
			            	if (NPC.velocity.Y < yDir) 
			            	{
			            		NPC.velocity.Y = NPC.velocity.Y + acceleration;
			            		if (NPC.velocity.Y < 0 && yDir > 0)
			            			NPC.velocity.Y = NPC.velocity.Y + acceleration;
			            	}
			            	else if (NPC.velocity.Y > yDir) 
			            	{
			            		NPC.velocity.Y = NPC.velocity.Y - acceleration;
			            		if (NPC.velocity.Y > 0 && yDir < 0)
			            			NPC.velocity.Y = NPC.velocity.Y - acceleration;
			            	}
			            }
                        AttackTimer++;

                        if (AttackTimer % 10 == 0 && AttackTimer <= 120)
                        {
                            int C = 0;
                            int V = -1660;
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + C, player.Center.Y + V, 0, 1, ProjectileType<VoidLaser>(), 20, 1, Main.myPlayer, 0, 0);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + V, player.Center.Y + C, 1, 0, ProjectileType<VoidLaser>(), 20, 1, Main.myPlayer, 0, 0);
							}
                        }
                        if (AttackTimer > 180)
                        {
                            State = AttackState.AttackPattern3;
                        }
                    }
                    else
                    {
                        State = AttackState.AttackPattern3;
                    }
                }
                break;
				case AttackState.AttackPattern8:
				{
					if (NPC.life <= NPC.lifeMax * .3f)
					{
						NPC.rotation = NPC.velocity.X * 0;
                        NPC.velocity *= 0.22f;
                        AttackTimer++;
						if (AttackTimer == 1)
				    	{
					    	FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Mobs/VoidBossSound/VoidTeleport"), (int)NPC.Center.X, (int)NPC.Center.Y, 1f);
				    	}
						if (AttackTimer <= 15)
						{
							NPC.alpha += 15;
                            for (int i = 0; i < 4; i++)
                            {
					    		float num627 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
					    		float num628 = Main.rand.NextFloat();
					    		Vector2 position7 = NPC.Center + new Vector2(0, -0).RotatedBy(NPC.rotation) + num627.ToRotationVector2() * (80f + 80f * num628);
					    		Vector2 vector154 = (num627 + (float)Math.PI).ToRotationVector2() * (8f + 8f * Main.rand.NextFloat() + 8f * num628);
                                ParticleManager.NewParticle<BloomCircle_FadingIn>(position7, vector154 * 0.3f, Color.Lerp(new Color(130, 57, 125), new Color(23, 12, 64), 0.56f), Main.rand.NextFloat(1f, 2f), 0, 0);
                            }
						}
                        if (AttackTimer == 20)
                        {
                            attackTeleport++;
                            NPC.alpha = 0;
                            float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                            Vector2 spawnPlace = player.Center + Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 580f;
                            NPC.Center = spawnPlace;
							SoundEngine.PlaySound(SoundID.Item82, spawnPlace);
							NPC.netUpdate = true;
					    	for (int i = 0; i < 40; i++)
                                ParticleManager.NewParticle<BloomCircle>(spawnPlace, new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f)), Color.Lerp(new Color(130, 57, 125), new Color(23, 12, 64), 0.56f), Main.rand.NextFloat(1f, 2f), 0, 0);
                        }
                        if (AttackTimer == 40)
                        {
				    		if(Main.netMode != NetmodeID.MultiplayerClient)
				    		{
				    			FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/LaserB"), (int)NPC.Center.X, (int)NPC.Center.Y, .34f, .6f);
            	    		    Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
            	    		    direction.Normalize();
            	    			direction.X *= 8f;
            	    			direction.Y *= 8f;
            	    		    {
            	    			    float A = (float)Main.rand.Next(-50, 50) * 0.01f;
            	    			    float B = (float)Main.rand.Next(-50, 50) * 0.01f;
				    				int damage = Main.expertMode ? 14 : 18;
            	    			    if (Main.netMode != NetmodeID.MultiplayerClient)
            	    			        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<VoidBigBolt>(), damage, 1, Main.myPlayer, 0, 0);
                        		}
				    		}
                        }
						if (AttackTimer > 60)
                        {
							attackTeleportCharges = 12;
                            if (attackTeleport >= attackTeleportCharges)
                            {
                                State = AttackState.AttackPattern3;
                            }
                            else
                            {
                                AttackTimer = 0;
                            }
                        }
					}
                    else
                    {
                        State = AttackState.AttackPattern3;
                    }
				}
				break;
            }
        }
		private enum AttackState : int
		{
            AttackPattern1,
			AttackPattern2,
            AttackPattern3,
            AttackPattern4,
            AttackPattern5,
            AttackPattern6,
            AttackPattern7,
			AttackPattern8,
		}
		private AttackState State
		{
			get { return (AttackState)(int)NPC.ai[0]; }
			set { NPC.ai[0] = (int)value; }
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= (NPC.ai[0] == 2 ? 3 : 4))
			{
				NPC.frameCounter = 0;
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
			}
			NPC.spriteDirection = NPC.direction;
		}
    }
}