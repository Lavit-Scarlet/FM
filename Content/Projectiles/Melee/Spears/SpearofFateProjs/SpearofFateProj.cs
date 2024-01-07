using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Melee.Spears.SpearofFateProjs;
using Terraria.Audio;
using FM.Globals;

namespace FM.Content.Projectiles.Melee.Spears.SpearofFateProjs
{
	public class SpearofFateProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = 19;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.timeLeft = 300;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			float rotation = Projectile.velocity.ToRotation();
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + rotation.ToRotationVector2() * 15, Projectile.Center + rotation.ToRotationVector2() * -88, 24f * Projectile.scale, ref point))
				return true;
			return base.Colliding(projHitbox, targetHitbox);
		}
		public float movementFactor = 0;
		Vector2 originalVelo = Vector2.Zero;
		bool runOnce = true;
		float counter = 0;
		int nextProj = 1;
		public override bool PreAI()
		{
			if(runOnce)
            {
				originalVelo = Projectile.velocity;
				runOnce = false;
			}
			int useTime = (int)Projectile.ai[0];
			Player player = Main.player[Projectile.owner];
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2);
			Projectile.direction = player.direction;
			float degrees = 360 * (counter / useTime);
			counter++;

			Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 mousePosition = new Vector2(-40, 0).RotatedBy(originalVelo.ToRotation());
			mousePosition += new Vector2(32, 0).RotatedBy(MathHelper.ToRadians(degrees * Projectile.direction) + originalVelo.ToRotation());
			Vector2 toMouse = mousePosition;
			Projectile.velocity = new Vector2(-Projectile.velocity.Length(), 0).RotatedBy(toMouse.ToRotation());

			Projectile.direction = player.direction;
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 3;
			player.itemAnimation = 3;
			Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
			Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
			if (!player.frozen)
			{
				float progress = counter / useTime;
				float sin = (float)Math.Sin(progress * MathHelper.Pi);
				movementFactor = MathHelper.Lerp(10, 24, sin);
			}
			Projectile.position += Projectile.velocity * movementFactor;
			if (counter >= useTime)
			{
				Projectile.Kill();
			}
			Projectile.rotation = (Projectile.Center - player.Center).ToRotation() + MathHelper.ToRadians(135f);
			Projectile.spriteDirection = -Projectile.direction;
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= MathHelper.ToRadians(90f);
			}

			if(degrees > 60 * nextProj && nextProj <= 4)
            {
				nextProj++;
				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 perturbedSpeed = Projectile.velocity;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<SpearofFateShootProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, -2);
				}
			}
			return false;
		}
	}
}