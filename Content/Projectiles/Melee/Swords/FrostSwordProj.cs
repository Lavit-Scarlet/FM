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

namespace FM.Content.Projectiles.Melee.Swords
{
	public class FrostSwordProj : ModProjectile
	{
		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 3;
        }
		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;

			Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
			AIType = ProjectileID.ThrowingKnife;

			Projectile.frame = Main.rand.Next(3);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(BuffID.Frostburn, 60, false);
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
			for (int k = 0; k < 4; k++)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 80, Projectile.oldVelocity.X * -0.2f, Projectile.oldVelocity.Y * -0.2f);
			}
		}
	}
}
