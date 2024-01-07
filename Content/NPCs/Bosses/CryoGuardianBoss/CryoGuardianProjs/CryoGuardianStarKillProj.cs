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
    public class CryoGuardianStarKillProj : ModProjectile
    {
		public override void SetDefaults()
		{
            Projectile.width = 10;
            Projectile.height = 10;

			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

            Projectile.timeLeft = 40;
            Projectile.penetrate = -1;       
		}
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Ice");
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 2; k++)
            {
                int D = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 80, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
		}

	}
}