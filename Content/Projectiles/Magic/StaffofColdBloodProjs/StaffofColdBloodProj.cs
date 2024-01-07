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
using FM.Content.Projectiles.Magic.StaffofColdBloodProjs;
using static Terraria.ModLoader.ModContent;
using ParticleLibrary;
using FM.Particles;


namespace FM.Content.Projectiles.Magic.StaffofColdBloodProjs
{
	public class StaffofColdBloodProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 102;
			Projectile.height = 102;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Magic;
			//Projectile.extraUpdates = 1;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{

			target.AddBuff(BuffID.Frostburn, 300, false);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
		}
		public override void AI()
		{
			if (Main.rand.NextBool(2))
			{
				float rotParticle = MathHelper.ToRadians(-10);
				int numberParticles = 1;
				for (int i = 0; i < numberParticles; i++)
				{
					ParticleManager.NewParticle<Snowflake>(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * 1.2f, Color.LightBlue, Main.rand.NextFloat(0.1f, 0.2f), 0, 0);
					ParticleManager.NewParticle<BigFog>(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * 1.2f, Color.LightBlue, Main.rand.NextFloat(0.2f, 0.4f), 0, 0);
				}
			}
			Projectile.rotation += (float)Projectile.direction * 0.12f;
			int dust = Dust.NewDust(Projectile.position, 98, 98, 135, 0.0f, 0.0f, 0, default(Color), 1.4f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1f;
			++Projectile.ai[1];
            if ((double)Projectile.ai[1] % 30.0 == 0.0)
			{
				SoundEngine.PlaySound(SoundID.Item120, Projectile.position);
				int n = 8;
                int deviation = Main.rand.Next(-180, 180);
                for (int i = 0; i < n; i++)
                {
                    float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= 15f;
                    perturbedSpeed.Y *= 15f;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile),Projectile.Center, perturbedSpeed, ProjectileType<StaffofColdBloodShootProj>(), Projectile.damage / 2, 0, Projectile.owner);
                }
			}
		}
		public override void Kill(int timeLeft)
		{
			int randParticles = Main.rand.Next(15, 30);
			for (int i = 0; i < randParticles; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f)), Color.LightBlue, Main.rand.NextFloat(1f, 3f), 0, 0);
				ParticleManager.NewParticle<BigFog>(Projectile.Center, new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f)), Color.LightBlue, Main.rand.NextFloat(0.2f, 0.4f), 0, 0);
				ParticleManager.NewParticle<Snowflake>(Projectile.Center, new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f)), Color.LightBlue, Main.rand.NextFloat(0.1f, 0.2f), 0, 0);
			}
		}
	}
}
