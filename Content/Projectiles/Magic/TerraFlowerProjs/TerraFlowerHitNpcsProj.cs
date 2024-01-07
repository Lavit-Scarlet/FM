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
using FM.Content.Projectiles.Magic.TerraFlowerProjs;
using FM.Globals;

namespace FM.Content.Projectiles.Magic.TerraFlowerProjs
{
	public class TerraFlowerHitNpcsProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 100;
		}

		public override void AI()
		{
			{
				int num = 5;
	    		for (int k = 0; k < 5; k++)
		    	{
		    		int index2 = Dust.NewDust(Projectile.position, 8, 8, 107, 0.0f, 0.0f, 0, default(Color), 1f);
		    		Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
		    		Main.dust[index2].scale = 1f;
		    		Main.dust[index2].velocity *= 0f;
		    		Main.dust[index2].noGravity = true;
		    	}
		    	int num2 = 5;
		    	for (int e = 0; e < 5; e++)
		    	{
		    		int index2 = Dust.NewDust(Projectile.position, 8, 8, 107, 0.0f, 0.0f, 0, default(Color), 1f);
		    		Main.dust[index2].position = Projectile.Center - Projectile.velocity / num2 * (float)e;
		     		Main.dust[index2].scale = 1f;
		    		Main.dust[index2].velocity *= 0.4f;
		    		Main.dust[index2].noGravity = true;
		    	}
    
		     	Player player = Main.player[Projectile.owner];
				Projectile.ai[0] -= 1.1f;
				if (Projectile.localAI[1] == 0f)
				{
					Projectile.localAI[1] = 1f;
                    DustHelper.DrawStar(Projectile.Center, 107, 5, 1f, 4f, 1f, 2f, 1f, true, 4, -1); //107 = Дуст: 5 = Количество липестков: 2f = Основной размер: 3f = Плотность Дуста: 1f = размер дуста:
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 25; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 107, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}
