using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Buffs.Debuff;

namespace FM.Content.NPCs.FlanricMobs
{
	public class ShootCrimeraFlanricProj : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.timeLeft = 240;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
		}
		public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255);
        }
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Deadly laser");
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
            Projectile.Kill();
        }
	}
}
