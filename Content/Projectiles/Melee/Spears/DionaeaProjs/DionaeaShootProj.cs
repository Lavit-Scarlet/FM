using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Effects.PrimitiveTrails;
using System.IO;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using FM.Content.Projectiles.Melee.Spears.DionaeaProjs;

namespace FM.Content.Projectiles.Melee.Spears.DionaeaProjs
{
    public class DionaeaShootProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            //Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
        }
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(197, 238, 128, 0) * (1f - (float)Projectile.alpha / 255f);
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
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 107, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1f;
        }
		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 40; num257++)
			{
				int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, 107, 0f, 0f, 0, default(Color), 1.4f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].noLight = true;
				vector9 -= value19 * 2f;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int n = 3;
            int deviation = Main.rand.Next(0, 60);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 6f;
                perturbedSpeed.Y *= 6f;

                Projectile.NewProjectile(Projectile.InheritSource(Projectile),Projectile.Center, perturbedSpeed, ProjectileType<DionaeaShootHitNPCsProj>(), Projectile.damage / 2, 0, Projectile.owner);
            }
		}
    }
}