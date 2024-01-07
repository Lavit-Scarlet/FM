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

namespace FM.Content.Projectiles.Magic.Books
{
    public class BookofTrueFireProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 1;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);

            Main.spriteBatch.End();
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_1").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.07f);
            effect.Parameters["repeats"].SetValue(2f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}

        private readonly int NUMPOINTS = 12;
        public Color baseColor = new(255, 255, 100);
        public Color endColor = new(255, 0, 0);
        public Color EdgeColor = new(255, 55, 0);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 5.4f;

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 6)
				{
					Projectile.frame = 0;
				}
            }
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, EdgeColor, thickness);
            }
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				ParticleManager.NewParticle<EmberParticle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), Color.White, Main.rand.NextFloat(0.1f, 1f), 0, 0);
			}
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 25; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0);
				Main.dust[num].velocity *= 6f;
                Main.dust[num].scale = 2f;
				Main.dust[num].noGravity = true;
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