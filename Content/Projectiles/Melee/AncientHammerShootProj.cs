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
using FM.Content.Projectiles.Melee;
using Terraria.GameContent.Drawing;

namespace FM.Content.Projectiles.Melee
{
	public class AncientHammerShootProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 68;
			Projectile.height = 68;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Color.White * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
		public override void AI()
		{
			Projectile.velocity *= 0.99f;
			Projectile.rotation += Projectile.velocity.Length() * 0.03f * Projectile.direction;
			int size = 48;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
			if (collding)
			{
				Projectile.alpha += 10;
				Projectile.velocity *= 0.8f;
			}
            Projectile.alpha += 3;
            if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}  
			for (int i = 0; i < 1; i++)
			{
				int dust = Dust.NewDust(Projectile.position, 68, 68, 269);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Projectile.velocity * 0.5f;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(target.Center, target.width, target.height, 269, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 6.2f;
				Main.dust[num].scale = 1;
				Main.dust[num].noGravity = true;
			}
			for (int p = 0; p < 1; p++)
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) }, Projectile.owner);
				Dust.NewDust(Main.rand.NextVector2FromRectangle(target.Hitbox), 0, 0, 6);
			}
        }
	}
}
