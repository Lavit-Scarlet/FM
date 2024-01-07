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
using FM.Content.Projectiles.Melee.BadAppleProjs;


namespace FM.Content.Projectiles.Melee.BadAppleProjs
{
	public class BadAppleProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 76;
			Projectile.height = 76;
            Projectile.scale = 1f;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255);
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("FM/Content/Projectiles/Melee/BadAppleProjs/BadAppleProj_Circle", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            Color c2 = Color.Lerp(Color.Red, Color.Black, 0.5f);
            for (int k = 0; k < 3; k++)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, c2, Projectile.rotation, drawOrigin, 0.52f, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return true;
        }
		public override void AI()
		{
            if ((int)Projectile.ai[0] == 0)
            {
                Projectile.direction = Projectile.velocity.X < 0f ? -1 : 1;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 60f)
            {
                Projectile.tileCollide = false;
            }
            if (Projectile.ai[0] > 46f)
            {
                float speed = Math.Max((Main.player[Projectile.owner].Center - Projectile.Center).Length() / 60f, 10f) + Projectile.ai[0] * 0.0003f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center) * speed, Math.Max(1f - (Main.player[Projectile.owner].Center - Projectile.Center).Length() / 40f, 0.04f));
                if ((Projectile.Center - Main.player[Projectile.owner].Center).Length() < 20f)
                {
                    Projectile.Kill();
                }
            }
            Projectile.rotation += 0.2f * Projectile.direction;
			Projectile.spriteDirection = Projectile.direction;

			int size = 76;
            Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 4;
            height = 4;
            fallThrough = true;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] += 5f;
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Collision.HitTiles(Projectile.position + new Vector2(Projectile.width / 4f, Projectile.height / 4f), oldVelocity, Projectile.width / 2, Projectile.height / 2);
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
	}
}
