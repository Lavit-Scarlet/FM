using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Effects.PrimitiveTrails;
using System.IO;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using FM.Effects;
using System.Collections.Generic;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Ranged.Guns
{
	public class LeichenfaustProjTest : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
		public override void SetDefaults()
		{
			Projectile.ignoreWater = true;
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = true;
		}
        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, 0, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);


            Texture2D texture = ModContent.Request<Texture2D>("FM/Assets/Textures/SoftGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(70, 255, 255), 0, drawOrigin, Projectile.scale * 1f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


            Texture2D texture2 = ModContent.Request<Texture2D>("FM/Content/Projectiles/Ranged/Guns/LeichenfaustProjTest_Circle", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin2 = new(texture2.Width / 2, texture2.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            for (int k = 0; k < 1; k++)
            {
                Main.EntitySpriteDraw(texture2, Projectile.Center - Main.screenPosition, null, new Color(70, 255, 255), Projectile.rotation += (float)Projectile.direction * 0.2f, drawOrigin2, 0.55f, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		public override void AI()
		{
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
            }
            
			Player player = Main.player[Projectile.owner];
			Vector2 center = Projectile.Center;
			float num8 = (float)player.miscCounter / 14f;
			float num7 = 1.0471975512f * 2;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					int num6 = Dust.NewDust(center, 0, 0, 156, 0f, 0f, 0, default(Color), 1.1f);
					Main.dust[num6].noGravity = true;
					Main.dust[num6].velocity = Vector2.Zero;
					Main.dust[num6].position = center + (num8 * 4.28318548f + num7 * (float)i).ToRotationVector2() * 12f;
				}
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 16;
            return true;
        }
		public override void Kill(int timeLeft)
		{
			ParticleManager.NewParticle<HollowCircle_Small>(Projectile.Center, Vector2.Zero, new Color(50, 255, 255, 150), 1f, 0, 0);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 100; i++)
			{
				float num627 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
				float num628 = Main.rand.NextFloat();
		    	Vector2 position7 = Projectile.Center + new Vector2(0, -0).RotatedBy(Projectile.rotation) + num627.ToRotationVector2() * (56f + 56f);
		    	Vector2 vector154 = (num627 + (float)Math.PI).ToRotationVector2() * 1;
		    	ParticleManager.NewParticle<BloomCircle_FadingIn>(position7, vector154 * 4, new Color(50, 255, 255), 1, 0, 0);
				
			}
			for (int p = 0; p < 60; p++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f)), new Color(50, 255, 255), Main.rand.NextFloat(0.4f, 0.8f), 0, 0);
			}
		}
	}
}
