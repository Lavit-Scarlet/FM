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
	public class HeavenlyBowProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 12;
            Projectile.height = 12;
            Projectile.penetrate = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}
		private readonly int NUMPOINTS = 38;
        public Color baseColor = new(240, 240, 240);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 2.8f;
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
            float rotParticle = MathHelper.ToRadians(-60);
			float numberParticles = Main.rand.Next(2, 8);
			for (int i = 0; i < numberParticles; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 1f), Color.White, Main.rand.NextFloat(0.3f, 0.8f), 0, 0);
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.myPlayer == Projectile.owner && hit.Crit)
			{
                FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/HeavenlyShoot"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .50f, 0.6f);
				float theta = (float)Main.rand.NextDouble() * 3.14f * 2;
				float mag = 120;
				
				theta = (float)Main.rand.NextDouble() * 3.14f * 2;
				mag = 320;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center.X + (int)(mag * Math.Cos(theta)), target.Center.Y + (int)(mag * Math.Sin(theta)), -8 * (float)Math.Cos(theta), -8 * (float)Math.Sin(theta), ProjectileType<HeavenlyBowCritProj>(), Projectile.damage, 0, Main.myPlayer);
			}
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(240, 240, 240), Projectile.rotation, drawOrigin, new Vector2(0.44f, 1.8f), SpriteEffects.None, 0);

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
