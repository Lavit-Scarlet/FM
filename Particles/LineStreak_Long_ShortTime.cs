using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class LineStreak_Long_ShortTime : Particle
	{
		public int timeLeftMax;
		
		public override string Texture => "FM/Particles/LineStreak_Long";
		
		public override void SetDefaults()
		{
			width = 150;
			height = 150;
			timeLeft = 600;
			tileCollide = false;
			oldPos = new Vector2[3];
		}

		public override void AI()
		{
			velocity *= 0.9f;
			opacity -= 0.05f;
			scale *= 0.92f;

			if (opacity <= 0) {
				active = false;
			}
			
			rotation = velocity.ToRotation();
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>(Texture).Value;
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width - width, height) / 2, new Vector2(1.35f, 0.4f) * scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
	}
}