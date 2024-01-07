using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FM.Content.Projectiles.ReworkVanilla
{
	public class DiamondStaffProj : ModProjectile
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
			//Projectile.extraUpdates = 100;
		}

		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 8, 8, 91, 0.0f, 0.0f, 0, default(Color), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 0.8f;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;
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
					int dust = Dust.NewDust(Projectile.Center - new Vector2(0.0f, 4.0f), 0, 0, 91, 0.0f, 0.0f, 0, new Color(), 1.0f);
					Main.dust[dust].scale = 0.7f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position = Projectile.Center + vector2;
					Main.dust[dust].velocity = Projectile.velocity * 0.0f + vector2.SafeNormalize(Vector2.UnitY) * 1.0f;
				}
			}

		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 14; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 91, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}
