using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class LensFlare : Particle
	{
		public override string Texture => "FM/Particles/LensFlare";
		
		public float scaleMax;
		
		public override void SetDefaults()
		{
			width = 1600;
			height = 1600;
			timeLeft = 10;
			tileCollide = false;
			SpawnAction = Spawn;
			opacity = 1f;
		}

		public override void AI()
		{
			velocity *= 0f;
			
			if (timeLeft <= 10 / 2f)
			{
				scale.X = MathHelper.Lerp(scaleMax, 0f, (float)(10 - timeLeft) / 10);
				scale.Y = MathHelper.Lerp(scaleMax, 0f, (float)(10 - timeLeft) / 10);
				opacity = MathHelper.Lerp(1f, 0f, (float)(10 - timeLeft) / 10);
			}
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D lensFlare = ModContent.Request<Texture2D>("FM/Particles/LensFlare").Value;
			
			spriteBatch.Draw(lensFlare, VisualPosition, new Rectangle(0, 0, lensFlare.Width, lensFlare.Width), color * opacity, rotation, new Vector2(lensFlare.Width, lensFlare.Height) / 2, 1 * scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		
		private void Spawn()
		{
			scaleMax = scale.X;
		}
	}
}