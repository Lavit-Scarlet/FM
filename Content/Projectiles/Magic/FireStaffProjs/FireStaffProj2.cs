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

namespace FM.Content.Projectiles.Magic.FireStaffProjs
{
	public class FireStaffProj2 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.ignoreWater = false;
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
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
			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 6, 6, 6, 0.0f, 0.0f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;
			}
				
			if (Projectile.wet && !Projectile.lavaWet)
			{
				Projectile.Kill();
			}			
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0, default(Color), 1.6f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}
