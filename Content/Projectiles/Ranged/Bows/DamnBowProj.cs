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

namespace FM.Content.Projectiles.Ranged.Bows
{
	public class DamnBowProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 1;
			//AIType = ProjectileID.Bullet;
			Projectile.aiStyle = 1;
		}
		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 4, 4, 27);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
			for (int d = 0; d < 1; d++)
			{
				int index2 = Dust.NewDust(Projectile.position, 10, 10, 27);
	    		Main.dust[index2].noGravity = true;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.ShadowFlame, 180, false);
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].scale = 0.9f;
				Main.dust[num].noGravity = false;
			}
		}
	}
}
