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
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Ranged.Guns
{
	public class IkvRifle : ModProjectile
	{
		int timer = 0;

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.extraUpdates = 2;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
                target.AddBuff(ModContent.BuffType<Electric>(), 60);
        }

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 2)
            {
                Color ColorDV = new Color(216, 246, 253); ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(Projectile.scale * 0.4f, Projectile.scale * 1.2f);//длина, ширина
                Scale *= Mult;
                Mult *= 1f - Projectile.alpha / 255f;
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                color *= Mult; 
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
            }
            return false;
        }
		public override void AI()
		{
			Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;

			Player player = Main.player[Projectile.owner];
			Projectile.ai[0] -= 1.1f;
			if (Projectile.localAI[1] == 0f)
			{
				Projectile.localAI[1] = 1f;

				for (int k = 0; k < 6; k++)
				{
					int index4 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 226, Projectile.oldVelocity.X * 1.2f, Projectile.oldVelocity.Y * 1.2f);
					//Main.dust[index4].position = projectile.Center;
					Main.dust[index4].noGravity = true;
				    Main.dust[index4].scale = 0.6f;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			for (int k = 0; k < 4; k++)
			{
				int index5 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 226, Projectile.oldVelocity.X * -1f, Projectile.oldVelocity.Y * -1f);
				Main.dust[index5].noGravity = true;
				Main.dust[index5].scale = 0.6f;
			}
		}
	}
}
