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

namespace FM.Content.Projectiles.Ranged.Guns
{
	public class LeichenfaustProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.ignoreWater = true;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.extraUpdates = 2;
			Projectile.tileCollide = true;
		}
		public override void AI()
		{
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 14f)
            {
				int num = 5;
				for (int k = 0; k < 10; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 4, 4, 156, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 1f;
					Main.dust[index2].velocity *= 0f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}

				Player player = Main.player[Projectile.owner];
				//Projectile.velocity *= .98f;
				Projectile.rotation += .1f;
				Vector2 center = Projectile.Center;
				float num8 = (float)player.miscCounter / 14f;
				float num7 = 1.0471975512f * 2;
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						int num6 = Dust.NewDust(center, 0, 0, 156, 0f, 0f, 100, default(Color), 1f);
						Main.dust[num6].noGravity = true;
						Main.dust[num6].velocity = Vector2.Zero;
						Main.dust[num6].noLight = false;
						Main.dust[num6].position = center + (num8 * 4.28318548f + num7 * (float)i).ToRotationVector2() * 8f;
					}
				}
			}


		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int num621 = 0; num621 < 70; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 156, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num622].velocity *= 6f;
				Main.dust[num622].noGravity = true;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].noGravity = true;
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
			for (int num623 = 0; num623 < 70; num623++)
			{
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 156, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 5f;
			}
		}

	}
}
