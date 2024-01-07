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

namespace FM.Content.Projectiles.Ranged.Bows.SunSparkProjs
{
    public class SunSparkProj2Spawn : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.timeLeft = 64;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 4;
            Projectile.tileCollide = false;
        }
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale * 2f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void Kill(int timeLeft)
        {
			if(Main.myPlayer == Projectile.owner)
            {
                Color color = Color.Lerp(Color.Yellow, Color.White, 0.36f);
                for (int p = 0; p < 3; p++)
                {
                    ParticleManager.NewParticle<BloomCircle_PerfectNotRandScale>(Projectile.Center, Vector2.Zero, color, 1.8f, 0, 0);
                    ParticleManager.NewParticle<HollowCircle_Small>(Projectile.Center, Vector2.Zero, color, 0.8f, 0, 0);
                }
                SoundEngine.PlaySound(SoundID.Item105, Projectile.position);
				for (int p = 0; p < 30; p++)
				{
        	        ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-12f, 12f), Main.rand.NextFloat(-12f, 12f)), color, Main.rand.NextFloat(0.5f, 1f), 0, 0);
				}
				for(int i = -4; i <= 4; i++)
				{
					Vector2 velocity = new Vector2(i, 9 - Math.Abs(i) * 0.4f) * 0.3f;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<SunSparkProj2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, -1, 0);
				}
            }
        }
    }
}