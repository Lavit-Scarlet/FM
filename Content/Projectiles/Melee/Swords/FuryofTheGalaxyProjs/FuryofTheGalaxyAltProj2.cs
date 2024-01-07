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
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;
using FM.Content.Projectiles.Melee.Swords.FuryofTheGalaxyProjs;

namespace FM.Content.Projectiles.Melee.Swords.FuryofTheGalaxyProjs
{
	public class FuryofTheGalaxyAltProj2 : ModProjectile, ITrailProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
		}
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new RainbowTrail(5f, 0.002f, 1f, .75f), new RoundCap(), new DefaultTrailPosition(), 50f, 3000f, new ImageShader(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_4", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 1f));
            //tManager.CreateTrail(Projectile, new StandardColorTrail(Main.DiscoColor * .22f), new RoundCap(), new DefaultTrailPosition(), 40f, 800f, new DefaultShader());
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.velocity = Projectile.velocity.RotatedBy(System.Math.PI / -90);
        }
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Main.DiscoColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            //Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 50, 255, 150), Projectile.rotation, drawOrigin, Projectile.scale * 2f, SpriteEffects.None, 0);
            //Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 50, 255, 100), -Projectile.rotation, drawOrigin, Projectile.scale * 1.5f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            int n = 8;
            int deviation = Main.rand.Next(-60, 60);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 18f;
                perturbedSpeed.Y *= 18f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, perturbedSpeed, ProjectileType<FuryofTheGalaxyTwoProj>(), Projectile.damage, 0, Projectile.owner);
            }
        }
	}
}
