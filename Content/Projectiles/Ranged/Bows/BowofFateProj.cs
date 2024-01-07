using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Effects.PrimitiveTrails;
using System.IO;
using Terraria.Audio;

namespace FM.Content.Projectiles.Ranged.Bows
{
	public class BowofFateProj : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 252, 148), new Color(61, 187, 177)), new RoundCap(), new DefaultTrailPosition(), 2f, 200f, new ImageShader(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_2", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 2f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 252, 148) * .20f, new Color(61, 187, 177) * .20f), new RoundCap(), new DefaultTrailPosition(), 6f, 300f, new DefaultShader());
        }
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 1;
			//AIType = ProjectileID.Bullet;
			Projectile.aiStyle = 1;
		}


		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int k = 0; k < 4; k++)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 163, Projectile.oldVelocity.X * -0.1f, Projectile.oldVelocity.Y * -0.1f);
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 155, Projectile.oldVelocity.X * -0.1f, Projectile.oldVelocity.Y * -0.1f);
			}
		}
	}
}
