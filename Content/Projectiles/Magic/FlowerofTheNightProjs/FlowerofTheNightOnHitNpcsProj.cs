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

namespace FM.Content.Projectiles.Magic.FlowerofTheNightProjs
{
	public class FlowerofTheNightOnHitNpcsProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 5;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) 
		{
			if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) 
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) 
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 14, 14, 27, 0.0f, 0.0f, 0, default(Color), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 0.6f;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;
			}

			Projectile.ai[0] -= 1.1f;
			if (Projectile.localAI[1] == 0f)
			{
				Projectile.localAI[1] = 1f;
				for (int i = 0; i < 10; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27, 0f, 0f, 0, default(Color), 2f);
					Main.dust[num1].velocity *= 1.2f;
					Main.dust[num1].noGravity = true;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 1.2f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}
