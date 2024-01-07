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
using System.IO;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;
using FM.Effects;
using System.Collections.Generic;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Ranged.Guns
{
    public class SoulShotgunRocketProj2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.55f;
            Projectile.extraUpdates = 1;
        }

		private readonly int NUMPOINTS = 22;
        public Color baseColor = new(255, 100, 100);
        public Color endColor = new(100, 0, 0);
		public Color edgeColor = new(255, 0, 0);

        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 1.66f;

        public override void AI()
        {
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, edgeColor, thickness);
            }
			Projectile.netUpdate = true;
			NPC pre = null;
			if (FMHelper.ClosestNPC(ref pre, 400, Projectile.Center))
			{
				float direction1 = Projectile.velocity.ToRotation();
				direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(1));
				Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
			
			}
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 50, 50), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255), Projectile.rotation, drawOrigin, Projectile.scale / 1.5f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


            Main.spriteBatch.End();//Trail
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_1").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.06f);
            effect.Parameters["repeats"].SetValue(3f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }
        public override void Kill(int timeLeft)
		{
			for (int p = 0; p < 15; p++)
			{
				ParticleManager.NewParticle<LineStreak_Long_ShortTime>(Projectile.Center, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f)), Color.Red, Main.rand.NextFloat(0.3f, 0.7f), 0, 0);
			}
			for (int d = 0; d < 20; d++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 235, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}
			for (int d2 = 0; d2 < 20; d2++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 182, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
		}
    }
}
