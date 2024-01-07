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
    public class CryoGuardianBigAirProj : ModProjectile
    {
		public override void SetDefaults()
		{
            Projectile.width = 18;
            Projectile.height = 18;

			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;

            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
		}
		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 1, 1, 135, 0.0f, 0.0f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 0.6f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
			for (int i = 0; i < 15; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 80);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
				Main.dust[num].scale = 1f;
			}
		}
	}
}