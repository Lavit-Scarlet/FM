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
using FM.Content.Projectiles.Magic.FireStaffProjs;

namespace FM.Content.Projectiles.Magic.FireStaffProjs
{
	public class FireStaffProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.ignoreWater = false;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 30;
			Projectile.DamageType = DamageClass.Magic;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
			}
		}
		public override void AI()
		{
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 5.8f)
            {
				int num = 5;
				for (int k = 0; k < 5; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 12, 12, 6, 0.0f, 0.0f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 1.6f;
					Main.dust[index2].velocity *= 0.4f;
					Main.dust[index2].noGravity = true;
				}
			}	
			if (Projectile.wet && !Projectile.lavaWet)
			{
				Projectile.Kill();
			}			
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 16; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0, default(Color), 1.6f);
				Main.dust[num].velocity *= 4f;
				Main.dust[num].noGravity = true;
			}
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(3);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
				Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, perturbedSpeed, ProjectileType<FireStaffProj2>(), Projectile.damage, 1, Projectile.owner);
			}
		}
	}
}
