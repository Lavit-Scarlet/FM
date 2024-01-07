using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace FM.Content.Projectiles.Magic.Books.ForestBookProjs
{
    public class ForestBookProj : ModProjectile
    {
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 18;
            Projectile.height = 18;

			Projectile.friendly = true;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;

            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Grass, Projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Grass, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}
		}

	}
}