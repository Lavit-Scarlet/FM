using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using FM.Globals;
using FM.Content.Projectiles.Melee.BadAppleProjs;

namespace FM.Content.Projectiles.Melee.BadAppleProjs
{
	public class BadAppleHeal : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}
		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}
		public override bool CanHitPvp(Player target)
		{
			return false;
		}
		public override bool CanHitPlayer(Player target)
		{
			return false;
		}
		public int randHeal;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 0, 0, 235, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0.4f;
				Main.dust[index2].noGravity = true;
			}
            float speed = Math.Max((Main.player[Projectile.owner].Center - Projectile.Center).Length() / 60f, 10f) + Projectile.ai[0] * 0.0003f;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center) * speed, Math.Max(1f - (Main.player[Projectile.owner].Center - Projectile.Center).Length() / 40f, 0.04f));
            if ((Projectile.Center - Main.player[Projectile.owner].Center).Length() < 20f)
            {
				randHeal = Main.rand.Next(1, 40);
		 	    player.statLife += randHeal;
		  	    player.HealEffect(randHeal);
                Projectile.Kill();
            }
		}
	}
}
