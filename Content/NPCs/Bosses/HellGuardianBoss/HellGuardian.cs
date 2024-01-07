
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
using FM.Content.NPCs.Bosses.HellGuardianBoss;
using static Terraria.ModLoader.ModContent;
using FM.Globals;
using ParticleLibrary;
using FM.Particles;
using Terraria.GameContent;
using FM.Content.Items.Materials;
using FM.Content.Items.Weapons.Magic;
using FM.Content.Items.Weapons.Melee.Spears;
using FM.Content.Items.Placeable.MusicBoxes;
using FM.Content.Items.Weapons.Ranged.Guns;
using FM.Content.Items.Weapons.Ranged.Bows;
using FM.Content.NPCs.Bosses.HellGuardianBoss;
using FM.Content.Items.Weapons.Melee.Swords;
using FM.Content.Items.BossBags;
using FM.Content.Buffs.Debuff;

namespace FM.Content.NPCs.Bosses.HellGuardianBoss
{
	[AutoloadBossHead]
	public class HellGuardian : ModNPC
	{
		public override void SetStaticDefaults()
		{
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
			NPC.width = 110;
			NPC.height = 110;
			NPC.damage = 26;
			NPC.defense = 20;
			NPC.lifeMax = 4435;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.boss = true;
			NPC.npcSlots = 10f;
			//DrawOffsetY = 24;
			NPC.aiStyle = -1;
			NPC.lavaImmune = true;
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<HellGuardianBag>()));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<StaffofTheInfernalNecromancer>(), 2));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HellstoneBow>(), 2));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MagmaSword>(), 2));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.Hellstone, 1, 4, 12));
			notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.Obsidian, 1, 1, 16));
        }
		bool dash = false;
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


            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
            Color white = Color.White;
            float colorLerpAmt = 0.5f;
            int afterimageAmt = 7;
			Color color = Color.Lerp(Color.Yellow, Color.Red, 0.5f);

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
        public override bool CheckDead()
        {
            Player player = Main.LocalPlayer;
			FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
			modPlayer.Shake += 6;
			Color color = Color.Lerp(Color.DarkRed, Color.Yellow, 0.25f);
			Vector2 goreVel = new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
			for (int g = 0; g < 2; g++)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("HellGuardianGore").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("HellGuardianGore3").Type, NPC.scale);
			}
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("HellGuardianGore2").Type, NPC.scale);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("HellGuardianGore4").Type, NPC.scale);
            ParticleManager.NewParticle<HollowCircle>(NPC.Center, Vector2.Zero, color, 0.4f, 0, 0);
            for (int i = 0; i < 42; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(NPC.Center, new Vector2(Main.rand.NextFloat(-30f, 30f), Main.rand.NextFloat(-30f, 30f)), color, Main.rand.NextFloat(1f, 3f), 0, 0);
				ParticleManager.NewParticle<LineStreak_Long_Impact>(NPC.Center, new Vector2(Main.rand.NextFloat(-30f, 30f), Main.rand.NextFloat(-30f, 30f)), color, Main.rand.NextFloat(1f, 3f), 0, 0);
			}
            return true;
        }
		public override void OnKill()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int centerX = (int)(NPC.position.X + (NPC.width / 2)) / 16;
				int centerY = (int)(NPC.position.Y + (NPC.height / 2)) / 16;
				int halfLength = NPC.width / 2 / 16 + 1;
				for (int x = centerX - halfLength; x <= centerX + halfLength; x++)
				{
					for (int y = centerY - halfLength; y <= centerY + halfLength; y++)
					{
						Tile tile = Main.tile[x, y];
						if ((x == centerX - halfLength || x == centerX + halfLength || y == centerY - halfLength || y == centerY + halfLength) && !Main.tile[x, y].HasTile)
						{
							tile.TileType = TileID.Obsidian;
							tile.HasTile = true;
						}
						tile.LiquidType = LiquidID.Lava;
						tile.LiquidAmount = 0;
						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendTileSquare(-1, x, y, 1);
						else
							WorldGen.SquareTileFrame(x, y, true);
					}
				}
			}
			for (int i = 0; i < 40; i++)
			{
				int num = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 54, 0f, 0f, 0, default(Color), 3f);
				Main.dust[num].velocity *= 12f;
				Main.dust[num].noGravity = true;
			}
		}
		private Vector2 targetPos;
		private Vector2 targetPos2;
		private int ivala = 0;
		public override void AI()
        {
			NPC.netUpdate = true;
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			Player player = Main.player[NPC.target];
			if (player.dead)
			{
				NPC.rotation = (player.Center - NPC.Center).ToRotation() - (float)Math.PI / 2;
				NPC.velocity.Y += 0.5f;
				NPC.velocity.X *= 0.8f;
				NPC.EncourageDespawn(10);
				return;
			}
            {
				switch (State)
				{
					case AttackState.AttackPattern1:
						{
							dash = true;
							NPC.ai[1] += 1;
							targetPos = player.Center;
							if (NPC.ai[1] == 4)
							{
								SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
								if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= NPC.lifeMax * .5f)
								{
									SoundEngine.PlaySound(SoundID.Item73, NPC.Center);
									Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
            					    direction.Normalize();
            					    direction.X *= 4f;
            					    direction.Y *= 4f;
									for (int i = 0; i < 6; ++i)
            					    {
            					        float A = (float)Main.rand.Next(-200, 200) * 0.01f;
            					        float B = (float)Main.rand.Next(-200, 200) * 0.01f;
										int damage = Main.expertMode ? 14 : 18;
            					        if (Main.netMode != NetmodeID.MultiplayerClient)
            					            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<HellGuardianProj>(), damage, 1, Main.myPlayer, 0, 0);
                    				}
								}
							}
							if (NPC.ai[1] <= 18)
							{
								SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
								if (Main.getGoodWorld) NPC.ai[1] += 1;
								Vector2 targetVel = Vector2.Normalize(targetPos - NPC.Center) * 25;
								NPC.velocity = targetVel * (-0.1f);
								var targetRot = Vector2.Subtract(player.Center, NPC.Center).ToRotation() - (float)Math.PI / 2;
								NPC.rotation += (targetRot - NPC.rotation) / 4;
								if (NPC.ai[1] == 18)
								{
									NPC.velocity = targetVel;
								}
							}
							if ((NPC.ai[1] - 19) * (NPC.ai[1] - 85) <= 0)
							{
								NPC.rotation = NPC.velocity.ToRotation() - (float)Math.PI / 2;
								int num21 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 174, NPC.velocity.X, NPC.velocity.Y, 100, default(Color), 1.4f);
								Main.dust[num21].noGravity = true;
								Main.dust[num21].scale = 1;
								Main.dust[num21].velocity *= -1;
							}
							if (NPC.ai[1] >= 85)
							{
								NPC.ai[1] = 0;
								NPC.velocity *= 0.2f;
								State = AttackState.AttackPattern3;
							}
						}
					break;
					case AttackState.AttackPattern2:
						{
							dash = true;
							NPC.ai[1] += 1;
							targetPos = player.Center;
							if (NPC.ai[1] == 4)
							{
								SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
								if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= NPC.lifeMax * .5f)
								{
									SoundEngine.PlaySound(SoundID.Item73, NPC.Center);
									Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
            					    direction.Normalize();
            					    direction.X *= 4f;
            					    direction.Y *= 4f;
									for (int i = 0; i < 6; ++i)
            					    {
            					        float A = (float)Main.rand.Next(-200, 200) * 0.01f;
            					        float B = (float)Main.rand.Next(-200, 200) * 0.01f;
										int damage = Main.expertMode ? 14 : 18;
            					        if (Main.netMode != NetmodeID.MultiplayerClient)
            					            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<HellGuardianProj>(), damage, 1, Main.myPlayer, 0, 0);
                    				}
								}
							}
							if (NPC.ai[1] <= 18)
							{
								SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
								Vector2 targetVel = Vector2.Normalize(targetPos - NPC.Center) * 25;
								if (Main.getGoodWorld) NPC.ai[1] += 1;
								NPC.velocity = targetVel * (-0.1f);
								var targetRot = Vector2.Subtract(player.Center, NPC.Center).ToRotation() - (float)Math.PI / 2;
								NPC.rotation += (targetRot - NPC.rotation) / 4;
								if (NPC.ai[1] == 18)
								{
									NPC.velocity = targetVel;
								}
							}
							if ((NPC.ai[1] - 19) * (NPC.ai[1] - 85) <= 0)
							{
								NPC.rotation = NPC.velocity.ToRotation() - (float)Math.PI / 2;
								int num21 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 174, NPC.velocity.X, NPC.velocity.Y, 100, default(Color), 1.4f);
								Main.dust[num21].noGravity = true;
								Main.dust[num21].noLight = true;
								Main.dust[num21].scale = 1;
								Main.dust[num21].velocity *= -1;
							}
							if (NPC.ai[1] >= 85)
							{
								NPC.ai[1] = 0;
								NPC.velocity *= 0.2f;
								State = AttackState.AttackPattern3;
							}
						}
					break;
					case AttackState.AttackPattern3:
						{
							dash = false;
							ivala += 1;
							int pattern = Main.rand.Next(1, 3);
							if (ivala > 7)
							{
								ivala = -1;
								NPC.ai[1] = -30;
							}
							if (ivala < 5)
							{
								switch (pattern)
								{
									case 1:
										State = AttackState.AttackPattern1;
										break;
									case 2:
										State = AttackState.AttackPattern2;
										break;
								}
							}
							else
							{
								NPC.TargetClosest();
								switch (pattern)
								{
									case 1:
										State = AttackState.AttackPattern4;
										break;
									case 2:
										State = AttackState.AttackPattern5;
										break;
								}
							}
						}
					break;
					case AttackState.AttackPattern4:
						{
							NPC.ai[1] += 1;
							targetPos = new Vector2(player.Center.X + ((NPC.Center.X - player.Center.X > 0) ? 1 : -1) * 545, player.Center.Y);
							NPC.rotation = (player.Center - NPC.Center).ToRotation() - (float)Math.PI / 2;
							Vector2 targetVel = Vector2.Normalize(targetPos - NPC.Center) * 16;
							NPC.velocity.X += ((targetVel.X - NPC.velocity.X > 0) ? 1 : -1) * 0.4f;
							NPC.velocity.Y += ((targetVel.Y - NPC.velocity.Y > 0) ? 1 : -1) * 0.4f;
							if (Vector2.Distance(targetPos, NPC.Center) <= 30) NPC.velocity *= 0.8f;

							if (NPC.life > NPC.lifeMax * 0.4)
							{
								if (NPC.ai[1] % 38 == 0)
								{
									SoundEngine.PlaySound(SoundID.Item73, NPC.position);
									if(Main.netMode != NetmodeID.MultiplayerClient)
									{
            					        Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
            					        direction.Normalize();
            					        direction.X *= 10f;
            					        direction.Y *= 10f;
            					        {
            					            float A = (float)Main.rand.Next(-50, 50) * 0.01f;
            					            float B = (float)Main.rand.Next(-50, 50) * 0.01f;
											int damage = Main.expertMode ? 14 : 18;
            					            if (Main.netMode != NetmodeID.MultiplayerClient)
            					                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<HellGuardianProj>(), damage, 1, Main.myPlayer, 0, 0);
                    					}
									}
								}
							}
							else
							{
								if (NPC.ai[1] % 32 == 0)
								{
									SoundEngine.PlaySound(SoundID.Item73, NPC.position);
									if(Main.netMode != NetmodeID.MultiplayerClient)
									{
            					        Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
            					        direction.Normalize();
            					        direction.X *= 10f;
            					        direction.Y *= 10f;
            					        {
            					            float A = (float)Main.rand.Next(-50, 50) * 0.01f;
            					            float B = (float)Main.rand.Next(-50, 50) * 0.01f;
											int damage = Main.expertMode ? 14 : 18;
            					            if (Main.netMode != NetmodeID.MultiplayerClient)
            					                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<HellGuardianProj>(), damage, 1, Main.myPlayer, 0, 0);
                    					}
									}
								}
							}
							if (NPC.ai[1] > 300)
							{
								NPC.ai[1] = 0;
								NPC.velocity *= 0.2f;
								State = AttackState.AttackPattern3;
							}
						}
					break;
					case AttackState.AttackPattern5:
						{
							NPC.ai[1] += 1;
							targetPos = new Vector2(player.Center.X + ((NPC.Center.X - player.Center.X > 0) ? 1 : -1) * 545, player.Center.Y);
							NPC.rotation = (player.Center - NPC.Center).ToRotation() - (float)Math.PI / 2;
							Vector2 targetVel = Vector2.Normalize(targetPos - NPC.Center) * 16;
							NPC.velocity.X += ((targetVel.X - NPC.velocity.X > 0) ? 1 : -1) * 0.4f;
							NPC.velocity.Y += ((targetVel.Y - NPC.velocity.Y > 0) ? 1 : -1) * 0.4f;
							if (Vector2.Distance(targetPos, NPC.Center) <= 30) NPC.velocity *= 0.8f;
							float tick = 60;
							if (NPC.life <= NPC.lifeMax * .3f)
							{
								tick = 30;
							}
							if (NPC.ai[1] % tick == 0 && NPC.ai[1] <= 180)
							{
								SoundEngine.PlaySound(SoundID.Item73, NPC.position);
								if(Main.netMode != NetmodeID.MultiplayerClient)
								{
									float ProjVel = 8f;
									if (NPC.life <= NPC.lifeMax * .3f)
									{
										ProjVel = 10f;
									}
									Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
            					    direction.Normalize();
            					    direction.X *= ProjVel;
            					    direction.Y *= ProjVel;
									for (int i = 0; i < 3; ++i)
            					    {
            					        float A = (float)Main.rand.Next(-150, 150) * 0.01f;
            					        float B = (float)Main.rand.Next(-150, 150) * 0.01f;
										int damage = Main.expertMode ? 16 : 20;
            					        if (Main.netMode != NetmodeID.MultiplayerClient)
            					            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<HellGuardianProj>(), damage, 1, Main.myPlayer, 0, 0);
                    				}
								}
							}
							if (NPC.ai[1] > 240)
							{
								NPC.ai[1] = 0;
								NPC.velocity *= 0.2f;
								State = AttackState.AttackPattern3;
							}
						}
					break;
				}
			}
		}
		private enum AttackState : int
		{
			AttackPattern4,
			AttackPattern5,
			AttackPattern1,
			AttackPattern2,
			AttackPattern3,
		}
		private AttackState State
		{
			get { return (AttackState)(int)NPC.ai[0]; }
			set { NPC.ai[0] = (int)value; }
		}
	}
}