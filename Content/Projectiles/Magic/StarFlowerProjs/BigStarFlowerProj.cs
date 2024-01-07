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

namespace FM.Content.Projectiles.Magic.StarFlowerProjs
{
    public class BigStarFlowerProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
        }
        public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(2f, 1.6f, 0.1f));
		}
        public override bool PreDraw(ref Color lightColor)
		{
            Texture2D texture = ModContent.Request<Texture2D>("FM/Assets/Textures/SoftGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 224, 69), Projectile.rotation, drawOrigin, Projectile.scale * 1.7f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
            Color ColorDV = new Color(255, 224, 69); ColorDV.A = 0;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), ColorDV, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);


            Main.spriteBatch.End();
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/StarTrail").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.05f);
            effect.Parameters["repeats"].SetValue(2f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}
        public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        private readonly int NUMPOINTS = 60;
        public Color baseColor = new(255, 255, 100);
        public Color endColor = new(255, 100, 0);
        public Color EdgeColor = new(255, 255, 0);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 8f;

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, 0.255f, .199f, .069f);
            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, EdgeColor, thickness);
            }
        }
		public override void Kill(int timeLeft)
		{
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/StarExplosion"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 45; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 269, 0f, 0f, 0);
				Main.dust[num].velocity *= 6f;
                Main.dust[num].scale = 1f;
				//Main.dust[num].noGravity = true;
			}
			for (int i = 0; i < 25; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 269, 0f, 0f, 0);
				Main.dust[num1].velocity *= 4f;
                Main.dust[num1].scale = 3f;
				Main.dust[num1].noGravity = true;
			}
			for (int i = 0; i < 10; i++)
			{
				ParticleManager.NewParticle<StarParticle2>(Projectile.Center, new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f)), new Color(255, 255, 10), Main.rand.NextFloat(0.06f, .17f), 0, 0);
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 600, false);
		}
    }
}