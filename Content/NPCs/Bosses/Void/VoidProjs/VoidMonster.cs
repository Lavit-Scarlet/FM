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
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.NPCs.Bosses.Void.VoidProjs
{
    public class VoidMonster : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.hostile = true;
			Projectile.tileCollide = false;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 240;
            Projectile.penetrate = -1;
        }
        public override bool PreDraw(ref Color lightColor)
		{
            Main.spriteBatch.End();
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


			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, spriteEffects, 0);
			return false;
		}

        private readonly int NUMPOINTS = 40;
        public Color baseColor = new(130, 57, 125);
        public Color endColor = new(23, 12, 64);
        public Color edgeColor = new(77, 19, 105);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 5.6f;

        int timer = 0;
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.spriteDirection = Projectile.direction;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                float rotParticle = MathHelper.ToRadians(-20);
				int numberParticles = Main.rand.Next(6, 12);
				for (int i = 0; i < numberParticles; i++)
				{
                    ParticleManager.NewParticle<BloomCircle_PerfectNotRandScale>(Projectile.Center, new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f)), Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(0.5f, 1f), 0, 0);
                    ParticleManager.NewParticle<BloomCircle_PerfectNotRandScale>(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(1.8f, 2.4f), Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(0.5f, 1f), 0, 0);
                }
                FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Void"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .6f, .2f);
            }
			if (Main.rand.NextBool(3))
			{
				float rotParticle = MathHelper.ToRadians(-10);
				int numberParticles = 1;
				for (int i = 0; i < numberParticles; i++)
				{
                    ParticleManager.NewParticle<BloomCircle_PerfectNotRandScale>(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * 1.2f, Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(0.5f, 1f), 0, 0);
                    ParticleManager.NewParticle<BloomCircle_PerfectNotRandScale>(Projectile.Center, new Vector2(Main.rand.NextFloat(-.86f, .86f), Main.rand.NextFloat(-.86f, .86f)), Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(0.5f, 1f), 0, 0);
                }
			}

            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, edgeColor, thickness);
            }
        }
        public override void Kill(int timeLeft)
		{
            for (int i = 0; i < 10; i++)
                ParticleManager.NewParticle<BloomCircle_PerfectNotRandScale>(Projectile.Center, new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f)), Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(0.5f, 1f), 0, 0);
		}
    }
}