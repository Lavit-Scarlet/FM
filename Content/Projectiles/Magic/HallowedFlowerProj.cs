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
using FM.Effects;
using System.Collections.Generic;
using FM.Content.Buffs.Debuff;
using ParticleLibrary;
using FM.Particles;
using Terraria.GameContent.Drawing;

namespace FM.Content.Projectiles.Magic
{
    public class HallowedFlowerProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.scale = 1f;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }
        private readonly int NUMPOINTS = 12;
        public Color baseColor = new(255, 255, 255);
        public Color endColor = new(255, 220, 60);
        public Color EdgeColor = new(255, 220, 60);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 2.4f;
        public override void AI()
        {
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Player player = Main.player[Projectile.owner];
			
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
                float rotParticle = MathHelper.ToRadians(-70);
                float numberParticles = 15;
                for (int i = 0; i < numberParticles; i++)
				{
        	        Color color = Color.Lerp(Color.Yellow, Color.White, 0.6f);
					ParticleManager.NewParticle<LineStreak_ShortTime>(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(1f, 2f), color, 2f, 0, 0);
				}
			}
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, EdgeColor, endColor, baseColor, thickness, true);
            }
		}
		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.8f, 0.2f));
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_4").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.05f);
            effect.Parameters["repeats"].SetValue(1f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);//



            Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
            Color ColorDV = Color.White;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), ColorDV, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);



            Texture2D texture = ModContent.Request<Texture2D>("FM/Assets/Textures/SoftGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 220, 60), Projectile.rotation, drawOrigin, Projectile.scale / 1.4f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);



            return false;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 15; i++)
			{
                Color color = Color.Lerp(Color.Yellow, Color.White, 0.6f);
				ParticleManager.NewParticle<LineStreak_ShortTime>(Projectile.Center, new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f)), color, 4f, 0, 0);
			}
        }
    }
}
