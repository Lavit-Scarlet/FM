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

namespace FM.Content.Projectiles.Melee.Swords.FuryofTheGalaxyProjs
{
	public class FuryofTheGalaxyTwoProj : ModProjectile, ITrailProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
		}
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new RainbowTrail(5f, 0.002f, 1f, .75f), new RoundCap(), new DefaultTrailPosition(), 50f, 300f, new ImageShader(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_4", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 1f));
            //tManager.CreateTrail(Projectile, new StandardColorTrail(Main.DiscoColor * .22f), new RoundCap(), new DefaultTrailPosition(), 30f, 500f, new DefaultShader());
        }
		public override void AI()
        {
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

			Projectile.netUpdate = true;
			NPC pre = null;
			if (FMHelper.ClosestNPC(ref pre, 400, Projectile.Center, true))
			{
				float direction1 = Projectile.velocity.ToRotation();
				direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(4));
				Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
			
			}
        }
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Main.DiscoColor, Projectile.rotation, drawOrigin, Projectile.scale * 1f, SpriteEffects.None, 0);
           // Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 50, 255, 100), -Projectile.rotation, drawOrigin, Projectile.scale * .8f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
	}
}
