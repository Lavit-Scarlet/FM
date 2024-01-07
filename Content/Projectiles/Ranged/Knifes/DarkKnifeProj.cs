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
    public class DarkKnifeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
			AIType = ProjectileID.ThrowingKnife;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
        }
		public override void AI()
		{
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int k = 0; k < 6; k++)
			{
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 54, Projectile.oldVelocity.X * -0.2f, Projectile.oldVelocity.Y * -0.2f);
				Main.dust[dust].noGravity = true;
    			Main.dust[dust].scale = 1f;
			}
		}
    }
}