using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class LineStreak_SwordImpact : Particle
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
			rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 360));
		}

		public override void AI()
		{
			velocity *= 0f;
			opacity -= 0.05f;
			scale *= 0.85f;

			if (opacity <= 0) {
				active = false;
			}
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/LineStreak_Long").Value;
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width, height) / 2, new Vector2(1f, 0.75f) * scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
	}
}