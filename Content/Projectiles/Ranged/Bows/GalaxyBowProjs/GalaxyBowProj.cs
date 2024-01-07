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

namespace FM.Content.Projectiles.Ranged.Bows.GalaxyBowProjs
{
	public class GalaxyBowProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 2;
		}
		private readonly int NUMPOINTS = 38;
        public Color baseColor = Main.DiscoColor;
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 6.2f;
        public override void AI()
        {
            Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
            if (Main.rand.NextBool(8))
            {
                float rotParticle = MathHelper.ToRadians(-24);
		    	float numberParticles = 1;
		    	for (int i = 0; i < numberParticles; i++)
		    	{
		    		ParticleManager.NewParticle<StarParticle2>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.2f, 0.6f), Main.DiscoColor, Main.rand.NextFloat(0.04f, 0.08f), 0, 0);
		    	}
            }
            if (Main.netMode != NetmodeID.Server)
            {
            	TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
            	TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, baseColor, baseColor, thickness);
            }
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				ParticleManager.NewParticle<StarParticle2>(Projectile.Center, new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f)), Main.DiscoColor, Main.rand.NextFloat(0.1f, .2f), 0, 0);
			}
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();//Trail
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/StarTrail").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.07f);
            effect.Parameters["repeats"].SetValue(2.1f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);


            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("FM/Assets/Textures/WhiteFlare").Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);
            Vector2 drawOrigin2 = new(texture2.Width / 2, texture2.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Main.DiscoColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture2, Projectile.Center - Main.screenPosition, null, Main.DiscoColor, Projectile.rotation, drawOrigin2, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
	}
}
