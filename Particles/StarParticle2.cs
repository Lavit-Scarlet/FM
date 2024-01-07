using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class StarParticle2 : Particle
	{
		public float timeLeftMax;
		
		public override void SetDefaults()
		{
			width = 512;
			height = 512;
			timeLeft = Main.rand.Next(60, 120);
			tileCollide = false;
			opacity = 0f;
			rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 360));
			SpawnAction = Spawn;
		}

		public override void AI()
		{
			Utils.Clamp(velocity.X, -2f, 2f);
			Utils.Clamp(velocity.Y, -2f, 2f);
			
			if (opacity < 1f && timeLeft >= timeLeftMax - 10);
				opacity += 0.1f;
				
			velocity *= 0.965f;
			scale *= 0.965f;
			
			rotation += velocity.X / 24;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/StarParticle2").Value;

			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width, height) / 2, scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		
		private void Spawn()
		{
			timeLeftMax = timeLeft;
		}
	}
}