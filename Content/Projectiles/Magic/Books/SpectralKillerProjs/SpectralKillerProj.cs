using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Magic.Books.SpectralKillerProjs;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;
using FM.Effects;
using System.Collections.Generic;

namespace FM.Content.Projectiles.Magic.Books.SpectralKillerProjs
{
	public class SpectralKillerProj : ModProjectile
	{
        private int timer = 0;
        private Vector2 catchSpeed = new Vector2();
		public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;  
            Projectile.tileCollide = false;
			Projectile.timeLeft = 100;
            Projectile.scale = 1f;
		}
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
		public override bool PreDraw(ref Color lightColor)
		{
            Main.spriteBatch.End();
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/GlowTrail").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.05f);
            effect.Parameters["repeats"].SetValue(1f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);


			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
		}

        private readonly int NUMPOINTS = 40;
        public Color baseColor = new(100, 100, 255);
        public Color endColor = new(100, 100, 255);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 2.0f;

		public override void AI()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, baseColor, thickness);
            }
            if (timer == 0)
            {
                catchSpeed.X = Projectile.velocity.X;
                catchSpeed.Y = Projectile.velocity.Y;
                Projectile.velocity = new Vector2(0f,0f);
            }
            if (timer < 20)
            {
			    Projectile.rotation = (float)Math.Atan2((double)catchSpeed.Y, (double)catchSpeed.X) + 1.57f + (timer * (6.28f / 20f));
            }
            if (timer == 20)
            {
                Projectile.rotation = (float)Math.Atan2((double)catchSpeed.Y, (double)catchSpeed.X) + 1.57f;
                Projectile.velocity = catchSpeed;
            }
            for (int i = 0 ; i < Main.projectile.Length ; i++)
            {
                if (Main.projectile[i].hostile && Main.projectile[i].damage > 0 && Contains(Main.projectile[i].Center))
                    Main.projectile[i].Kill();
            }
            timer++;
		}
        public override void Kill(int timeLeft)
        {
            Vector2 target = new Vector2();
            for (int i = 0 ; i < Main.projectile.Length ; i++)
            {
                if (Main.projectile[i].type == ProjectileType<SpectralKillerPricelProj>())
                    target = Main.projectile[i].Center;
            }
            Vector2 speed = new Vector2(-28f,0f);
            speed = speed.RotatedBy((float)Math.Atan2((double)Projectile.Center.Y - target.Y, (double)Projectile.Center.X - target.X));
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, speed, ProjectileType<SpectralKillerReturnProj>(), Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
        }
        public bool Contains(Vector2 placement)
		{
			if (placement.X > Projectile.position.X && placement.X < Projectile.position.X + 30f && placement.Y > Projectile.position.Y && placement.Y < Projectile.position.Y + 30f)
				return true;
			return false;
		}
	}
}