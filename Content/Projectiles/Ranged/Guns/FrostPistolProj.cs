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

namespace FM.Content.Projectiles.Ranged.Guns
{
	public class FrostPistolProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 1;
			Projectile.aiStyle = 1;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.Frostburn, 120, false);
			}
		}
		public override void AI()
		{
			for (int d = 0; d < 1; d++)
			{
				int index2 = Dust.NewDust(Projectile.position, 10, 10, 67);
	    		Main.dust[index2].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
			for (int i = 0; i < 6; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 67);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].scale = 1f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}
