using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class BloomCircle_Environmental : Particle
	{
		public int timer = Main.rand.Next(50, 100);
		public float speedX = Main.rand.NextFloat(4f, 9f);
		public float mult = Main.rand.NextFloat(10f, 31f) / 200f;
		public int timeLeftMax;
		public float size = 0f;

		public float randomMulti;

		public override string Texture => "FM/Particles/BloomCircle";
		
		public override void SetDefaults()
		{
			width = 64;
			height = 64;
			timeLeft = Main.rand.Next(151, 480);
			tileCollide = false;
			oldPos = new Vector2[3];
			SpawnAction = Spawn;
			layer = Layer.AfterUI;
			opacity = 0;

			randomMulti = Main.rand.NextFloat(0.05f, 0.1f);
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
				// Add the sine component to the velocity.
				// This is scaled by the mult, which changes every cycle.
				velocity += new Vector2(sineX * mult, -Main.rand.NextFloat(0.5f, 1f) / 200f);

				// Clamp the velocity so the particle doesnt go too fast.
				Utils.Clamp(velocity.X, -2f, 2f);
				Utils.Clamp(velocity.Y, -2f, 2f);

				// Decrement the timer
				timer--;

				// Halfway through, start fading.
				if (timeLeft <= timeLeftMax / 2f)
				{
					opacity = MathHelper.Lerp(1f, 0f, (float)(timeLeftMax / 2f - timeLeft) / (timeLeftMax / 2f));
					randomMulti *= 0.995f;
				}
				else 
					if (opacity < 1f)
						opacity += 0.025f;
			}
			
			rotation = velocity.ToRotation();
			ai[0]--;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/BloomCircle").Value;

			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width, height) / 2, new Vector2(1 + randomMulti, 1 - randomMulti) * size * scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		
		private void Spawn()
		{
			timeLeftMax = timeLeft;
			size = (Main.rand.NextFloat(2f, 5f) / 10f) * scale.X;
		}
		
		private void NewMovementCycle()
		{
			timer = Main.rand.Next(50, 100);
			speedX = Main.rand.NextFloat(0.75f, 3f);
			mult = Main.rand.NextFloat(7f, 15f) / 200f;
		}
	}
}