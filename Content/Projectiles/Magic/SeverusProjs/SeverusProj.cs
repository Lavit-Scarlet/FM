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
using FM.Content.Projectiles;
using FM.Content.Projectiles.Magic.SeverusProjs;

namespace FM.Content.Projectiles.Magic.SeverusProjs
{
    public class SeverusProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.timeLeft = 60;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void AI()
        {
			Projectile.rotation += (float)Projectile.direction * 0.2f;
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 80, 0.0f, 0.0f, 0, new Color(), 1f);
			    Main.dust[dust].noGravity = true;
			    Main.dust[dust].scale = 0.8f;
            }
        }
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 8; k++)
            {
                int D = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 80, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f);
            }
			SoundEngine.PlaySound(SoundID.Item28, Projectile.position);
			int n = 8;
            int deviation = Main.rand.Next(-120, 120);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 8f;
                perturbedSpeed.Y *= 8f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile),Projectile.Center, perturbedSpeed, ProjectileType<SeverusTwoProj>(), Projectile.damage, 0, Projectile.owner);
            }
		}
    }
}