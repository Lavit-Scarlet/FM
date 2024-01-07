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
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Magic
{
    public class ElectroStaffProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.extraUpdates = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<Electric>(), 120);
        }
        public override void AI()
        {
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Projectile.netUpdate = true;
			NPC pre = null;
			if (FMHelper.ClosestNPC(ref pre, 200, Projectile.Center))
			{
			    float direction1 = Projectile.velocity.ToRotation();
			    direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(2));
			    Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
			}
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				for (int i = 0; i < 3; i++)
				{
					Color color = Color.Lerp(Color.Blue, Color.White, 0.56f);
					ParticleManager.NewParticle<HollowCircle_Small2>(Projectile.Center, Projectile.velocity, color, 0.4f, 0, 0);
				}
			}
        }

		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Request<Texture2D>("FM/Assets/Textures/62").Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);

            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            Color color = Projectile.GetAlpha(new Color(37, 72, 250, 0));

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

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void Kill(int timeLeft)
		{
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Color color = Color.Lerp(Color.Blue, Color.White, 0.56f);
            int randParticles = Main.rand.Next(15, 30);
			for (int i = 0; i < randParticles; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-4.4f, 4.4f), Main.rand.NextFloat(-4.4f, 4.4f)), color, Main.rand.NextFloat(0.6f, 0.8f), 0, 0);
			}
		}
    }
}
