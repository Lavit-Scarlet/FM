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

namespace FM.Content.Projectiles.Magic.Books
{
	public class FreezeMarkProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 120;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
		}
		public float start = 0;
		public float rot;
		public override void AI()
		{
			float progress = 0.1f;
			Player player = Main.player[Projectile.owner]; 
			Projectile.ai[0]++;
			if (Projectile.ai[0] > 20)
			{
				int num = 5;
				for (int k = 0; k < 5; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 1, 1, 135, 0.0f, 0.0f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 0.6f;
					Main.dust[index2].velocity *= 0f;
					Main.dust[index2].noGravity = true;
				}
			}
			
			start = MathHelper.Lerp(start, 10, 0.05f);
			Projectile.ai[1]++;
			if (Projectile.ai[1] == 20)
			{
				if(Projectile.owner == Main.myPlayer)
				{
					Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 15f;
					Projectile.netUpdate = true;
				}
			}
			if (Projectile.ai[1] > 20 && Projectile.ai[1] < 50)
            {
				if(Projectile.ai[1] == 28)
				SoundEngine.PlaySound(SoundID.Item28, Projectile.Center);
            }
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(BuffID.Frostburn, 120, false);
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
			for (int i = 0; i < 6; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 80);
				Main.dust[num].velocity *= 1.4f;
				Main.dust[num].noGravity = true;
				Main.dust[num].scale = 1f;
			}
		}
	}
}
