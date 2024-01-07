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
using FM.Content.Items.Weapons.Melee.Swords;
using FM.Content.Projectiles.Melee.Swords.ExoBladeProjs;

namespace FM.Content.Projectiles.Melee.Swords.ExoBladeProjs
{
    public class ExoBladeSlash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 236;
            Projectile.height = 180;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
            Projectile.frameCounter = 0;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), Projectile.scale, spriteEffects, 0);
			return false;
		}
        int timeSound = 0;
        int timeCutting = 0;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            timeCutting++;
            if (timeCutting >= 3)
            {
                timeCutting = 0;
				FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Slashing"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .24f, .14f);
        	    Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<ExoBladeCuttingProj>(), Projectile.damage, 0, Projectile.owner, Main.rand.NextFloat(MathHelper.TwoPi), 1f);
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            timeSound++;
            if (timeSound == 15)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);
                timeSound = 0;
            }
            //Frames and crap
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
            }

            // Create idle light and dust.
            Vector2 origin = Projectile.Center + Projectile.velocity * 3f;
            Lighting.AddLight(origin, 0.2f, 0.2f, 1f);
            /*if (Main.rand.NextBool(3))
            {
                int redDust = Dust.NewDust(origin - Projectile.Size / 2f, Projectile.width, Projectile.height, 226, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1f);
                Main.dust[redDust].noGravity = true;
                Main.dust[redDust].position -= Projectile.velocity;
            }*/

            Vector2 playerRotatedPoint = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                if (player.channel && !player.noItems && !player.CCed)
                    HandleChannelMovement(player, playerRotatedPoint);
                else
                    Projectile.Kill();
            }

            // Rotation and directioning.
            float velocityAngle = Projectile.velocity.ToRotation();
            Projectile.rotation = velocityAngle + (Projectile.spriteDirection == -1).ToInt() * MathHelper.Pi;
            Projectile.direction = (Math.Cos(velocityAngle) > 0).ToDirectionInt();

            // Positioning close to the end of the player's arm.
            float offset = 30f * Projectile.scale;
            Projectile.position = playerRotatedPoint - Projectile.Size * 0.5f + velocityAngle.ToRotationVector2() * offset;

            // Sprite and player directioning.
            Projectile.spriteDirection = Projectile.direction;
            player.ChangeDir(Projectile.direction);

            // Prevents the projectile from dying
            Projectile.timeLeft = 2;

            // Player item-based field manipulation.
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }

        public void HandleChannelMovement(Player player, Vector2 playerRotatedPoint)
        {
            float speed = 1f;
            if (player.inventory[player.selectedItem].shoot == Projectile.type)
            {
                speed = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
            }
            Vector2 newVelocity = (Main.MouseWorld - playerRotatedPoint).SafeNormalize(Vector2.UnitX * player.direction) * speed;

            // Sync if a velocity component changes.
            if (Projectile.velocity.X != newVelocity.X || Projectile.velocity.Y != newVelocity.Y)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = newVelocity;
        }
        public override Color? GetAlpha(Color lightColor) => new Color(0, 0, 200, 0);
    }
}
