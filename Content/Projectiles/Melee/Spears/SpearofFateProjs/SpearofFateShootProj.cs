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

namespace FM.Content.Projectiles.Melee.Spears.SpearofFateProjs
{
	public class SpearofFateShootProj : ModProjectile
	{
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 52;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            //Projectile.timeLeft = 30;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.light = 0.25f;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num = 0f;
            return new bool?(Collision.CheckAABBvLineCollision(
                Utils.TopLeft(targetHitbox),
                Utils.Size(targetHitbox),
                Projectile.Center - Vector2.Normalize(Projectile.velocity) * ((Projectile.width + Projectile.height) / 4),
                Projectile.Center + Vector2.Normalize(Projectile.velocity) * ((Projectile.width + Projectile.height) / 4),
                Projectile.width,
                ref num));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 2)
            {
                Color ColorDV = new Color(172, 255, 114); ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(1f, Projectile.scale) * 2;
                Scale *= Mult;
                Mult *= 1f - Projectile.alpha / 255f;
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Color CW = new Color(172, 255, 114); CW.A = 0;
                CW *= 0.25f;
                color *= Mult; CW *= Mult;
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), CW, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
            }
            return false;
        }
		public override void AI()
		{
			Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
            Projectile.velocity *= 0.98f;
			Projectile.alpha += 5;
            if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			} 
		}
	}
}
