using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class HollowCircle : Particle
	{
		public float size = 0f;
		public float sizeToAdd;
		
		public override string Texture => "FM/Particles/HollowCircle";
		
		public override void SetDefaults()
		{
			width = 1600;
			height = 1600;
			timeLeft = 300;
			tileCollide = false;
			SpawnAction = Spawn;
			opacity = 1f;
		}

		public override void AI()
		{
			velocity *= 0f;
			
			size += sizeToAdd;
			
			if (size < 0.5f) {
				sizeToAdd = 0.05f;
			}
			else if (size >= 0.5f)
			{
				sizeToAdd *= 0.96f;
				opacity -= 0.01f;
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
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width, height) / 2, 1 * size * scale, SpriteEffects.None, 0f);
			
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