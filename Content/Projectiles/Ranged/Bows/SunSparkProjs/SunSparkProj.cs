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
using FM.Content.Projectiles.Ranged.Bows.SunSparkProjs;

namespace FM.Content.Projectiles.Ranged.Bows.SunSparkProjs
{
    public class SunSparkProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = 1;
        }
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Request<Texture2D>("FM/Assets/Textures/62").Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);

            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            Color color = Projectile.GetAlpha(new Color(250, 72, 37, 0));

            frameHeight = texture.Height / Main.projFrames[Projectile.type];
            frameY = frameHeight * Projectile.frame;

            sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);

            origin = sourceRectangle.Size() / 2f;

            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Main.EntitySpriteDraw(texture, Projectile.oldPos[i] - Projectile.position + Projectile.Center - Main.screenPosition, sourceRectangle, color, Projectile.oldRot[i] + MathHelper.PiOver2, origin, Projectile.scale - (i / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }



            texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(93, 175, 234), Projectile.rotation, drawOrigin, Projectile.scale * 1f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
        }
		public override void Kill(int timeLeft)
		{
			for (int p = 0; p < 12; p++)
			{
                Color color = Color.Lerp(Color.Yellow, Color.White, 0.36f);
                ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f)), color, Main.rand.NextFloat(0.4f, 1f), 0, 0);
			}
            ParticleManager.NewParticle<EmberParticle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), Color.White, Main.rand.NextFloat(0.1f, 1f), 0, 0);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int d = 0; d < 10; d++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0);
				Main.dust[num].velocity *= 6f;
                Main.dust[num].scale = 2f;
				Main.dust[num].noGravity = true;
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
			}
			Vector2 velocity = new Vector2(0.1f * Math.Sign(Projectile.velocity.X), -7f);
			Vector2 position = new Vector2(Projectile.position.X, Projectile.position.Y + 7);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), position, velocity, ProjectileType<SunSparkProj2Spawn>(), Projectile.damage, 0, Main.player[Projectile.owner].whoAmI);
		}
    }
}