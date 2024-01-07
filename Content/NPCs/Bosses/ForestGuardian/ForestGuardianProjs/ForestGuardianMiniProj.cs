using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace FM.Content.NPCs.Bosses.ForestGuardian.ForestGuardianProjs
{
    public class ForestGuardianMiniProj : ModProjectile
    {
		public override void SetDefaults()
		{
            Projectile.width = 10;
            Projectile.height = 10;

			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;

            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Grass, Projectile.position);
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Grass, 0f, 0f, 0, default(Color), 0.7f);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}
		}

	}
}