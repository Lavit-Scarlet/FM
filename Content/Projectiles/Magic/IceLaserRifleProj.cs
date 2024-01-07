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

namespace FM.Content.Projectiles.Magic
{
    public class IceLaserRifleProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 2;
            Projectile.alpha = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.extraUpdates = 4;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 2)
            {
                Color ColorDV = Color.White; ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(Projectile.scale * 2f, Projectile.scale * 0.1f);//длина, ширина
                Scale *= Mult;
                Mult *= 1f - Projectile.alpha / 255f;
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Color CW = Color.White; CW.A = 0;
                CW *= 0.25f;
                color *= Mult; CW *= Mult;
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), CW, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
            }
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(BuffID.Frostburn, 120, false);
			}
		}
    }
}