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
using FM.Content.Projectiles.Magic.CrystalStaffProjs;

namespace FM.Content.Projectiles.Magic.CrystalStaffProjs
{
	public class CrystalStaffProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 9f)
            {
				int num = 5;
				for (int k = 0; k < 3; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 4, 4, 254, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 1.2f;
					Main.dust[index2].velocity *= 0.6f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
				for (int k = 0; k < 8; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 4, 4, 254, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 0.8f;
					Main.dust[index2].velocity *= 0f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			for (int i = 0; i < 25; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 254, 0f, 0f, 0, default(Color), 1.6f);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}

			int RProj = Main.rand.Next(2, 4);
			for (int p = 0; p < RProj; ++p)
			{
				Vector2 targetDir = ((((float)Main.rand.Next(-180, 180)))).ToRotationVector2();
				targetDir.Normalize();
				targetDir *= 4;
				Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, targetDir, ProjectileType<CrystalStaffTwoProj>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
			}
		}
	}
}
