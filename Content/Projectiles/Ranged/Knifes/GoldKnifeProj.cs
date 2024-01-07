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

namespace FM.Content.Projectiles.Ranged.Knifes
{
    public class GoldKnifeProj : ModProjectile
    {
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
			AIType = ProjectileID.ThrowingKnife;
            Projectile.width = 10;
            Projectile.height = 10;
        }
		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int k = 0; k < 3; k++)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 10, Projectile.oldVelocity.X * -0.1f, Projectile.oldVelocity.Y * -0.1f);
			}
		}
    }
}