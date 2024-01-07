using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Globals;
using Terraria.Audio;
using Terraria.GameContent;
using FM.Content.Projectiles.Magic.PrimordialStaffProjs;
using System.IO;
using Terraria.Enums;
using FM.Content.Items.Weapons.Magic;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Magic.PrimordialStaffProjs
{
    public class PrimordialStaffProj : ModProjectile
    {
        private const float AimResponsiveness = 0.66f;

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 900;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            UpdatePlayerVisuals(player, rrp);

            if (Projectile.localAI[0] == 0f)
                Projectile.localAI[0] = 47f / 60;

            Projectile.ai[0] += Projectile.localAI[0];
            int maxTime = PrimordialStaff.ChargeFrames + PrimordialStaff.CooldownFrames;
            if (Projectile.ai[0] > maxTime)
            {
                Projectile.Kill();
                return;
            }

            float chargeLevel = MathHelper.Clamp(Projectile.ai[0] / PrimordialStaff.ChargeFrames, 0f, 1f);

            float angle = Projectile.rotation - MathHelper.PiOver2;
            Vector2 gemOffset = Vector2.One * PrimordialStaff.GemDistance * 1.4142f;
            Vector2 gemPos = Projectile.Center + gemOffset.RotatedBy(angle);

            if (chargeLevel >= 1f && Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 1f; 
                FiringEffects(gemPos);
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile laser = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), gemPos, Projectile.velocity, ModContent.ProjectileType<PrimordialStaffLaser>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    laser.Center = gemPos;
                }
            }
            else if (Projectile.ai[1] == 0f)
            {
                UpdateAim(rrp, Projectile.velocity.Length());
                ChargingEffects(gemPos, chargeLevel);
            }
        }

        private void UpdatePlayerVisuals(Player player, Vector2 rrp)
        {
            // Place the projectile directly into the player's hand at all times
            Projectile.Center = rrp;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            // The staff is a holdout projectile, so change the player's variables to reflect that
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // Multiplying by projectile.direction is required due to vanilla spaghetti.
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.velocity.ToRotation() - MathHelper.PiOver2);
        }

        // Adjusts the aim vector of the staff to point towards the mouse. This is Last Prism code.
        private void UpdateAim(Vector2 source, float speed)
        {
            Vector2 aimVector = Vector2.Normalize(Main.MouseWorld - source);
            if (aimVector.HasNaNs())
                aimVector = -Vector2.UnitY;
            aimVector = Vector2.Normalize(Vector2.Lerp(aimVector, Vector2.Normalize(Projectile.velocity), AimResponsiveness));
            aimVector *= 30f;

            if (aimVector != Projectile.velocity)
                Projectile.netUpdate = true;
            Projectile.velocity = aimVector;
        }

        private void ChargingEffects(Vector2 center, float chargeLevel)
        {
            Color color = Color.Lerp(Color.Yellow, Color.White, 0.66f);
            float incomingRadius = 9f;
            Vector2 offsetUnit = Main.rand.NextVector2Unit();
            Vector2 dustPos = center + offsetUnit * incomingRadius;
			float num627 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float num628 = Main.rand.NextFloat();
			Vector2 position7 = dustPos + new Vector2(0, -0).RotatedBy(Projectile.rotation) + num627.ToRotationVector2() * (50f + 50f * num628);
			Vector2 vector154 = (num627 + (float)Math.PI).ToRotationVector2() * (8f + 8f * Main.rand.NextFloat() + 8f * num628);
			ParticleManager.NewParticle<BloomCircle_FadingIn>(position7, vector154 * 0.2f, color, Main.rand.NextFloat(0.2f, 0.4f), 0, 0);
        }

        private void FiringEffects(Vector2 center)
        {
            Color color = Color.Lerp(Color.Yellow, Color.White, 0.66f);
			for (int i = 0; i < 30; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(center, new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f)), color, Main.rand.NextFloat(0.4f, 0.8f), 0, 0);
			}
        }
    }
}
