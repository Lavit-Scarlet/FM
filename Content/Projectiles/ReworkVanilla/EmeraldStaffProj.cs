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
	public class EmeraldStaffProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 100;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 100;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
		}

		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 8, 8, 110, 0.0f, 0.0f, 0, default(Color), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 0.8f;
				Main.dust[index2].velocity *= 0.05f;
				Main.dust[index2].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 18; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 110, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 1f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}
