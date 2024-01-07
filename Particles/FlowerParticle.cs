using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class FlowerParticle : Particle
	{
		public float size = 0f;
		public float sizeToAdd;
		
		public override string Texture => "FM/Particles/FlowerParticle";
		
		public override void SetDefaults()
		{
			width = 512;
			height = 512;
			timeLeft = 300;
			tileCollide = false;
			SpawnAction = Spawn;
			opacity = 1f;
		}

		public override void AI()
		{
			velocity *= 0f;
			
			size += sizeToAdd;
			
			if (size < 0.20f) {
				sizeToAdd = 0.075f;
			}
			else if (size >= 0.20f)
			{
				sizeToAdd *= 0.9f;
				opacity -= 0.0325f;
			}
			
			if (size >= 3f)
				active = false;
			
			rotation = velocity.ToRotation();
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>(Texture).Value;

			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, particle.Width, particle.Height), color * opacity, rotation, new Vector2(particle.Width, particle.Height) / 2, 1 * size * scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

		
		private void Spawn()
		{
			size = 0;
		}
	}
}