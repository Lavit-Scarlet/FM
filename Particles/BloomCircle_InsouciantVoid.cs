using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class BloomCircle_InsouciantVoid : Particle
	{
		public override string Texture => "FM/Particles/BloomCircle";
		
		public int timeLeftMax;
		public float size = 0f;
		public float sizeMultiplier;
		public float velRotation;
		public float rotNumber;
		
		public override void SetDefaults()
		{
			width = 64;
			height = 64;
			timeLeft = Main.rand.Next(20, 120);
			tileCollide = false;
			SpawnAction = Spawn;
			opacity = 1f;
			rotNumber = Main.rand.NextFloat(8, 11);
			
			sizeMultiplier = Main.rand.NextFloat(0.05f, 0.25f);
		}

		public override void AI()
		{
			Utils.Clamp(velocity.X, -2f, 2f);
			Utils.Clamp(velocity.Y, -2f, 2f);
			
			size -= 0.0005f;
			rotation = velocity.ToRotation();
			
			if (velRotation < 20)
				velRotation += 5;
			
			if (timeLeft <= timeLeftMax / 2f)
				opacity = MathHelper.Lerp(1f, 0f, (float)(timeLeftMax / 2f - timeLeft) / (timeLeftMax / 2f));
				
			velocity = velocity.RotatedBy(MathHelper.ToRadians(velRotation / rotNumber));
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/BloomCircle").Value;

			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width, height) / 2, new Vector2(1 + sizeMultiplier, 1 -  (sizeMultiplier * 0.035f)) * size, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

		
		private void Spawn()
		{
			timeLeftMax = timeLeft;
			size = Main.rand.NextFloat(1f, 4.5f) / 10f;
		}
	}
}