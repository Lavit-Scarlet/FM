using FM.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace FM.Content.Projectiles.Magic
{
	public class IceJackProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.Frostburn, 180, false);
			}
		}
		public override void SetDefaults() 
		{
			Projectile.width = 32;
			Projectile.height = 32;

			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			
			Projectile.coldDamage = true;
			
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.usesIDStaticNPCImmunity = true;
		}

		int delayingAi;
		
		public override void AI()
		{
			Projectile.velocity *= 0.4f;
			int num = 135;
			float num2 = 4f;
			int num3 = 15;
			int num4 = 15;
			int num5 = 1;
			int num6 = 1;
			int num7 = 20;
			int num8 = 20;
			int num9 = 30;
			int maxValue = Main.projFrames[Projectile.type];
			bool flag = Projectile.ai[0] < (float)num7;
			bool flag2 = Projectile.ai[0] >= (float)num8;
			bool flag3 =Projectile.ai[0] >= (float)num9;
			
			if (delayingAi < 15)
			{
				delayingAi += 1;
			}
			
			if (delayingAi >= 15)
			{
				Projectile.ai[0] += 1f;
			}
			
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.frame = Main.rand.Next(maxValue);
				for (int i = 0; i < num3; i++)
				{
					Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24f, 24f), num, Projectile.velocity * num2 * MathHelper.Lerp(0.2f, 0.7f, Main.rand.NextFloat()));
					dust.velocity += Main.rand.NextVector2Circular(1f, 1f);
					dust.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
					dust.noGravity = true;
				}
				for (int j = 0; j < num4; j++)
				{
					Dust dust2 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24f, 24f), num, Main.rand.NextVector2Circular(2f, 2f) + Projectile.velocity * num2 * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
					dust2.velocity += Main.rand.NextVector2Circular(1f, 1f);
					dust2.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
					dust2.fadeIn = 1f;
					dust2.noGravity = true;
				}
				
				for (int i = 0; i < Main.rand.Next(1, 6); i++)
				{
					ParticleManager.NewParticle<BloomCircle_Gravity>(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), new Vector2(Projectile.velocity.X * Main.rand.NextFloat(0.3f, 1.5f), Projectile.velocity.Y * Main.rand.NextFloat(0.3f, 1.5f)).RotatedByRandom(MathHelper.ToRadians(16)), Color.White, Main.rand.NextFloat(0.2f, 0.4f), 0, 0);
				}
				if (Main.rand.NextBool(6))
				{
					ParticleManager.NewParticle<Snowflake>(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), new Vector2(Projectile.velocity.X * Main.rand.NextFloat(0.3f, 1.5f), Projectile.velocity.Y * Main.rand.NextFloat(0.3f, 1.5f)).RotatedByRandom(MathHelper.ToRadians(12)), Color.White, Main.rand.NextFloat(0.12f, 0.22f), 0, 0);
					ParticleManager.NewParticle<BigFog>(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), new Vector2(Projectile.velocity.X * Main.rand.NextFloat(0.3f, 1.5f), Projectile.velocity.Y * Main.rand.NextFloat(0.3f, 1.5f)).RotatedByRandom(MathHelper.ToRadians(12)), Color.LightBlue, Main.rand.NextFloat(0.1f, 0.2f), 0, 0);
				}
				
				Terraria.Audio.SoundEngine.PlaySound(SoundID.DeerclopsIceAttack, Projectile.Center);
			}
			if (flag)
			{
				Projectile.Opacity += 0.1f;
				Projectile.scale = Projectile.Opacity * Projectile.ai[1];
				for (int k = 0; k < num5; k++)
				{
					Dust dust3 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), num, Projectile.velocity * num2 * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
					dust3.velocity += Main.rand.NextVector2Circular(1f, 1f);
					dust3.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
					dust3.noGravity = true;
				}
			}
			if (flag2)
			{
				Projectile.Opacity -= 0.1f;
				for (int l = 0; l < num6; l++)
				{
					Dust dust4 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), num, Projectile.velocity * num2 * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
					dust4.velocity += Main.rand.NextVector2Circular(1f, 1f);
					dust4.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
					dust4.noGravity = true;
				}
			}
			if (flag3)
			{
				Projectile.Kill();
			}
		}
		
		public override bool? CanCutTiles() {
			return true;
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionPoint15 = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + (Projectile.velocity * 0.4f) .SafeNormalize(-Vector2.UnitY) * 200f * Projectile.scale, 22f * Projectile.scale, ref collisionPoint15))
			{
				return true;
			}
			return false;
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			Color color32 = Lighting.GetColor((int)((double)Projectile.position.X + (double)Projectile.width * 0.5) / 16, (int)(((double)Projectile.position.Y + (double)Projectile.height * 0.5) / 16.0));
			
			Texture2D value23 = TextureAssets.Projectile[Projectile.type].Value;
			Microsoft.Xna.Framework.Rectangle value24 = value23.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			Vector2 origin11 = new Vector2(16f, value24.Height / 2);
			Microsoft.Xna.Framework.Color alpha4 = Projectile.GetAlpha(color32);
			Vector2 scale2 = new Vector2(Projectile.scale);
			float lerpValue4 = Utils.GetLerpValue(30f, 25f, Projectile.ai[0], clamped: true);
			scale2.Y *= lerpValue4;
			Vector4 vector37 = color32.ToVector4();
			Vector4 vector38 = new Microsoft.Xna.Framework.Color(38, 37, 89).ToVector4();
			vector38 *= vector37;
			
			Main.EntitySpriteDraw(TextureAssets.Extra[98].Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Projectile.velocity * Projectile.scale * 2.75f, null, new Color(vector38.X, vector38.Y, vector38.Z) * Projectile.Opacity, Projectile.rotation + (float)Math.PI / 2f, TextureAssets.Extra[98].Value.Size() / 2f, Projectile.scale * 0.9f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(value23, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), value24, alpha4, Projectile.rotation, origin11, scale2, SpriteEffects.None, 0);
			return false;
		}
	}
}
