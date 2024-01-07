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
using FM.Content.Projectiles.Melee.Spears.HeavenlySpearProjs;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Melee.Spears.HeavenlySpearProjs
{
	public class HeavenlySpearShootProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1f;
		}
		public override bool? CanCutTiles() => false;
		public float rot;

		private readonly int NUMPOINTS = 24;
        public Color baseColor = new(244, 244, 244);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 4f;
        public override void AI()
        {
            if (Main.netMode != NetmodeID.Server)
            {
            	TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
        		TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, baseColor, baseColor, thickness);
        	}
            Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
            if (Main.rand.NextBool(10))
            {
                float rotParticle = MathHelper.ToRadians(-24);
				float numberParticles = Main.rand.Next(1, 5);
				for (int i = 0; i < numberParticles; i++)
				{
					ParticleManager.NewParticle<BloomCircle>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.4f), Color.White, Main.rand.NextFloat(0.1f, 0.7f), 0, 0);
				}
            }
            Vector2 move = Vector2.Zero;
            float distance = 600f;
            bool targetted = false;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (!target.CanBeChasedBy())
                    continue;

                Vector2 newMove = target.Center - Projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                if (distanceTo < distance)
                {
                    move = target.Center;
                    distance = distanceTo;
                    targetted = true;
                }
            }
            if (targetted)
                Projectile.Move(move, Projectile.timeLeft > 50 ? 30 : 50, 50);
        }
		public override void Kill(int timeLeft)
		{
            float rotParticle = MathHelper.ToRadians(-40);
			float numberParticles = Main.rand.Next(1, 20);
			for (int i = 0; i < numberParticles; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.2f), Color.White, Main.rand.NextFloat(0.3f, 0.8f), 0, 0);
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Slashing"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .4f, -0.10f);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity / 6, ModContent.ProjectileType<HeavenlySpearCuttingProj>(), Projectile.damage, 0, Projectile.owner);
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
