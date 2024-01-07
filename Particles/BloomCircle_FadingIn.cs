using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class BloomCircle_FadingIn : Particle
	{
		public override string Texture => "FM/Particles/BloomCircle";

		public float size = 0f;
		public float sizeMultiplier;
		
		public override void SetDefaults()
		{
			width = 64;
			height = 64;
			timeLeft = 300;
			tileCollide = false;
			SpawnAction = Spawn;
			opacity = 0f;
			
			sizeMultiplier = Main.rand.NextFloat(0.75f, 1.5f);
		}

		public override void AI()
		{
			Utils.Clamp(velocity.X, -2f, 2f);
			Utils.Clamp(velocity.Y, -2f, 2f);
			
			if (opacity < 1f) {
				opacity += 0.1f;
			}

			velocity *= 0.965f;
			
			size -= 0.0075f;
			sizeMultiplier -=  0.008f;
			
			if (size <= 0)
				active = false;
			
			rotation = velocity.ToRotation();
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/BloomCircle").Value;

			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width, height) / 2, new Vector2(1 + sizeMultiplier, 1 -  (sizeMultiplier * 0.035f)) * size * scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

		
		private void Spawn()
		{
			size = Main.rand.NextFloat(1f, 4.5f) / 10f;
		}
	}
}