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
using FM.Content.Projectiles.Magic.FlowerofTheNightProjs;

namespace FM.Content.Projectiles.Magic.FlowerofTheNightProjs
{
	public class FlowerofTheNightProj : ModProjectile
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
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
		}

		public override void AI()
		{
			int num = 5;
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 9.7f)
			for (int k = 0; k < 6; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 8, 8, 27, 0.0f, 0.0f, 0, default(Color), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0.1f;
				Main.dust[index2].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 4f;
				Main.dust[num].noGravity = true;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int I = 0; I < 3; I++) 
			{
				Player player = Main.player[Projectile.owner];
		    	{
	     			float positionX = Main.rand.Next(-80, 80);
	     			float positionY = Main.rand.Next(-60, 0);
		    		int a = Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.Center.X + positionX, player.Center.Y + positionY, 0f, 0f, ProjectileType<FlowerofTheNightOnHitNpcsProj>(), 16, 0f, player.whoAmI);
		    		Vector2 vector2_2 = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.projectile[a].Center) * 5;
		    		Main.projectile[a].velocity = vector2_2;
	    		}
			}
		}
	}
}