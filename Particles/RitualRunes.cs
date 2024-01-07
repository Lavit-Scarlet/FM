using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class RitualRunes : Particle
	{
		public int frameNum;
		
		public override string Texture => "FM/Particles/RitualRunes";
		
		public override void SetDefaults()
		{
			width = 80;
			height = 80;
			timeLeft = 300;
			tileCollide = false;
			opacity = 0.05f;
			layer = Layer.BeforeUI;
			 
			frameNum = Main.rand.Next(0, 23);
		}

		public override void AI()
		{
			Utils.Clamp(velocity.X, -2f, 2f);
			Utils.Clamp(velocity.Y, -2f, 2f);
			
			velocity *= 0.98f;
			
			if (opacity < 1f)
				opacity += 0.1f;
			
			scale.X *= 0.96f;
			scale.Y *= 0.96f;
			
			if (scale.X <= 0.01f)
				active = false;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>(Texture).Value;

            int y3 = height * frameNum;
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, y3, width, height), color * opacity, rotation, new Vector2(width, height) / 2, scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
	}
}