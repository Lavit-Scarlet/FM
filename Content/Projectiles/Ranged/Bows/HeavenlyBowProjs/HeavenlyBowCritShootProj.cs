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

namespace FM.Content.Projectiles.Ranged.Bows.HeavenlyBowProjs
{
	public class HeavenlyBowCritShootProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 22;
            Projectile.height = 22;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 4;
		}
		private readonly int NUMPOINTS = 38;
        public Color baseColor = new(240, 240, 240);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 3.2f;
        public override void AI()
        {
            Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
            if (Main.rand.NextBool(8))
            {
                float rotParticle = MathHelper.ToRadians(-24);
		    	float numberParticles = Main.rand.Next(1, 3);
		    	for (int i = 0; i < numberParticles; i++)
		    	{
		    		ParticleManager.NewParticle<BloomCircle>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.2f, 0.6f), Color.White, Main.rand.NextFloat(0.1f, 0.7f), 0, 0);
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
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/StarFlower"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.66f, 0.5f);
			for (int i = 0; i < 40; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f)), Color.White, Main.rand.NextFloat(0.05f, 0.4f), 0, 0);
			}
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(240, 240, 240), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);



            Main.spriteBatch.End();//Trail
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
            return false;
        }
	}
}
