using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Magic.Books.HistoryBookRemakeProjs;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using FM.Globals;


namespace FM.Content.Projectiles.Magic.Books.HistoryBookRemakeProjs
{
	public class HistoryBookRemakeProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 960;
			ProjectileID.Sets.TrailCacheLength[Type] = 60;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			//Projectile.alpha = 50;
			Projectile.aiStyle = 9;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 12;
			Projectile.Opacity = 0.5f;
			Projectile.penetrate = 9;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.tileCollide = false;
		}
		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ProjectileType<HistoryBookBoomProj>(), Projectile.damage, 0, Projectile.owner, 0f, 0f);
			SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
			for (int d = 0; d < 14; d++)
			{
				Dust ExplosionDust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10), 0, Color.Red, 1f);
				ExplosionDust2.noGravity = true;
			}
			Vector2 DustTarget = Projectile.Center;
			for (int d = 0; d < Projectile.oldPos.Length; d++)
			{
				float LerpValue = Utils.GetLerpValue(Projectile.oldPos.Length, 0f, d, clamped: true);
				float DustLerp = MathHelper.Lerp(0.3f, 1f, LerpValue);
				int DustChance = Main.rand.Next(1, 4);
				if ((float)d >= (float)Projectile.oldPos.Length * 0.3f)
					DustChance--;
				if ((float)d >= (float)Projectile.oldPos.Length * 0.65f)
					DustChance -= 2;
				if ((float)d >= (float)Projectile.oldPos.Length * 0.85f)
					DustChance -= 3;
				for (int c = 0; c < DustChance; c++)
				{
					Dust TrailDust = Dust.NewDustDirect(Projectile.oldPos[d], Projectile.width * (int)LerpValue, Projectile.height * (int)LerpValue, DustID.RainbowMk2, 0f, 0f, 0, Color.Red, 1f);
					TrailDust.fadeIn = Main.rand.NextFloat() * 1.2f * DustLerp;
					TrailDust.noGravity = true;
					TrailDust.scale = 0.9f + Main.rand.NextFloat() * 1.2f * DustLerp;
					TrailDust.velocity *= Main.rand.NextFloat() *  0.8f;
					TrailDust.velocity += Projectile.oldPos[d].DirectionTo(DustTarget).SafeNormalize(Vector2.Zero) * 6f;
					DustTarget = Projectile.oldPos[d];
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.ai[0] == -1f)
			{
				Projectile.ai[1] = -1f;
				Projectile.netUpdate = true;
			}

			for (int d = 0; d < 12; d++)
			{
				Dust HitDust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.RainbowMk2, 0f, 0f, 0, Color.Red, 1f);
				HitDust.fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
				HitDust.noGravity = true;
				HitDust.scale = 0.6f + Main.rand.NextFloat() * 0.9f;
				HitDust.velocity = Main.rand.NextVector2Circular(1f, 1f);
				HitDust.velocity += Projectile.velocity * Main.rand.NextFloatDirection() * 0.5f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>("FM/Content/Projectiles/Magic/Books/HistoryBookRemakeProjs/HistoryBookRemakeProj").Value;
			Vector2 position = Projectile.Center + new Vector2(0f, Projectile.gfxOffY) - Main.screenPosition;
			
			Color color = new Color(255, 0, 0, 0);
			float LerpValue = Utils.GetLerpValue(0f, 8f, ((Vector2)(Projectile.velocity)).Length() * 2, clamped: true);
			float rotation = Projectile.rotation * LerpValue - (float)Math.PI / 2f * LerpValue;
			Vector2 origin = new Vector2(texture.Width, texture.Height) / 2f;
			Vector2 scale = Vector2.One * Projectile.scale * Utils.GetLerpValue(32f, 0f, Projectile.position.Distance(Projectile.oldPos[6]), clamped: true);
			scale.X *= MathHelper.Lerp(1f, 0.8f, LerpValue);
			SpriteEffects effects = SpriteEffects.None;

			Main.EntitySpriteDraw(texture, position, null, color, rotation, origin, scale, effects, 0);

			Vector2 scale2 = scale + scale * 0.25f * (float)Math.Cos(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f));
			Main.EntitySpriteDraw(texture, position, null, color, rotation, origin, scale2, effects, 0);
			default(HistoryBookRemakeProjDrawer).Draw(Projectile);

			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.2f, 0.4f));
		}
	}
}