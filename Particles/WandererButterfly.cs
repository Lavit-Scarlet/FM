using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class WandererButterfly : Particle
	{
		public int timer = Main.rand.Next(50, 100);
		public float speedX = Main.rand.NextFloat(1f, 5f);
		public float mult = Main.rand.NextFloat(5f, 10f) / 200f;
		public int timeLeftMax;
		public float size = 0f;
		public float maxOpacity;
		public int frameNum;
		public int frameRandMax;
		public int frameCounter;

		public override void SetDefaults()
		{
			width = 13;
			height = 12;
			timeLeft = Main.rand.Next(180, 300);
			tileCollide = false;
			oldPos = new Vector2[3];
			opacity = 0;
			SpawnAction = Spawn;
			
			if (Main.rand.NextFloat() <= 0.40f)
				layer = Layer.BeforePlayers;
			
			frameNum = Main.rand.Next(0, 5);
			frameRandMax = Main.rand.Next(2, 4);
			frameCounter = Main.rand.Next(0, 5);
			maxOpacity = Main.rand.NextFloat(0.5f, 1f);
		}

		public override void AI()
		{
			Vector2 center = new Vector2(width, height) * 0.5f;
			Lighting.AddLight(center, 0.1f * opacity * size, 0.35f * opacity * size, 0.6f * opacity * size);
			
			// You can pass in a number to determine how long until it starts its ember movement.
			if (ai[0] <= 0)
			{
				float sineX = (float)Math.Sin(Main.GlobalTimeWrappedHourly / speedX);
				
				// Makes the particle change directions or speeds.
				// Timer is used for keeping track of the current cycle
				if (timer == 0)
					NewMovementCycle();

				// Adds the wind velocity to the particle.
				// It adds less the faster it is already going.
				velocity += new Vector2((Main.windSpeedCurrent * 0.5f) * (Main.windPhysicsStrength * 0.5f) * MathHelper.Lerp(1f, 0.1f, Math.Abs(velocity.X) / 8f), 0f);
				// Add the sine component to the velocity.
				// This is scaled by the mult, which changes every cycle.
				velocity += new Vector2(sineX * mult, -Main.rand.NextFloat(0.5f, 1f) / 100f);

				// Clamp the velocity so the particle doesnt go too fast.
				Utils.Clamp(velocity.X, -1f, 0.5f);
				Utils.Clamp(velocity.Y, -0.5f, -0.25f);
				
				if (velocity.X < -1f)
					velocity.X = -1f;
				
				if (velocity.X > 1f)
					velocity.X = 1f;
				
				if (velocity.Y < -0.8f)
					velocity.Y = -0.8f;
				
				// Decrement the timer
				timer--;

				// Halfway through, start fading.
				if (timeLeft <= timeLeftMax / 2f)
				{
					velocity *= 0.95f;
					opacity = MathHelper.Lerp(maxOpacity * 1f, maxOpacity * 0f, (float)(timeLeftMax / 2f - timeLeft) / (timeLeftMax / 2f));
				}
				else if (timeLeft > timeLeftMax / 3f && ai[0] < -5)
				{
					if (opacity < maxOpacity)
						opacity += 0.05f * maxOpacity;
				}
			}
			
			if (++frameCounter > frameRandMax)
			{
				frameNum++;
				frameCounter = 0;
			}
			
			if (frameNum > 5)
				frameNum = 0;
			
			rotation = velocity.ToRotation() + MathHelper.ToRadians(90);
			ai[0]--;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/WandererButterfly").Value;
			Texture2D glow = ModContent.Request<Texture2D>("FM/Particles/Light").Value;

			Color color = Color.Multiply(new(255, 255, 255), opacity);
			Color glowColor = Color.LightBlue * (opacity * 0.5f);
			
            int y3 = height * frameNum;
			
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, y3, width, height), color, rotation, new Vector2(width, height) / 2, size, SpriteEffects.None, 0f);
			spriteBatch.Draw(glow, VisualPosition, new Rectangle(0, 0, 50, 50), glowColor, rotation, new Vector2(width + 37, height + 38) / 2, size * 0.5f, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		
		private void Spawn()
		{
			timeLeftMax = timeLeft;
			size = Main.rand.NextFloat(7f, 12f) / 10f;
		}
		
		private void NewMovementCycle()
		{
			timer = Main.rand.Next(50, 100);
		}
	}
	
	public class WandererButterflyTeleport : Particle
	{
		public override string Texture => "FM/Particles/WandererButterfly";
		
		public int timer = Main.rand.Next(50, 100);
		public float speedX = Main.rand.NextFloat(4f, 9f);
		public float mult = Main.rand.NextFloat(10f, 31f) / 200f;
		public int timeLeftMax;
		public float size = 0f;
		public float maxOpacity;
		public int frameNum;
		public int frameRandMax;
		public int frameCounter;

		public override void SetDefaults()
		{
			width = 13;
			height = 12;
			timeLeft = Main.rand.Next(120, 300);
			tileCollide = false;
			oldPos = new Vector2[3];
			opacity = 0;
			SpawnAction = Spawn;
			
			if (Main.rand.NextFloat() <= 0.40f)
				layer = Layer.BeforePlayers;
			
			frameNum = Main.rand.Next(0, 5);
			frameRandMax = Main.rand.Next(3, 8);
			frameCounter = Main.rand.Next(0, 5);
			maxOpacity = Main.rand.NextFloat(0.5f, 1f);
		}

		public override void AI()
		{
			Vector2 center = new Vector2(width, height) * 0.5f;
			Lighting.AddLight(center, 0.1f * opacity * size, 0.35f * opacity * size, 0.6f * opacity * size);
			
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
					opacity = MathHelper.Lerp(0.75f, 0f, (float)(timeLeftMax / 2f - timeLeft) / (timeLeftMax / 2f));
				}
				else 
					if (opacity < 0.75f)
						opacity += 0.025f;
			}
			
			if (++frameCounter > frameRandMax)
			{
				frameNum++;
				frameCounter = 0;
			}
			
			if (frameNum > 5)
				frameNum = 0;
			
			rotation = velocity.ToRotation() + MathHelper.ToRadians(90);
			ai[0]--;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/WandererButterfly").Value;
			Texture2D glow = ModContent.Request<Texture2D>("FM/Particles/Light").Value;

			Color color = Color.Multiply(new(255, 255, 255), opacity);
			Color glowColor = Color.LightBlue * (opacity * 0.5f);
			
            int y3 = height * frameNum;
			
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, y3, width, height), color, rotation, new Vector2(width, height) / 2, size, SpriteEffects.None, 0f);
			spriteBatch.Draw(glow, VisualPosition, new Rectangle(0, 0, 50, 50), glowColor, rotation, new Vector2(width + 37, height + 38) / 2, size * 0.5f, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		
		private void Spawn()
		{
			timeLeftMax = timeLeft;
			size = Main.rand.NextFloat(7f, 12f) / 10f;
		}
		
		private void NewMovementCycle()
		{
			timer = Main.rand.Next(50, 100);
			speedX = Main.rand.NextFloat(0.75f, 3f);
			mult = Main.rand.NextFloat(7f, 15f) / 200f;
		}
	}
}