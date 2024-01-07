using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class Snowflake : Particle
	{
		public override string Texture => "FM/Particles/Snowflake";

		public float timeLeftMax;
		public int frameNum;
		
		public override void SetDefaults()
		{
			width = 64;
			height = 64;
			timeLeft = Main.rand.Next(71, 294);
			tileCollide = false;
			opacity = 0f;
			rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 360));
			SpawnAction = Spawn;
			frameNum = Main.rand.Next(0, 3);
		}

		public override void AI()
		{
			if (timeLeft <= timeLeftMax / 2f) {
				opacity = MathHelper.Lerp(0.75f, 0f, (float)(timeLeftMax / 2f - timeLeft) / (timeLeftMax / 2f));
			}
			else {
				if (opacity < 0.75f)
					opacity += 0.05f;
			}
				
			velocity *= 0.988f;
			rotation += velocity.X / 18;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/Snowflake").Value;

			int y3 = height * frameNum;
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, y3, width, height), color * opacity, rotation, new Vector2(width, height) / 2, scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		
		private void Spawn()
		{
			timeLeftMax = timeLeft;
		}
	}
}