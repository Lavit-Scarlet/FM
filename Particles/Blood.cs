using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace FM.Particles
{
	public class Blood : Particle
	{
		public float sizeMultiplier;
		
		public override void SetDefaults()
		{
			width = 10;
			height = 10;
			timeLeft = 60000;
			tileCollide = true;
			opacity = 0.05f;
			gravity = 0.15f;
			
			sizeMultiplier = Main.rand.NextFloat(0.5f, 1.35f);
		}

		public override void AI()
		{
			if (!ChildSafety.Disabled)
				active = false;
			
			Utils.Clamp(velocity.X, -5f, 5f);
			Utils.Clamp(velocity.Y, -5f, 5f);
			
			if (opacity < 0.8f)
				opacity += 0.025f;
			
			scale *= 0.99f;
			
			if  (velocity.X == 0 && velocity.Y == 0)
				sizeMultiplier = 2f;
			else
				sizeMultiplier -=  0.002f;
			
			if (scale.X <= 0.005f)
				active = false;
			
			rotation = (velocity.X == 0 && velocity.Y == 0) ? 0 :velocity.ToRotation();
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			
			Texture2D particle = ModContent.Request<Texture2D>("FM/Particles/Blood").Value;
			
			
			spriteBatch.Draw(particle, VisualPosition, new Rectangle(0, 0, 64, 64), lightColor * opacity, rotation, new Vector2(particle.Width, particle.Height) / 2, new Vector2(1 + sizeMultiplier, 1 - (sizeMultiplier * 0.05f)) * scale, SpriteEffects.None, 0f);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
	}
}