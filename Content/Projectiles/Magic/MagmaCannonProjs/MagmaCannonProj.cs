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
using FM.Content.Projectiles.Magic.MagmaCannonProjs;
using static Terraria.ModLoader.ModContent;
using FM.Effects;
using System.Collections.Generic;

namespace FM.Content.Projectiles.Magic.MagmaCannonProjs
{
    public class MagmaCannonProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 1;
        }
		public override Color? GetAlpha(Color lightColor) 
		{
			return new Color(255, 215, 70, 0) * Projectile.Opacity;
		}
		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.67f, 0.1f));
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

            Main.spriteBatch.End();
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_1").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.05f);
            effect.Parameters["repeats"].SetValue(1f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
			return true;
		}

        private readonly int NUMPOINTS = 30;
        public Color baseColor = new(255, 255, 100);
        public Color endColor = new(255, 0, 0);
        public Color EdgeColor = new(255, 255, 0);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 4f;

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 174, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1f;
            
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, EdgeColor, thickness);
            }
        }
		public override void Kill(int timeLeft)
		{
            Player player = Main.player[Projectile.owner];
			FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
			modPlayer.Shake += 8;
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 80; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 174, FMHelper.PolarVector(Main.rand.Next(4, 10), theta));
                dust.noGravity = true;
				dust.scale = 3f;
            }
            int n = 8;
            int deviation = Main.rand.Next(0, 120);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(10, 10).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 6f;
                perturbedSpeed.Y *= 6f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile),Projectile.Center, perturbedSpeed, ProjectileType<MagmaCannonFireProj>(), Projectile.damage / 2, 0, Projectile.owner);
            }
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
			}
		}
    }
}