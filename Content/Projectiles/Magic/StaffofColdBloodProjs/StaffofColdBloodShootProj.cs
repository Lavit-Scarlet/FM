using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Effects.PrimitiveTrails;
using System.IO;
using Terraria.Audio;

namespace FM.Content.Projectiles.Magic.StaffofColdBloodProjs
{
	public class StaffofColdBloodShootProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Magic;
			//Projectile.extraUpdates = 1;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.Frostburn, 180, false);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
		}
		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position, 18, 18, 135, 0.0f, 0.0f, 0, default(Color), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1.6f;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			//SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
			for (int num623 = 0; num623 < 10; num623++)
			{
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 0, default(Color), 2f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 1f;
			}
		}
	}
}
