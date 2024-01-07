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
using FM.Content.Projectiles.Melee.Spears.GalaxiaProjs;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Melee.Spears.GalaxiaProjs
{
    public class GalaxiaProj2 : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
		public override void SetDefaults()
        {
            Projectile.width = 250;
            Projectile.height = 30;
            Projectile.scale = 1f;
            Projectile.timeLeft = 10;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
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
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = new(0, 0, texture.Width, texture.Height);
            Color color = Main.DiscoColor;
            color *= 1f;
            color.A = 0;
            for (int i = 0; i < 1; i++)
            {
                Vector2 Scale = new(Projectile.scale * 7f, Projectile.scale * 0.4f);
                //Color color2 = Color.White * 0.2f;
                //color2.A = 0;
                Vector2 vector = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                //Main.spriteBatch.Draw(texture, vector, new Rectangle?(rectangle), color2, Projectile.velocity.ToRotation(), Utils.Size(rectangle) / 2f, Scale, 0, 0f);
                Main.spriteBatch.Draw(texture, vector, new Rectangle?(rectangle), color, Projectile.velocity.ToRotation(), Utils.Size(rectangle) / 2f, Scale, 0, 0f);
            }
            return false;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			
			
			float rotParticle = MathHelper.ToRadians(-45);
			
			float numberParticles = Main.rand.Next(3, 6);
			for (int i = 0; i < numberParticles; i++)
			{
				ParticleManager.NewParticle<LineStreak_Long_ShortTime>(target.Center, Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.2f), Main.DiscoColor, Main.rand.NextFloat(1f, 2f), 0, 0);
                ParticleManager.NewParticle<StarParticle2>(target.Center, Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.6f), Main.DiscoColor, Main.rand.NextFloat(0.01f, 0.1f), 0, 0);
			}
		}
        protected virtual float HoldoutRangeMin => 10f;
		protected virtual float HoldoutRangeMax => 15f;
		public override void AI()
        {
            float progress = 0.1f;
            Player player = Main.player[Projectile.owner]; 
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= .98f;
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
        }
    }
}