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

namespace FM.Content.Projectiles.Melee.ChakramofFateProjs
{
    public class ChakramofFateEndProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Magic;
            //Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            //Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
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
                Vector2 Scale = new Vector2(Projectile.scale * 3f, Projectile.scale * 3f);
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
            Projectile.alpha += 15;
            if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
        }
    }
}