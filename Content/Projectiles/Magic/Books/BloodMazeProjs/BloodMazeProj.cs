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
using FM.Content.Projectiles.Magic.Books.BloodMazeProjs;

namespace FM.Content.Projectiles.Magic.Books.BloodMazeProjs
{
	public class BloodMazeProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = 15;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
        }
        private readonly int NUMPOINTS = 30;
        public Color baseColor = new(232, 106, 106);
        public Color endColor = new(93, 20, 20);
        public Color EdgeColor = new(162, 42, 42);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 3.45f;
        public override void AI()
        {
			Projectile.rotation += (float)Projectile.direction * 0.3f;
			Player player = Main.player[Projectile.owner];
			Projectile.ai[0] -= 1.1f;
			if (Projectile.localAI[1] == 0f)
			{
				Projectile.localAI[1] = 1f;
				/*float count = 40.0f;
				for (int k = 0; (double)k < (double)count; k++)
				{
					Vector2 vector2 = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double)k * (6.22 / (double)count), new Vector2()) * new Vector2(2.0f, 14.0f)).RotatedBy((double)Projectile.velocity.ToRotation(), new Vector2());
					int dust = Dust.NewDust(Projectile.Center - new Vector2(0.0f, 4.0f), 0, 0, 182, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[dust].scale = 0.7f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position = Projectile.Center + vector2;
					Main.dust[dust].velocity = Projectile.velocity * 0.0f + vector2.SafeNormalize(Vector2.UnitY) * 2.0f;
				}*/
				for (int i = 0; i < 3; i++)
				{
					Color color = Color.Lerp(Color.Red, Color.Pink, 0.5f);
					ParticleManager.NewParticle<HollowCircle_Small2>(Projectile.Center, Projectile.velocity, color, 0.4f, 0, 0);
				}
			}
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, EdgeColor, endColor, baseColor, thickness, true);
            }
        }
        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);
		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.1f, 0.1f));
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
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);


            Texture2D texture = ModContent.Request<Texture2D>("FM/Assets/Textures/SoftGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(232, 106, 106), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) 
        {
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) 
			{
				Projectile.Kill();
			}
			else 
			{
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

				if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) 
				{
					Projectile.velocity.X = -oldVelocity.X;
				}

				if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) 
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
        }

        public override void Kill(int timeLeft)
		{
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			for (int i = 0; i < 3; i++)
			{
				Color color = Color.Lerp(Color.Red, Color.Pink, 0.4f);
				ParticleManager.NewParticle<FlowerParticle>(Projectile.Center, Projectile.velocity, color, 0.6f, 0, 0);
			}
			//DustHelper.DrawStar(Projectile.Center, 182, 5, 0.8f, 5, 1.4f, 1.4f, 1f, true);
		}
	}
}
