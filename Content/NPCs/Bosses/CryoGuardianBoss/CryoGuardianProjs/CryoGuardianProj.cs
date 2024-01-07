using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.NPCs.Bosses.CryoGuardianBoss.CryoGuardianProjs;
using Terraria.Audio;

namespace FM.Content.NPCs.Bosses.CryoGuardianBoss.CryoGuardianProjs
{
    public class CryoGuardianProj : ModProjectile
    {

		public override void SetDefaults()
		{
            Projectile.width = 10;
            Projectile.height = 10;

			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;

            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
			Projectile.frame = Main.rand.Next(4);
		}
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Ice bolt");
			Main.projFrames[Projectile.type] = 4;
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
            for (int k = 0; k < 6; k++)
            {
                int D = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 80, Projectile.oldVelocity.X * -0.2f, Projectile.oldVelocity.Y * -0.2f);
            }
		}

	}
}