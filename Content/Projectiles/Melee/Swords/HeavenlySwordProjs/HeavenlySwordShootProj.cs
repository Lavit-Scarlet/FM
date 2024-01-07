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
using FM.Content.Projectiles.Melee.Swords.HeavenlySwordProjs;
using Terraria.GameContent.Drawing;

namespace FM.Content.Projectiles.Melee.Swords.HeavenlySwordProjs
{
	public class HeavenlySwordShootProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 224;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
        }
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            Color color = Color.Lerp(new Color(216, 246, 253), new Color(216, 246, 253), 0.75f);
            color.A = 0;
            color *= 1.25f;
            color *= 1f - Projectile.alpha / 255f;
            for (int j = 0; j < Projectile.oldPos.Length; j++)
            {
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Color color3 = color * ((Projectile.oldPos.Length - j) / (float)Projectile.oldPos.Length);
                if (j != 0) color3 *= 0.5f;
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color3, Projectile.rotation, Utils.Size(rectangle) / 2f, new Vector2(1.25f, Projectile.scale * 1.25f), 0, 0f);
            }
            return false;
        }
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.alpha += 3;
            if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			} 
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num = 0f;
            return new bool?(Collision.CheckAABBvLineCollision(
                Utils.TopLeft(targetHitbox),
                Utils.Size(targetHitbox),
                Projectile.Center - Vector2.Normalize(Projectile.velocity) * (Projectile.width / 2 * Projectile.scale),
                Projectile.Center + Vector2.Normalize(Projectile.velocity) * (Projectile.width / 2 * Projectile.scale),
                Projectile.height * Projectile.scale,
                ref num));
        }
	}
}
