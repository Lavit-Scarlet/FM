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
using FM.Content.Projectiles.Ranged.Bows.FlanProjs;
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Ranged.Bows.FlanProjs
{
    public class Flan175AltProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
        }
        public override bool PreAI()
		{
            Projectile.velocity = Projectile.velocity.RotatedBy(System.Math.PI / 40);
			int num = 1;
			for (int k = 0; k < 1; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 0, 0, 219);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
			}
            return true;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item68, Projectile.position);
            int n = 8;
            int deviation = Main.rand.Next(0, 360);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 5.5f;
                perturbedSpeed.Y *= 5.5f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, perturbedSpeed, ProjectileType<Flan175AltShootProj>(), Projectile.damage, 1, Projectile.owner);
            }
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
        }
    }
}