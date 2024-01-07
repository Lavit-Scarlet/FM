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
using FM.Content.Projectiles.Magic.FlanricRoseProjs;
using FM.Effects;
using System.Collections.Generic;
using FM.Content.Buffs.Debuff;
using ParticleLibrary;
using FM.Particles;


namespace FM.Content.Projectiles.Magic.FlanricRoseProjs
{
    public class FlanricRoseProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
        }
        private readonly int NUMPOINTS = 30;
        public Color baseColor = new(255, 100, 100);
        public Color EdgeColor = new(50, 0, 0);
        public Color endColor = new(255, 0, 0);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 6f;
        public override void AI()
        {
            Projectile.velocity *= 0.96f;
			Projectile.rotation += Projectile.velocity.Length() * 0.03f * Projectile.direction;
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 219, 0.0f, 0.0f, 0, new Color(), 1f);
			    Main.dust[dust].noGravity = true;
			    Main.dust[dust].scale = 0.8f;
            }
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, EdgeColor, thickness);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
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

            return true;
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 16; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 219);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}
			for (int i = 0; i < 1; i++)
			{
				Color color = Color.Lerp(Color.Red, Color.Pink, 0.4f);
				ParticleManager.NewParticle<FlowerParticle2>(Projectile.Center, Projectile.velocity, color, 0.1f, 0, 0);
			}
			SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.position);
			int n = 8;
            int deviation = Main.rand.Next(-120, 120);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 10f;
                perturbedSpeed.Y *= 10f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile),Projectile.Center, perturbedSpeed, ProjectileType<FlanricRoseProj2>(), Projectile.damage, 0, Projectile.owner);
            }
		}
    }
}