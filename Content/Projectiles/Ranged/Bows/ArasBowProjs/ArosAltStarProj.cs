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

namespace FM.Content.Projectiles.Ranged.Bows.ArasBowProjs
{
	public class ArosAltStarProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Projectile.velocity *= 0.94f;
			Projectile.rotation += Projectile.velocity.Length() * 0.03f * Projectile.direction;
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(163, 196, 219), Projectile.rotation, drawOrigin, Projectile.scale / 5, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 240, false);
		}
		public override void Kill(int timeLeft)
		{
			FMDraw.SpawnRing(Projectile.Center, new Color(163, 196, 219), glowScale: 0f);
			for (int i = 0; i < 80; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 68, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 6.2f;
				Main.dust[num].noGravity = true;
			}
			for (int i = 0; i < 30; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 68, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num1].velocity *= 3.2f;
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 2;
			}
			int y = 6;
            int z = Main.rand.Next(360);
            for (int x = 0; x < y; x++)
            {
                Vector2 Vec = MathHelper.ToRadians((float)360 / y * x + z).ToRotationVector2();
                SoundEngine.PlaySound(SoundID.Item68, Projectile.position);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vec * 6f, ModContent.ProjectileType<ArosAltStarShootProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            Projectile.netUpdate = true;
		}
	}
}