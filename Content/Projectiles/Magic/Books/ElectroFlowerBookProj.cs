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
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Magic.Books
{
    public class ElectroFlowerBookProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<Electric>(), 120);
        }
        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);

            Main.spriteBatch.End();
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Lightning").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.07f);
            effect.Parameters["repeats"].SetValue(3f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);


            Texture2D texture = ModContent.Request<Texture2D>("FM/Assets/Textures/SoftGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(93, 175, 234), Projectile.rotation, drawOrigin, Projectile.scale * 1f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

        private readonly int NUMPOINTS = 40;
        public Color baseColor = new(216, 246, 253);
        public Color endColor = new(93, 175, 234);
        public Color EdgeColor = new(34, 20, 93);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 8f;

        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * 0.3f;
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, EdgeColor, thickness);
            }
            Vector2 move = Vector2.Zero;
            float distance = 200f;
            bool targetted = false;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (!target.CanBeChasedBy())
                    continue;

                Vector2 newMove = target.Center - Projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                if (distanceTo < distance)
                {
                    move = target.Center;
                    distance = distanceTo;
                    targetted = true;
                }
            }
            if (targetted)
                Projectile.Move(move, Projectile.timeLeft > 50 ? 30 : 50, 50);
        }
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 25; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 226, 0f, 0f, 0);
				Main.dust[num].velocity *= 4f;
                Main.dust[num].scale = 1f;
				Main.dust[num].noGravity = false;
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity) 
        {
            for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 226, 0f, 0f, 0);
				Main.dust[num].velocity *= 3f;
                Main.dust[num].scale = 0.7f;
				Main.dust[num].noGravity = true;
			}
            SoundEngine.PlaySound(SoundID.Item93, Projectile.position);
			if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) 
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) 
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
        }
    }
}