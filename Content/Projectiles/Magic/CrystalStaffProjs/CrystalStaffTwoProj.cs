using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using FM.Content.Projectiles.Magic;
using FM.Globals;

namespace FM.Content.Projectiles.Magic.CrystalStaffProjs
{
	public class CrystalStaffTwoProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 80;
			Projectile.DamageType = DamageClass.Magic;
			//Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 1; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 4, 4, 254, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 0.8f;
				Main.dust[index2].velocity *= 0.4f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
			for (int k = 0; k < 7; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 4, 4, 254, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 0.3f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
			
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 30f)
            {
				Projectile.penetrate = 1;
				Projectile.extraUpdates = 1;
				Projectile.netUpdate = true;
				NPC pre = null;
				if (FMHelper.ClosestNPC(ref pre, 300, Projectile.Center, true))
				{
					Projectile.timeLeft = 120;
					float direction1 = Projectile.velocity.ToRotation();
					direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(3));
					Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
				}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity) 
        {
			if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) 
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) 
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
        }

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			for (int i = 0; i < 12; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 254, 0f, 0f, 0, default(Color), 1.6f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
				Main.dust[num].scale = 0.8f;
			}
		}
	}
}
