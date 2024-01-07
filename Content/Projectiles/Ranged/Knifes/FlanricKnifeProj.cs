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
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Ranged.Knifes
{
    public class FlanricKnifeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 113;
			AIType = ProjectileID.ThrowingKnife;
            //AIType = ProjectileID.Bullet;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            //Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
        }
        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			Projectile.spriteDirection = Projectile.direction;
		}
		public override void Kill(int timeLeft)
		{
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.position);
			for (int k = 0; k < 12; k++)
			{
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 219, Projectile.oldVelocity.X * -0.2f, Projectile.oldVelocity.Y * -0.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
			}
		}
    }
}