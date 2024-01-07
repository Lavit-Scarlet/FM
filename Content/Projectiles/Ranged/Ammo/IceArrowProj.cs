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
using FM.Content.Projectiles.Ranged.Ammo;

namespace FM.Content.Projectiles.Ranged.Ammo
{
    public class IceArrowProj : ModProjectile
    {
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Ranged;
			//Projectile.extraUpdates = 1;
			Projectile.aiStyle = 1;
		}

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 80, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f);
            }

            if (Main.rand.Next(4) == 0)
            {
                int n = 2;
                int deviation = Main.rand.Next(0, 60);
                for (int i = 0; i < n; i++)
                {
                    float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= -3f;
                    perturbedSpeed.Y *= -3f;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile),Projectile.Center, perturbedSpeed, ProjectileType<IceArrowFireProj>(), Projectile.damage, 1, Projectile.owner);
                }
            }
        }
    }
}