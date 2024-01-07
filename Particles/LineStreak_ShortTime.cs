using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class LineStreak_ShortTime : Particle
	{
		public float size = 0f;
		public float randomMulti;
		
		public override string Texture => "FM/Particles/LineStreak";
		
		public override void SetDefaults()
		{
			width = 35;
			height = 35;
			timeLeft = 600;
			tileCollide = false;
			oldPos = new Vector2[3];
			size = Main.rand.NextFloat(0.5f, 1.5f);
			
			randomMulti = Main.rand.NextFloat(0.1f, 0.5f);
		}

		public override void AI()
		{
			velocity *= 0.95f;
			opacity -= 0.1f;
			size *= 0.99f;
			
			if (opacity <= 0)
				active = false;
			
			randomMulti *= 0.9f;
			
			if (randomMulti < 0.1f) {
				randomMulti = 0.1f;
			}
			
			rotation = velocity.ToRotation();
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/LineStreak").Value;

			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width, height) / 2, new Vector2(1.5f * randomMulti, 0.75f) * size * new Vector2(scale.X, scale.Y), SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
	}
}