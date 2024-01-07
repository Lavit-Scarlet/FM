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
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Ranged.Bows.FlanProjs
{
    public class Flan175AltShootProj : ModProjectile
    {
        public bool stopped = false;

		public override void SetDefaults()
		{
			stopped = false;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 30;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(3))
			{
			  Player player = Main.player[Projectile.owner];
			  player.statLife += 1;
			  player.HealEffect(1);
			}
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			if (!stopped)
			{
				Projectile.velocity.X *= .93f;
				Projectile.velocity.Y *= .93f;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.position);
			for (int k = 0; k < 10; k++)
			{
				int D = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 219, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
				Main.dust[D].noGravity = true;
			}
		}
	}
}