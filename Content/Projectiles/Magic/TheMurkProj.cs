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
using System.IO;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;
using FM.Effects;
using System.Collections.Generic;
using FM.Content.Projectiles.Magic.WitherStaffProjs;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Magic
{
    public class TheMurkProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 240;
        }
        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			Projectile.spriteDirection = Projectile.direction;

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 54, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1f;

			Projectile.netUpdate = true;
			NPC pre = null;
			if (FMHelper.ClosestNPC(ref pre, 140, Projectile.Center))
			{
			    float direction1 = Projectile.velocity.ToRotation();
			    direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(1));
			    Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
			}
		}
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 54, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}
		}
    }
}