using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class LineStreak_FireButEnvironmental : Particle
	{
		public int timer = Main.rand.Next(50, 100);
		public float speedX = Main.rand.NextFloat(4f, 9f);
		public float mult = Main.rand.NextFloat(10f, 31f) / 200f;
		public int timeLeftMax;
		public float size = 0f;
		public float randomMulti;

		public override string Texture => "FM/Particles/LineStreak";
		
		public override void SetDefaults()
		{
			width = 35;
			height = 35;
			timeLeft = Main.rand.Next(90, 121);
			tileCollide = false;
			oldPos = new Vector2[3];
			SpawnAction = Spawn;
			layer = Layer.AfterUI;
			randomMulti = Main.rand.NextFloat(0.25f, 1f);
		}

		public override void AI()
		{
			// You can pass in a number to determine how long until it starts its ember movement.
			if (ai[0] <= 0)
			{
				float sineX = (float)Math.Sin(Main.GlobalTimeWrappedHourly * speedX);
				
				// Makes the particle change directions or speeds.
				// Timer is used for keeping track of the current cycle
				if (timer == 0)
					NewMovementCycle();

				// Adds the wind velocity to the particle.
				// It adds less the faster it is already going.
				velocity += new Vector2(Main.windSpeedCurrent * (Main.windPhysicsStrength * 3f) * MathHelper.Lerp(1f, 0.1f, Math.Abs(velocity.X) / 6f), 0f);
				// Add the sine component to the velocity.
				// This is scaled by the mult, which changes every cycle.
				velocity += new Vector2(sineX * mult, -Main.rand.NextFloat(2.5f, 4f) / 100f);

				// Clamp the velocity so the particle doesnt go too fast.
				Utils.Clamp(velocity.X, -4f, 4f);
				Utils.Clamp(velocity.Y, -15f, -3f);

				// Decrement the timer
				timer--;

				// Halfway through, start fading.
				if (timeLeft <= timeLeftMax / 2f)
					opacity = MathHelper.Lerp(1f, 0f, (float)(timeLeftMax / 2f - timeLeft) / (timeLeftMax / 2f));
			}
			
			rotation = velocity.ToRotation();
			ai[0]--;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/LineStreak").Value;

			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width, height) / 2, new Vector2(1 * randomMulti, 1) * size, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		
		private void Spawn()
		{
			timeLeftMax = timeLeft;
			size = Main.rand.NextFloat(8f, 15f) / 10f;
		}
		
		private void NewMovementCycle()
		{
			timer = Main.rand.Next(50, 100);
			speedX = Main.rand.NextFloat(4f, 9f);
			mult = Main.rand.NextFloat(10f, 31f) / 200f;
		}
	}
}