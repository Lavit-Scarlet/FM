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
using Terraria.GameContent.Drawing;

namespace FM.Content.Projectiles.Melee.Swords.MagmaSwordProjs
{
	public class MagmaSwordWave : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 80;
			Projectile.height = 140;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
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
            Color color = Color.Lerp(Color.White, Color.White, 0.75f);
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
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 174, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1f;

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.alpha += 5;
            if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			} 
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(80 / 2f, 140 / 2f), Projectile.width - 80, Projectile.height - 140);
            if (collding)
            {
                Projectile.velocity *= 0.7f;
            }
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
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
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return true;
        }
	}
}
