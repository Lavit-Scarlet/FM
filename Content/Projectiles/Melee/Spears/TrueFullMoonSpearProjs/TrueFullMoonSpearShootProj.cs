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

namespace FM.Content.Projectiles.Melee.Spears.TrueFullMoonSpearProjs
{
    public class TrueFullMoonSpearShootProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            //Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
        }
		public override Color? GetAlpha(Color lightColor)
		{
			//return Color.White;
			return new Color(53, 255, 43, 0) * (1f - (float)Projectile.alpha / 255f);
		}
		public override bool PreDraw(ref Color lightColor) 
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) 
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
		public override void AI()
        {
			{
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 107, 0.0f, 0.0f, 0, new Color(), 1f);
			    Main.dust[dust].noGravity = true;
			    Main.dust[dust].scale = 1f;
            }
        }
		public override void Kill(int timeLeft)
		{
			//Main.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			for (int k = 0; k < 20; k++)
			{
				int index5 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 107, Projectile.oldVelocity.X * 0.6f, Projectile.oldVelocity.Y * 0.6f);
				Main.dust[index5].noGravity = true;
				Main.dust[index5].scale = 1.2f;
			}
		}
    }
}