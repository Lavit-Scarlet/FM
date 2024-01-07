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
using static Terraria.ModLoader.ModContent;
using FM.Effects;
using System.Collections.Generic;
using ParticleLibrary;
using FM.Particles;
using FM.Content.Projectiles.Other.BlyatkaGunProjs;

namespace FM.Content.Projectiles.Other.BlyatkaGunProjs
{
	public class BlyatkaGunProj : ModProjectile
	{
		public override void SetDefaults() 
		{
			Projectile.width = 28;              
			Projectile.height = 28;              
			Projectile.aiStyle = 1;             
			Projectile.friendly = true;         
			Projectile.hostile = false;         
			Projectile.DamageType = DamageClass.Ranged;           
			Projectile.penetrate = 8;           
			Projectile.timeLeft = 600;                      
			Projectile.light = 2.25f;            
			Projectile.ignoreWater = true;          
			Projectile.tileCollide = true;          
			Projectile.extraUpdates = 1;            
			AIType = ProjectileID.Bullet;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}
		public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			Projectile.spriteDirection = Projectile.direction;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				ParticleManager.NewParticle<EmberParticle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f)), Color.White, Main.rand.NextFloat(0.1f, 2f), 0, 0);
			}
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 25; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0);
				Main.dust[num].velocity *= 6f;
                Main.dust[num].scale = 2f;
				Main.dust[num].noGravity = true;
			}
            int n = 8;
            int deviation = Main.rand.Next(0, 120);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(10, 10).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 6f;
                perturbedSpeed.Y *= 6f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile),Projectile.Center, perturbedSpeed, ProjectileType<BlyatkaGun2Proj>(), Projectile.damage, 0, Projectile.owner);
            }
		}
	}
}
