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


namespace FM.Content.Projectiles.Magic
{
	public class WaterStaffProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = 9; 
			Projectile.timeLeft = 6000;
			Projectile.DamageType = DamageClass.Magic;
		}

		private readonly int NUMPOINTS = 30;
        public Color baseColor = new(170, 172, 235);
        public Color endColor = new(22, 9, 138);
		public Color edgeColor = new(22, 9, 138);

        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 6f;

		public override void AI()
		{
			Projectile.rotation += (float)Projectile.direction * 0.3f;
			if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f)
			{
				Player player = Main.player[Projectile.owner];
				if (player.channel)
				{
					float maxDistance = 30f;
					Vector2 vectorToCursor = Main.MouseWorld - Projectile.Center;
					float distanceToCursor = vectorToCursor.Length();

					if (distanceToCursor > maxDistance)
					{
						distanceToCursor = maxDistance / distanceToCursor;
						vectorToCursor *= distanceToCursor;
					}

					int velocityXBy1000 = (int)(vectorToCursor.X * 1000f);
					int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);
					int velocityYBy1000 = (int)(vectorToCursor.Y * 1000f);
					int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);

					if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
					{
						Projectile.netUpdate = true;
					}

					Projectile.velocity = vectorToCursor;
				}
				else if (Projectile.ai[0] == 0f)
				{
					Projectile.timeLeft -= 5400;
					Projectile.netUpdate = true;

					float maxDistance = 30f; 
					Vector2 vectorToCursor = Main.MouseWorld - Projectile.Center;
					float distanceToCursor = vectorToCursor.Length();

					if (distanceToCursor == 0f)
					{
						vectorToCursor = Projectile.Center - player.Center;
						distanceToCursor = vectorToCursor.Length();
					}

					distanceToCursor = maxDistance / distanceToCursor;
					vectorToCursor *= distanceToCursor;

					Projectile.velocity = vectorToCursor;

					if (Projectile.velocity == Vector2.Zero)
					{
						Projectile.Kill();
					}
					Projectile.ai[0] = 1f;
				}
			}
			if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, edgeColor, thickness);
            }
		}
		public override bool PreDraw(ref Color lightColor)
        {
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

            return true;
        }
        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.GemSapphire, 0, 0, Scale: 1.4f);
                Main.dust[dust].velocity *= 2f;
                Main.dust[dust].noGravity = true;
            }
		}
	}
}
