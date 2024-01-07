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

namespace FM.Content.Projectiles.Magic.SoTIN
{
	public class StaffofTheInfernalNecromancerKillProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 260;
			Projectile.height = 260;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 20;
			Projectile.timeLeft = 120;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(10))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
			}
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.75f) / 255f, ((255 - Projectile.alpha) * 0.5f) / 255f, ((255 - Projectile.alpha) * 0.01f) / 255f);
			if (Projectile.wet && !Projectile.lavaWet)
			{
				Projectile.Kill();
			}
			bool flag15 = false;
			bool flag16 = false;
			if (Projectile.velocity.X < 0f && Projectile.position.X < Projectile.ai[0])
			{
				flag15 = true;
			}
			if (Projectile.velocity.X > 0f && Projectile.position.X > Projectile.ai[0])
			{
				flag15 = true;
			}
			if (Projectile.velocity.Y < 0f && Projectile.position.Y < Projectile.ai[1])
			{
				flag16 = true;
			}
			if (Projectile.velocity.Y > 0f && Projectile.position.Y > Projectile.ai[1])
			{
				flag16 = true;
			}
			if (flag15 && flag16)
			{
				Projectile.Kill();
			}
			float num461 = 50f;
			if (Projectile.ai[0] > 100f)
			{
				num461 -= (Projectile.ai[0] - 100f) / 2f;
			}
			if (num461 <= 0f)
			{
				num461 = 0f;
				Projectile.Kill();
			}
			num461 *= 0.5f;
			Projectile.ai[0] += 3.5f;
			int num462 = 0;
			while (num462 < num461)
			{
				float num463 = Main.rand.Next(-10, 11);
				float num464 = Main.rand.Next(-10, 11);
				float num465 = Main.rand.Next(4, 24);
				float num466 = (float)Math.Sqrt(num463 * num463 + num464 * num464);
				num466 = num465 / num466;
				num463 *= num466;
				num464 *= num466;
				int num467 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 174, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num467].noGravity = true;
				Main.dust[num467].position.X = Projectile.Center.X;
				Main.dust[num467].position.Y = Projectile.Center.Y;
				Dust expr_149DF_cp_0 = Main.dust[num467];
				expr_149DF_cp_0.position.X = expr_149DF_cp_0.position.X + Main.rand.Next(-10, 11);
				Dust expr_14A09_cp_0 = Main.dust[num467];
				expr_14A09_cp_0.position.Y = expr_14A09_cp_0.position.Y + Main.rand.Next(-10, 11);
				Main.dust[num467].velocity.X = num463;
				Main.dust[num467].velocity.Y = num464;
				num462++;
			}
		}
	}
}
