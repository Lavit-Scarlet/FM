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
    public class CryoGuardianBigProj : ModProjectile
    {
		public override void SetDefaults()
		{
            Projectile.width = 18;
            Projectile.height = 18;

			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
		}
		public float start = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Entity moveTarget;
			start = MathHelper.Lerp(start, 10, 0.05f);
			Projectile.ai[0]++;
			if (Projectile.ai[0] > 20)
			{
				int num = 5;
				for (int k = 0; k < 5; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 1, 1, 135, 0.0f, 0.0f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 1f;
					Main.dust[index2].velocity *= 0f;
					Main.dust[index2].noGravity = true;
				}
				Projectile.netUpdate = true;
			}
			Projectile.ai[1]++;
			if (Projectile.ai[1] == 20)
			{
				if(Projectile.owner == Main.myPlayer)
				{
					moveTarget = player;
					Projectile.velocity = Projectile.DirectionTo(moveTarget.Center) * 15f;
				}
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[1] > 20 && Projectile.ai[1] < 50)
            {
				if(Projectile.ai[1] == 28)
				    SoundEngine.PlaySound(SoundID.Item28, Projectile.position);
				Projectile.netUpdate = true;
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