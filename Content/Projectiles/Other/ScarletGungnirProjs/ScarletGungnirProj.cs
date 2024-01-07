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
using FM.Content.Projectiles.Other.ScarletGungnirProjs;
using FM.Effects;
using System.Collections.Generic;
using FM.Content.Buffs.Debuff;
using FM.Content.Projectiles.Magic.Books.HistoryBookRemakeProjs;

namespace FM.Content.Projectiles.Other.ScarletGungnirProjs
{
	public class ScarletGungnirProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 4;
			AIType = ProjectileID.Bullet;
			Projectile.aiStyle = 1;
			Projectile.GetGlobalProjectile<FMGlobalProjectile>().ignoresArmor = true;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);

            return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
			Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ProjectileType<ScarletGungnirBoomProj>(), Projectile.damage, 0, Projectile.owner, 0f, 0f);
			
			int n = 16;
            int deviation = Main.rand.Next(0, 360);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 10f;
                perturbedSpeed.Y *= 10f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile),Projectile.Center, perturbedSpeed, ProjectileType<ScarletGungnir2Proj>(), Projectile.damage, 0, Projectile.owner);
            }
            target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
		}

		/*private readonly int NUMPOINTS = 80;
        public Color baseColor = new(255, 0, 0);
        public Color endColor = new(100, 0, 0);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 9f;*/

		public override void AI()
		{
			for (int k = 0; k < 1; k++)
			{
				int index = Dust.NewDust(Projectile.position + Projectile.velocity, 30, 30, 219, Projectile.oldVelocity.X * 0.4f, Projectile.oldVelocity.Y * 0.4f);
				//Main.dust[index].position = Projectile.Center;
				Main.dust[index].scale = 0.8f;
				Main.dust[index].noGravity = true;
			}
			/*if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, baseColor, thickness);
            }*/
		}

	}
}
