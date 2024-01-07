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

namespace FM.Content.Projectiles.Magic.Books.StardomProjs
{
	public class StardomExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
		}
		public override void SetDefaults()
		{
			Projectile.width = 88;
			Projectile.height = 88;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
		{
			if (++Projectile.frameCounter >= 3)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 5)
				{
					Projectile.Kill();
				}
			}
		}
	}
}
