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
using FM.Content.Projectiles.Melee;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;


namespace FM.Content.Projectiles.Melee
{
	public class PrimordialThrowingAxProj : ModProjectile
	{
		public override void SetDefaults()
		{
            Projectile.tileCollide = true;
			Projectile.width = 62;
			Projectile.height = 62;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Melee; 
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
        private bool returnAxe = false;
		private bool Axe = true;
        public override void AI()
        {
			if (Axe)
			{
				Projectile.rotation += 0.188f * Projectile.direction;
				Projectile.spriteDirection = Projectile.direction;
				Projectile.velocity.Y += 0.15f;
			}
			if (returnAxe)
			{
				Projectile.friendly = false;
				Projectile.extraUpdates = 4;
				Projectile.alpha = 255;
				int num = 5;
				for (int k = 0; k < 2; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 20, 20, 222, 0.0f, 0.0f, 0, default(Color), 1f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 1f;
					Main.dust[index2].velocity *= 0.4f;
					Main.dust[index2].noGravity = true;
				}
                float speed = Math.Max((Main.player[Projectile.owner].Center - Projectile.Center).Length() / 60f, 10f) + Projectile.ai[0] * 0.0003f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center) * speed, Math.Max(1f - (Main.player[Projectile.owner].Center - Projectile.Center).Length() / 40f, 0.04f));
                if ((Projectile.Center - Main.player[Projectile.owner].Center).Length() < 20f)
                {
                    Projectile.Kill();
                }
			}
			int size = 62;
            Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 12; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 222, 0f, 0f, 0, default(Color), 1.2f);
				Main.dust[num].velocity *= 4f;
				Main.dust[num].noGravity = true;
			}
            returnAxe = true;
			Axe = false;
            Projectile.tileCollide = false;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			for (int i = 0; i < 12; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 222, 0f, 0f, 0, default(Color), 1.2f);
				Main.dust[num].velocity *= 4f;
				Main.dust[num].noGravity = true;

			}
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            returnAxe = true;
			Axe = false;
            Projectile.tileCollide = false;
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 20;
            height = 20;
            return true;
        }
	}
}
