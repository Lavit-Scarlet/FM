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
using FM.Content.Projectiles.Melee.Spears.TerraSpearProjs;

namespace FM.Content.Projectiles.Melee.Spears.TerraSpearProjs
{
    public class TerraSpearHitNpcProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.extraUpdates = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 2)
            {
                Color ColorDV = new Color(44, 217, 0); ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(Projectile.scale * 0.8f, Projectile.scale * 1.5f);//длина, ширина
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
            for (int k = 0; k < 1; k++)
            {
                int index = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 107, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f);
                Main.dust[index].position = Projectile.Center;
                Main.dust[index].scale = 1f;
                Main.dust[index].noGravity = true;
            }		
            Projectile.ai[0] -= 1.1f;
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				float count = 60.0f;
				for (int k = 0; (double)k < (double)count; k++)
				{
					Vector2 vector2 = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double)k * (6.22 / (double)count), new Vector2()) * new Vector2(2.0f, 14.0f)).RotatedBy((double)Projectile.velocity.ToRotation(), new Vector2());
					int dust = Dust.NewDust(Projectile.Center - new Vector2(0.0f, 4.0f), 0, 0, 107, 0.0f, 0.0f, 0, new Color(), 1.0f);
					Main.dust[dust].scale = 1f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position = Projectile.Center + vector2;
					Main.dust[dust].velocity = Projectile.velocity * 0.0f + vector2.SafeNormalize(Vector2.UnitY) * 2.0f;
				}
			}		
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 107, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
		}
    }
}