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
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Magic.Books
{
	public class IceStormProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 400;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
			Projectile.aiStyle = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.alpha = 255;
		}
        public override void AI()
        {
			if (Main.rand.NextBool(80))
			{
				ParticleManager.NewParticle<Snowflake>(Projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), Color.White, Main.rand.NextFloat(0.12f, 0.22f), 0, 0);
				ParticleManager.NewParticle<BigFog>(Projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), Color.LightBlue, Main.rand.NextFloat(0.1f, 0.2f), 0, 0);
			}
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1f;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(BuffID.Frostburn, 180, false);
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}
