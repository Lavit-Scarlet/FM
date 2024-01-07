using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace FM.Content.Projectiles.Magic.Books
{
	public class RageofFireProj : ModProjectile
	{
		public override void SetDefaults()
		{
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 42;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.8f, 0.1f));
		}
		public void FrameUpdate()
        {
            Main.projFrames[Projectile.type] = 7;
            Projectile.frameCounter++;
            if (Projectile.frameCounter == 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame == 7)
                    Projectile.frame = 0;
            }
        }
		public override void AI()
		{
			Projectile.rotation += (float)Projectile.direction * 0.4f;
			FrameUpdate();
			for (int i = 0; i < 1; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
                Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;

				int dust1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
                Main.dust[dust1].noGravity = false;
				Main.dust[dust1].scale = 0.7f;

				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
                Main.dust[dust2].noGravity = true;
				Main.dust[dust2].scale = 1.5f;
			}
			int size = 98;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
			if (collding)
			{
				Projectile.velocity *= 0.5f;
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, -oldVelocity.X, 0.75f);
			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, -oldVelocity.Y, 0.75f);
			return false;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(BuffID.OnFire, 120, false);
			}
		}
	}
}
