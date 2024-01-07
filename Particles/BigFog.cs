using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class BigFog : Particle
	{
		public float timeLeftMax;
		public float randRotMultiplier;
		
		public override void SetDefaults()
		{
			width = 256;
			height = 256;
			timeLeft = Main.rand.Next(96, 204);
			tileCollide = false;
			opacity = 0f;
			rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 360));
			SpawnAction = Spawn;
		}

		public override void AI()
		{
			Utils.Clamp(velocity.X, -2f, 2f);
			Utils.Clamp(velocity.Y, -2f, 2f);
			
			if (opacity < 0.5f && timeLeft > timeLeftMax / 2f) {
				opacity += 0.05f;
			}
			else if (timeLeft <= timeLeftMax / 2f) {
				opacity = MathHelper.Lerp(0.5f, 0f, (float)(timeLeftMax / 2f - timeLeft) / (timeLeftMax / 2f));
			}
			
			rotation += MathHelper.ToRadians(velocity.X * randRotMultiplier);
			velocity *= 0.98f;
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/BigFog").Value;
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, width, height), color * opacity, rotation, new Vector2(width, height) / 2, scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		
		private void Spawn()
		{
			randRotMultiplier = Main.rand.NextFloat(0.1f, 0.5f);
			timeLeftMax = timeLeft;
		}
	}
}