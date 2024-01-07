using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class BloomCircle_Gravity : Particle
	{
		public override string Texture => "FM/Particles/BloomCircle";
		
		public int timeLeftMax;
		public float size = 0f;
		
		public float sizeMultiplier;

		public override void SetDefaults()
		{
			width = 64;
			height = 64;
			timeLeft = Main.rand.Next(90, 180);
			SpawnAction = Spawn;
			opacity = 1f;
			gravity = 0.1f;
			
			sizeMultiplier = Main.rand.NextFloat(0.5f, 1.5f);
		}

		public override void AI()
		{
			size -= 0.00075f;
			
			if  (velocity.X == 0 && velocity.Y == 0)
				sizeMultiplier = 2f;
			else
				sizeMultiplier -=  0.002f;
			
			if (timeLeft <= timeLeftMax / 2f)
				opacity = MathHelper.Lerp(1f, 0f, (float)(timeLeftMax / 2f - timeLeft) / (timeLeftMax / 2f));
				
			rotation = (velocity.X == 0 && velocity.Y == 0) ? 0 :velocity.ToRotation();
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/BloomCircle").Value;

			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width + 53, height + 43) / 2, new Vector2(1 + sizeMultiplier, 1 - (sizeMultiplier * 0.035f)) * size, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

		
		private void Spawn()
		{
			timeLeftMax = timeLeft;
			size = (Main.rand.NextFloat(1f, 4f) / 10f) * scale.X;
		}
	}
}