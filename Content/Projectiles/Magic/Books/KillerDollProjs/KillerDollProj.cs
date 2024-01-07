using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using FM.Content.Projectiles.Magic.Books.KillerDollProjs;

namespace FM.Content.Projectiles.Magic.Books.KillerDollProjs
{
	public class KillerDollProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 30;
			Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
		{
			//Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation() + 1.57f;
			Projectile.ai[1] += 1f;
            if (Projectile.ai[1] < 4f)
            {
                // Fade out
                Projectile.alpha -= 50;
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha = 255;
                }
            }
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item71, Projectile.position);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<KillerDollLaserProj>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Projectile.whoAmI);
		}
	}
}
