using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using FM.Content.Projectiles.Magic.Books.KillerDollProjs;

namespace FM.Content.Projectiles.Magic.Books.KillerDollProjs
{
	public class KillerDollLaserProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 100;
		}
		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 4, 4, 229, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
			Player player = Main.player[Projectile.owner];
			Projectile.ai[0] -= 1.1f;
			if (Projectile.localAI[1] == 0f)
			{
				Projectile.localAI[1] = 1f;
				float count = 40.0f;
				for (int k = 0; (double)k < (double)count; k++)
				{
					Vector2 vector2 = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double)k * (6.22 / (double)count), new Vector2()) * new Vector2(2.0f, 14.0f)).RotatedBy((double)Projectile.velocity.ToRotation(), new Vector2());
					int dust = Dust.NewDust(Projectile.Center - new Vector2(0.0f, 4.0f), 0, 0, 229, 0.0f, 0.0f, 0, new Color(), 1.0f);
					Main.dust[dust].scale = 1f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position = Projectile.Center + vector2;
					Main.dust[dust].velocity = Projectile.velocity * 0.0f + vector2.SafeNormalize(Vector2.UnitY) * 1.0f;
					Main.dust[dust].noLight = true;
				}
			}
		}
		private Vector2 origin;
		private int targetWhoAmI = -1;

		public override void Kill(int timeLeft)
		{
			Vector2 dirUnit = Vector2.Normalize(Projectile.velocity);
            Projectile.NewProjectile(Entity.GetSource_FromAI(), origin + (dirUnit), Projectile.velocity, ModContent.ProjectileType<CollidingKnife>(), 0, 0f, Projectile.owner);
		}
	}
}
