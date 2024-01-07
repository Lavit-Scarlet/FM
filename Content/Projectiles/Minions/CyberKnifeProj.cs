using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Effects.PrimitiveTrails;
using System.IO;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using FM.Effects;
using System.Collections.Generic;
using ParticleLibrary;
using FM.Particles;
using FM.Content.Buffs.MinionBuffs;

namespace FM.Content.Projectiles.Minions
{
	public class CyberKnifeProj : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public int charge;
		public override void SetDefaults() 
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.minion = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.minionSlots = 1;
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_1").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.05f);
            effect.Parameters["repeats"].SetValue(2f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

			

			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
            return false;
        }
		public override bool? CanCutTiles() {
			return true;
		}
		public override bool MinionContactDamage() 
		{
			return true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true;
			return true;
		}
        private readonly int NUMPOINTS = 20;
        public Color baseColor = new(255, 255, 255);
        public Color endColor = new(130, 130, 255);
        public Color EdgeColor = new(80, 80, 255);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 2.4f;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, EdgeColor, thickness);
            }

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 42f;
			float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -player.direction;
			idlePosition.X += minionPositionOffsetX;
			Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
			{
				Projectile.position = idlePosition;
				//Projectile.velocity *= 1f;
				Projectile.netUpdate = true;
			}
			float overlapVelocity = 1f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
				{
					if (Projectile.position.X < other.position.X) Projectile.velocity.X -= overlapVelocity;
					else Projectile.velocity.X += overlapVelocity;

					if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
					else Projectile.velocity.Y += overlapVelocity;
				}
			}

			float distanceFromTarget = 550f;
			Vector2 targetCenter = Projectile.position;
			bool foundTarget = false;
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				if (between < 2000f)
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
			if (!foundTarget)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy())
					{
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						bool closeThroughWall = between < 100f;
						if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			Projectile.friendly = foundTarget;
			float speed = 60f;
			float inertia = 20f;

			if (foundTarget)
			{
				if (distanceFromTarget > 120f)
				{
					Vector2 direction = targetCenter - Projectile.Center;
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
					charge++;
					if (charge >= 240)
					{
						Projectile.velocity.X *= 30;
						Projectile.velocity.Y *= 30;
						if (Projectile.velocity.X >= 14.1)
						{ Projectile.velocity.X = 14; }
						if (Projectile.velocity.Y > 11.1)
						{ Projectile.velocity.Y = 11; }
						if (Projectile.velocity.X <= -14.1)
						{ Projectile.velocity.X = -14; }
						if (Projectile.velocity.Y < -11.1)
						{ Projectile.velocity.Y = -11; }
					}
					if (charge >= 270)
					{
						charge = 0;
					}
				}
			}
			else
			{
				charge = 0;
				if (distanceToIdlePosition > 600f)
				{
					speed = 60f;
					inertia = 20f;
				}
				else
				{
					speed = 60f;
					inertia = 20f;
				}
				if (distanceToIdlePosition > 20f)
				{
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
			}
			
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<CyberKnifeBuff>());
				Projectile.active = false;
			}
			if (player.HasBuff(ModContent.BuffType<CyberKnifeBuff>())) 
			{
				Projectile.timeLeft = 2;
			}
		}
	}
}
