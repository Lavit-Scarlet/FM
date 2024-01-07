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
using FM.Content.Projectiles.Magic.SoTIN;

namespace FM.Content.Projectiles.Magic.SoTIN
{
	public class StaffofTheInfernalNecromancerProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 8.4f)
            {
				int num = 5;
				for (int k = 0; k < 5; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 16, 16, 174, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 1.2f;
					Main.dust[index2].velocity *= 0.8f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}

				Player player = Main.player[Projectile.owner];
				Projectile.ai[0] -= 1.1f;
				if (Projectile.localAI[1] == 0f)
				{
					Projectile.localAI[1] = 1f;
					float count = 40.0f;
					for (int k = 0; (double)k < (double)count; k++)
					{
						Vector2 vector2 = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double)k * (6.22 / (double)count), new Vector2()) * new Vector2(2.0f, 14.0f)).RotatedBy((double)Projectile.velocity.ToRotation(), new Vector2());
						int dust = Dust.NewDust(Projectile.Center - new Vector2(0.0f, 4.0f), 0, 0, 174, 0.0f, 0.0f, 0, new Color(), 1.0f);
						Main.dust[dust].scale = 1f;
						Main.dust[dust].noGravity = true;
						Main.dust[dust].position = Projectile.Center + vector2;
						Main.dust[dust].velocity = Projectile.velocity * 0.0f + vector2.SafeNormalize(Vector2.UnitY) * 1.0f;
					}
				}
			}				

		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
			Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X + Projectile.velocity.X, Projectile.Center.Y + Projectile.velocity.Y, Projectile.velocity.X = 0f, Projectile.velocity.Y = 0f, ProjectileType<StaffofTheInfernalNecromancerKillProj>(), Projectile.damage, 1, Projectile.owner, 0f, 0f);
		}
	}
}
