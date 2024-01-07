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
using FM.Content.NPCs.Bosses.Void.VoidProjs;

namespace FM.Content.NPCs.Bosses.Void.VoidProjs
{
    public class VoidBigBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.hostile = true;
			Projectile.tileCollide = false;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.timeLeft = 120;
            Projectile.penetrate = -1;
            Projectile.scale = 2f;
            Projectile.extraUpdates = 1;
        }

		private readonly int NUMPOINTS = 40;

        public Color baseColor = new(130, 57, 125);
        public Color endColor = new(23, 12, 64);
        public Color edgeColor = new(77, 19, 105);

        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 4.4f;

        public override void AI()
        {
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, edgeColor, thickness);
            }
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 7) 
			{
				Projectile.frameCounter = 0;
				for (int i = 0; i < 1; i++)
				{
					ParticleManager.NewParticle<HollowCircle_Small2>(Projectile.Center, Projectile.velocity, Color.Lerp(new Color(130, 57, 125), new Color(23, 12, 64), 0.56f), 0.5f, 0, 0);
				}
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(23, 12, 64), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255), Projectile.rotation, drawOrigin, Projectile.scale / 1.5f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


            Main.spriteBatch.End();//Trail
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_3").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.05f);
            effect.Parameters["repeats"].SetValue(3f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }
        public override void Kill(int timeLeft)
		{
            int n = 8;
            int deviation = Main.rand.Next(-180, 180);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 5f;
                perturbedSpeed.Y *= 5f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, perturbedSpeed, ProjectileType<VoidBolt>(), Projectile.damage, 0, Projectile.owner);
            }
            for (int i = 0; i < 8; i++)
                ParticleManager.NewParticle<BloomCircle_PerfectNotRandScale>(Projectile.Center, new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f)), Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(0.5f, 1f), 0, 0);
		}
    }
}
