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
using System.IO;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;
using FM.Effects;
using System.Collections.Generic;

namespace FM.Content.Projectiles.Magic
{
	public class CriptaneFlowerProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Magic;
			//Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
            int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 8, 8, 5, 0.0f, 0.0f, 0, default(Color), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
			}
			Projectile.netUpdate = true;
			NPC pre = null;
			if (FMHelper.ClosestNPC(ref pre, 200, Projectile.Center, true))
			{
				float direction1 = Projectile.velocity.ToRotation();
				direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(1));
				Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
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
