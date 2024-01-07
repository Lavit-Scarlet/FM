using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace FM.Content.Projectiles.Magic.SeverusProjs
{
    public class SeverusTwoProj : ModProjectile
    {
		public override void SetDefaults()
		{
            Projectile.width = 10;
            Projectile.height = 10;

			Projectile.friendly = true;
			Projectile.tileCollide = true;

            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;       
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
            for (int k = 0; k < 2; k++)
            {
                int D = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 80, Projectile.oldVelocity.X * -0.1f, Projectile.oldVelocity.Y * -0.1f);
            }
		}

	}
}