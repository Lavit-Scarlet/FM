using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using FM.Globals;

namespace FM.Content.Projectiles.Melee.Swords
{
	public class CarnageProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 30;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 0, 0, 5, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
			}
			Vector2 move = Vector2.Zero;
            float distance = 200f;
            bool targetted = false;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (!target.CanBeChasedBy())
                    continue;

                Vector2 newMove = target.Center - Projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                if (distanceTo < distance)
                {
                    move = target.Center;
                    distance = distanceTo;
                    targetted = true;
                }
            }
            if (targetted)
                Projectile.Move(move, Projectile.timeLeft > 50 ? 30 : 50, 50);
		}

		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 30; k++)
			{
				int index = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 5, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f);
				Main.dust[index].position = Projectile.Center;
			}
		}
	}
}
