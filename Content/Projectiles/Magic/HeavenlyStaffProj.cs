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
using FM.Content.Projectiles.Magic.WitherStaffProjs;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Magic
{
	public class HeavenlyStaffProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
            Projectile.scale = 0.1f;
		}
		public override bool? CanCutTiles() => false;
		public float rot;

		private readonly int NUMPOINTS = 10;
        public Color baseColor = new(244, 244, 244);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 3f;
        public override void AI()
        {
			Projectile.width = 20;
            Projectile.height = 20;
            switch (Projectile.localAI[0])
            {
                case 0:
                    if (Projectile.localAI[2] == 0)
                    {
                        Projectile.rotation += Projectile.velocity.Length() * 0.03f * Projectile.direction;
                    }
                    Projectile.scale += 0.04f;
                    Projectile.alpha -= 30;
                    Projectile.velocity *= 0.96f;
                    if (Projectile.velocity.Length() < 1 && Projectile.alpha <= 0)
                    {
                        Projectile.velocity *= 0;
                        Projectile.localAI[0] = 1;
                    }
                    break;
                case 1:
                	if (Main.netMode != NetmodeID.Server)
                	{
                 		TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
        	        	TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, baseColor, baseColor, thickness);
        	        }
                    Projectile.penetrate = 1;
                    if (Main.rand.NextBool(5))
                    {
                        float rotParticle = MathHelper.ToRadians(-24);
						float numberParticles = Main.rand.Next(1, 10);
						for (int i = 0; i < numberParticles; i++)
						{
							ParticleManager.NewParticle<BloomCircle>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.4f), Color.White, Main.rand.NextFloat(0.1f, 0.7f), 0, 0);
						}
                    }
                    Projectile.localAI[2] = 1;
                    if (Projectile.localAI[1] == 0)
                    {
                        Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 30;
                        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
                        FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/HeavenlyBlast"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .80f, 1f);
                        ParticleManager.NewParticle<HollowCircle_Small2>(Projectile.Center, Projectile.velocity, Color.White, 0.6f, 0, 0);

                        float rotParticle = MathHelper.ToRadians(-40);
						float numberParticles = Main.rand.Next(15, 30);
						for (int i = 0; i < numberParticles; i++)
						{
							ParticleManager.NewParticle<BloomCircle>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.4f), Color.White, Main.rand.NextFloat(0.1f, 0.4f), 0, 0);
						}

                        Projectile.localAI[1] = 1;
                    }
                    break;
            }
            Projectile.alpha = (int)MathHelper.Clamp(Projectile.alpha, 0, 255);
            Projectile.scale = MathHelper.Clamp(Projectile.scale, 0.1f, 1);
        }
		public override void Kill(int timeLeft)
		{
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/StarFlower"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.66f, 0.92f);
            float rotParticle = MathHelper.ToRadians(-60);
			float numberParticles = Main.rand.Next(10, 20);
			for (int i = 0; i < numberParticles; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.2f), Color.White, Main.rand.NextFloat(0.3f, 0.8f), 0, 0);
			}
			for (int p = 0; p < 30; p++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), Color.White, Main.rand.NextFloat(0.01f, 0.4f), 0, 0);
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity) 
        {
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			else {
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(244, 244, 244), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

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
