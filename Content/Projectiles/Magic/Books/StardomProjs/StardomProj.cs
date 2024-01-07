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
using FM.Content.Projectiles.Magic.Books.StardomProjs;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Magic.Books.StardomProjs
{
	public class StardomProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = Main.rand.Next(80, 160);
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
		}
        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);


			Texture2D texture = ModContent.Request<Texture2D>("FM/Assets/Textures/SoftGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 0, 255), Projectile.rotation, drawOrigin, Projectile.scale * 1f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


            Main.spriteBatch.End();
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
		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.2f, 1f));
		}
		private readonly int NUMPOINTS = 30;
        public Color baseColor = new(255, 100, 255);
        public Color endColor = new(150, 0, 150);
        public Color EdgeColor = new(234, 37, 210);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 3.5f;
		
        public override void AI()
        {
			if (Main.rand.NextBool(12))
			{
                Color color = Color.Lerp(Color.Purple, Color.White, 0.30f);
				for (int i = 0; i < 2; i++)
				{
					ParticleManager.NewParticle<StarParticle2>(Projectile.Center, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), color, Main.rand.NextFloat(0.07f, 0.1f), 0, 0);
				}
			}
			Projectile.velocity *= 0.99f;
			Projectile.rotation += Projectile.velocity.Length() * 0.03f * Projectile.direction;
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, EdgeColor, thickness);
            }
        }
        public override void Kill(int timeLeft)
		{
            Color color = Color.Lerp(Color.Purple, Color.White, 0.30f);
            int randParticles = Main.rand.Next(6, 12);
			for (int i = 0; i < randParticles; i++)
			{
				ParticleManager.NewParticle<StarParticle2>(Projectile.Center, new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f)), color, Main.rand.NextFloat(0.07f, 0.1f), 0, 0);
			}
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/StarExplosion"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 223, 0f, 0f, 0, default(Color), 0.5f);
				Main.dust[num].velocity *= 4f;
				Main.dust[num].noGravity = true;
			}
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<StardomExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
		}
	}
}
