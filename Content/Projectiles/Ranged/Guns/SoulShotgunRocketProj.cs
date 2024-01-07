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
using FM.Effects.PrimitiveTrails;
using System.IO;
using ReLogic.Content;
using FM.Globals;

namespace FM.Content.Projectiles.Ranged.Guns
{
	public class SoulShotgunRocketProj : ModProjectile, ITrailProjectile
	{
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 25, 25), new Color(100, 25, 25)), new RoundCap(), new DefaultTrailPosition(), 8f, 200f, new DefaultShader());
        }
		int timer = 0;

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			//Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.extraUpdates = 1;
			Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
		}
		public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255);
        }
		public override void AI()
		{
			/*Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 4f)
			{
		    	int num = 5;
		    	for (int k = 0; k < 3; k++)
		    	{
		     		int index2 = Dust.NewDust(Projectile.position, 4, 4, 235, 0.0f, 0.0f, 0, new Color(), 1f);
		     		Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
		     		Main.dust[index2].scale = 1f;
		    		Main.dust[index2].velocity *= 0.02f;
		    		Main.dust[index2].noGravity = true;
		    		Main.dust[index2].noLight = false;
		    	}
		     	int num2 = 5;
		    	for (int u = 0; u < 3; u++)
		    	{
		     		int index2 = Dust.NewDust(Projectile.position, 4, 4, 182, 0.0f, 0.0f, 0, new Color(), 1f);
		     		Main.dust[index2].position = Projectile.Center - Projectile.velocity / num2 * (float)u;
		     		Main.dust[index2].scale = 0.7f;
		    		Main.dust[index2].velocity *= 0.02f;
		    		Main.dust[index2].noGravity = true;
		    		Main.dust[index2].noLight = false;
		    	}
			}*/

			Projectile.netUpdate = true;
			NPC pre = null;
			if (FMHelper.ClosestNPC(ref pre, 400, Projectile.Center))
			{
				float direction1 = Projectile.velocity.ToRotation();
				direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(1));
				Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
			
			}
			/*timer++;
			if(timer == 4)
			{
				Player player = Main.player[Projectile.owner];
				Projectile.ai[0] -= 1.1f;
				if (Projectile.localAI[1] == 0f)
				{
					Projectile.localAI[1] = 1f;

					for (int k = 0; k < 30; k++)
					{
						int index4 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 182, Projectile.oldVelocity.X * 0.8f, Projectile.oldVelocity.Y * 0.8f);
						//Main.dust[index4].position = projectile.Center;
						Main.dust[index4].noGravity = true;
					    Main.dust[index4].scale = 1.4f;
						int index5 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 235, Projectile.oldVelocity.X * 1f, Projectile.oldVelocity.Y * 1f);
						Main.dust[index5].noGravity = true;
					    Main.dust[index5].scale = 1.4f;
					}
				}
			}*/
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 50; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 182, FMHelper.PolarVector(Main.rand.Next(2, 16), theta));
                dust.noGravity = true;
				dust.scale = 1.5f;
            }
			for (int i = 0; i < 50; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 235, FMHelper.PolarVector(Main.rand.Next(2, 20), theta));
                dust.noGravity = true;
				dust.scale = 1.5f;
            }
		}
	}
}
