using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using FM.Content.Dusts;
using FM.Effects.PrimitiveTrails;
using FM.Globals;
using System;

namespace FM.Content.Projectiles.ReworkVanilla
{
    public class RubyStaffProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.velocity *= 1.02f;
            flareScale += Main.rand.NextFloat(-.02f, .02f);
            flareScale = MathHelper.Clamp(flareScale, .9f, 1.1f);
            flareOpacity += Main.rand.NextFloat(-.2f, .2f);
            flareOpacity = MathHelper.Clamp(flareOpacity, 0.6f, 1.1f);
        }
        public override bool PreAI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            float spread = .1f;
            Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-spread, spread));
            flareScale += Main.rand.NextFloat(-.03f, .03f);
            flareScale = MathHelper.Clamp(flareScale, .3f, .4f);
            flareOpacity += Main.rand.NextFloat(-.2f, .2f);
            flareOpacity = MathHelper.Clamp(flareOpacity, 0.6f, 1.1f);

			int num = 5;
			for (int k = 0; k < 5; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 8, 8, 90, 0.0f, 0.0f, 0, default(Color), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 0.8f;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;
			}
            return false;
        }
        public float flareScale;
        public float flareOpacity;
        public override void Kill(int timeLeft)
        {
			for (int i = 0; i < 14; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 90, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
        }
    }
}