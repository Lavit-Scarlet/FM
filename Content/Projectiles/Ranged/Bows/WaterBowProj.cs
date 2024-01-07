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
using FM.Globals;

namespace FM.Content.Projectiles.Ranged.Bows
{
	public class WaterBowProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 4f)
			{
		    	int num = 5;
		    	for (int k = 0; k < 20; k++)
		    	{
		     		int index2 = Dust.NewDust(Projectile.position, 4, 4, 172, 0.0f, 0.0f, 0, new Color(), 1f);
		     		Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
		     		Main.dust[index2].scale = 0.5f;
		    		Main.dust[index2].velocity *= 0f;
		    		Main.dust[index2].noGravity = true;
		    		Main.dust[index2].noLight = false;
		    	}
		     	int num2 = 5;
		    	for (int u = 0; u < 10; u++)
		    	{
		     		int index2 = Dust.NewDust(Projectile.position, 4, 4, 172, 0.0f, 0.0f, 0, new Color(), 1f);
		     		Main.dust[index2].position = Projectile.Center - Projectile.velocity / num2 * (float)u;
		     		Main.dust[index2].scale = 1f;
		    		Main.dust[index2].velocity *= 1f;
		    		Main.dust[index2].noGravity = true;
		    		Main.dust[index2].noLight = false;
		    	}
			}
		}
		public override void Kill(int timeLeft)
		{
            for (int i = 0; i < 50; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 172, FMHelper.PolarVector(Main.rand.Next(2, 16), theta));
                dust.noGravity = true;
				dust.scale = 1.5f;
            }
		}
	}
}
