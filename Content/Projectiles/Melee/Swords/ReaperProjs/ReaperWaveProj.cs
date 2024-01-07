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
using FM.Content.Projectiles.Melee.Swords.ReaperProjs;


namespace FM.Content.Projectiles.Melee.Swords.ReaperProjs
{
	public class ReaperWaveProj : ModProjectile
	{
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 240;
			Projectile.height = 240;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int RP = Main.rand.Next(0, 3);
			for (int p = 0; p < RP; ++p)
			{
				Vector2 targetDir = ((((float)Main.rand.Next(-180, 180)))).ToRotationVector2();
				targetDir.Normalize();
				targetDir *= 10;
				Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, targetDir, ProjectileType<ReaperSpiritProj>(), Projectile.damage, 0, Projectile.owner);
			}
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Projectile.alpha += 5;
            if (Projectile.alpha >= 235)
			{
				Projectile.Kill();
			}  
		}
	}
}
