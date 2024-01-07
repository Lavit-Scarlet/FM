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
using ParticleLibrary;
using FM.Particles;
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Magic.GaussRailGunProjs
{
	public class GaussRailGunLaser : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 540;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 100;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<Electric>(), 120);
        }
		public override void AI()
		{
			if (Projectile.localAI[0]++ > 20f)
			{
                for (int i = 0; i < 1; i++)
                {
		    		Color color = Color.Lerp(Color.Blue, Color.White, 0.56f);
                    Vector2 v = Projectile.Center;
                    v -= Projectile.velocity * (i * 0.25f);
                    ParticleManager.NewParticle(v, Vector2.Zero, new BloomCircle_PerfectNotRandScale(), color, Main.rand.NextFloat(0.1f, 0.3f));
                }				
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item93, Projectile.position);
			Color color = Color.Lerp(Color.Blue, Color.White, 0.56f);
            int randParticles = Main.rand.Next(15, 30);
			for (int p = 0; p < 3; p++)
			{
				ParticleManager.NewParticle<HollowCircle_Small>(Projectile.Center, Vector2.Zero, color, 0.44f, 0, 0);
				ParticleManager.NewParticle<BloomCircle_PerfectNotRandScale>(Projectile.Center, Vector2.Zero, color, 1f, 0, 0);
			}
			for (int i = 0; i < randParticles; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f)), color, Main.rand.NextFloat(0.6f, 0.8f), 0, 0);
			}
		}
	}
}
