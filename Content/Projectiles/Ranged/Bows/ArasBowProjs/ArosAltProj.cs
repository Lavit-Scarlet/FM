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
using FM.Content.Projectiles.Ranged.Bows.ArasBowProjs;
using FM.Effects.PrimitiveTrails;
using Terraria.Audio;
using Terraria.GameContent.Drawing;

namespace FM.Content.Projectiles.Ranged.Bows.ArasBowProjs
{
	public class ArosAltProj : ModProjectile, ITrailProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.extraUpdates = 2;
		}

		public void DoTrailCreation(TrailManager tManager)
        {
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(224, 249, 255), new Color(163, 196, 219)), new RoundCap(), new DefaultTrailPosition(), 6f, 400f, new DefaultShader());
        }

		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(224, 249, 255), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

		public override void AI()
		{
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 240, false);
		}
		public override void Kill(int timeLeft)
		{
			Vector2 velocity = new Vector2(0.1f * Math.Sign(Projectile.velocity.X), -7f);
			Vector2 position = new Vector2(Projectile.position.X, Projectile.position.Y + 7);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), position, velocity, ProjectileType<ArosAltStarProj>(), Projectile.damage, 0, Main.player[Projectile.owner].whoAmI);
			for (int i = 0; i < 30; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 68, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 4.2f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}