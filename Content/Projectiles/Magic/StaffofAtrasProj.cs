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
using FM.Content.Projectiles.Magic;

namespace FM.Content.Projectiles.Magic
{
	public class StaffofAtrasProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 100;
		}

		public override void AI()
		{
			int num = 5;
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 9f)
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 4, 4, 264, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1.2f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}

		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 264, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].noGravity = true;
				Main.dust[num].velocity *= 2f;
			}
		}
	}
}
