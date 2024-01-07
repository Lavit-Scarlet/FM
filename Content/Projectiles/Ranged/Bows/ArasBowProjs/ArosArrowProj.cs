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
using FM.Content.Projectiles.Ranged.Bows.ArasBowProjs;

namespace FM.Content.Projectiles.Ranged.Bows.ArasBowProjs
{
	public class ArosArrowProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.extraUpdates = 2;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			int num = 5;
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 2f)
				for (int k = 0; k < 7; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 1, 1, 135, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 0.8f;
					Main.dust[index2].velocity *= 0f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 240, false);
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
			for (int i = 0; i < 30; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 68, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 2.2f;
				Main.dust[num].noGravity = true;
			}
			var player = Main.player[Projectile.owner];
	
            int n = 3;
            int deviation = Main.rand.Next(0, 60);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 5.5f;
                perturbedSpeed.Y *= 5.5f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile),Projectile.Center, perturbedSpeed, ProjectileType<ArosArrowEndProj>(), Projectile.damage, 0, Projectile.owner);
            }
		}
	}
}