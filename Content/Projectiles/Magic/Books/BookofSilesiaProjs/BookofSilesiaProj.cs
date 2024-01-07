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
using System.IO;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;
using FM.Effects;
using System.Collections.Generic;
using FM.Content.Projectiles.Magic.WitherStaffProjs;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Magic.Books.BookofSilesiaProjs
{
    public class BookofSilesiaProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Rand = Main.rand.Next(50, 100);
            double angle = Main.rand.NextDouble() * 2d * Math.PI;
            MoveVector2.X = (float)(Math.Sin(angle) * Rand);
            MoveVector2.Y = (float)(Math.Cos(angle) * Rand);
        }
        public override bool? CanCutTiles() => Projectile.ai[0] != 0;
        public override bool? CanHitNPC(NPC target) => !target.friendly && Projectile.ai[0] != 0 ? null : false;
        public Vector2 MoveVector2;
        public Vector2 pos = new(0, -5);
        public ref float Rand => ref Projectile.localAI[0];
        private bool shoot;

		private readonly int NUMPOINTS = 15;
        public Color baseColor = new(244, 244, 244);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 3.4f;
        public float rot;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
			for (int p = 0; p < 1; p++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), Color.White, Main.rand.NextFloat(0.01f, 0.4f), 0, 0);
			}
            if (Projectile.alpha > 0)
                Projectile.alpha -= 5;
            if (Projectile.ai[1]++ < 60)
                pos *= 0.96f;
            else
            {
                if (Projectile.localAI[1] == 0)
                {
                    pos.Y += 0.06f;
                    if (pos.Y > .7f)
                        Projectile.localAI[1] = 1;
                }
                else if (Projectile.localAI[1] == 1)
                {
                    pos.Y -= 0.06f;
                    if (pos.Y < -.7f)
                        Projectile.localAI[1] = 0;
                }
            }
            if (Projectile.owner == Main.myPlayer)
            {
                if (Projectile.ai[0] == 0)
                {
                    Projectile.timeLeft = 180;
                    Projectile.position = player.Center + MoveVector2;
                    MoveVector2 += pos;
                    
                    Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation() + 1.57f;
                    rot = Projectile.rotation;

                    if (Projectile.localAI[2] == 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            ParticleManager.NewParticle<HollowCircle_Small>(Projectile.Center, Vector2.Zero, Color.White, new Vector2(0.32f, 0.12f), 0, 0);
                        }
                        Projectile.localAI[2] = 1;
                    }
                    if (shoot && Main.rand.NextBool(10) && Projectile.alpha <= 0)
                    {
                        FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/HeavenlyBlast"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .80f, 0.8f);
                        Projectile.rotation = rot;
                        //Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
                        Projectile.tileCollide = true;
                        Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 30;
                        Projectile.ai[0] = 1;
                    }
                }
            }
            if (Main.netMode != NetmodeID.Server)
            {
            	TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
            	TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, baseColor, baseColor, thickness);
            }
            if (!player.channel)
            {
                shoot = true;
            }
        }
		public override void Kill(int timeLeft)
		{
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/StarFlower"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.66f, 0.92f);
            float rotParticle = MathHelper.ToRadians(-40);
			float numberParticles = Main.rand.Next(10, 20);
			for (int i = 0; i < numberParticles; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.2f), Color.White, Main.rand.NextFloat(0.3f, 0.8f), 0, 0);
			}
			for (int p = 0; p < 30; p++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), Color.White, Main.rand.NextFloat(0.01f, 0.4f), 0, 0);
			}
		}
        public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.9f, 0.9f, 0.9f));
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(244, 244, 244), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);



            Main.spriteBatch.End();//Trail
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_1").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.05f);
            effect.Parameters["repeats"].SetValue(1f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }
    }
}