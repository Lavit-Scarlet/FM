using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class HollowCircle_Misty_Small : Particle
	{
		public float size = 0f;
		public float sizeToAdd;
		public float randRotSpeed;
		
		public override string Texture => "FM/Particles/HollowCircle_Misty";
		
		public override void SetDefaults()
		{
			width = 250;
			height = 250;
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
			
			randRotSpeed *= 0.935f;
			rotation += randRotSpeed;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>(Texture).Value;
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, particle.Width, particle.Width), color * opacity, rotation, new Vector2(particle.Width, particle.Height) / 2, 1 * size * scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, particle.Width, particle.Width), color * opacity, rotation * -0.4f, new Vector2(particle.Width, particle.Height) / 2, 1 * size * scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, particle.Width, particle.Width), color * opacity, rotation * 0.625f, new Vector2(particle.Width, particle.Height) / 2, 1 * size * scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

		
		private void Spawn()
		{
			size = 0;
			rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 360));
			randRotSpeed = Main.rand.NextFloat(0.05f, 0.2f) * (Main.rand.NextBool(2) ? -1 : 1);
		}
	}
}