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

namespace FM.Content.Projectiles.Magic
{
	public class IceStickProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.ignoreWater = false;
			Projectile.aiStyle = 1;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 120;
			Projectile.DamageType = DamageClass.Magic;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(6))
			{
				target.AddBuff(BuffID.Frostburn, 60, false);
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
					int index2 = Dust.NewDust(Projectile.position, 4, 4, 135, 0.0f, 0.0f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 1f;
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
			for (int i = 0; i < 6; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 0, default(Color), 1.6f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}
