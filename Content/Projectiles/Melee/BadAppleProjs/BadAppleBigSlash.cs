using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Globals;
using FM.Content.Projectiles.Melee.BadAppleProjs;
using System.IO;
using Terraria.DataStructures;
using System;
using static Terraria.ModLoader.ModContent;

namespace FM.Content.Projectiles.Melee.BadAppleProjs
{
	public class BadAppleBigSlash : SlashBadApple
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 136;
			Projectile.height = 140;
            Projectile.scale = 1f;
            AttackAI[0] = 14f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

		}
        public override Vector2 PosA(Vector2 Pos, Vector2 Vec)
        {
            Vector2 A = Pos + Vec * AttackAI[0] * Projectile.scale * 2.35f;
            return A;
        }
        public override Vector2 PosB(Vector2 Pos, Vector2 Vec)
        {
            Vector2 B = Pos + Vec * AttackAI[0] * Projectile.scale * 1f;
            return B;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (AttackAI[1] == 1f)
            {
                Color[] LColor = new Color[] { Color.Lerp(Color.Red, Color.Black, 0.5f) };
                AttackEffects(LColor);
                AttackEffects(Color.White);
            }
            return false;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int p = 0; p < 1; ++p)
			{
				Vector2 targetDir = ((((float)Main.rand.Next(-180, 180)))).ToRotationVector2();
				targetDir.Normalize();
				targetDir *= 10;
				Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, targetDir, ProjectileType<BadAppleHeal>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			SoundEngine.PlaySound(SoundID.Item103, Projectile.Center);
		}
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 1f)
            {
                if (Projectile.GetGlobalProjectile<FMGlobalProjectile>().ModAI[0] == 0f)
                {
                    AttackAI[0] = -5f;
                    Projectile.GetGlobalProjectile<FMGlobalProjectile>().ModAI[0] = 1f;
                    Projectile.netUpdate = true;
                }
                if (player.itemAnimation < player.itemAnimationMax * 0.75f)
                {
                    AttackAI[0] -= 24f / player.itemAnimationMax * 0.5f;
                }
                else
                {
                    AttackAI[0] += 24f / player.itemAnimationMax * 2f;
                }
            }
            if (AttackAI[1] == 0f)
            {
                DefVec = Projectile.velocity;
                AttackAI[1] = 1f;
                AttackAI[2] = Projectile.damage;
                Projectile.rotation = Projectile.velocity.ToRotation() + 0.795f;
                Projectile.rotation += MathHelper.Pi * 0.75f * Projectile.ai[1];
                Projectile.netUpdate = true;
            }
            player.heldProj = Projectile.whoAmI;
            player.itemTime = player.itemAnimation;
            Projectile.Center = player.Center;
            Projectile.spriteDirection = player.direction;
            Projectile.rotation += AttackAI[3];
            Projectile.velocity = (Projectile.rotation - 0.795f).ToRotationVector2() * 5f;
            Projectile.rotation = Projectile.velocity.ToRotation() + 0.795f;
            Projectile.position += Projectile.velocity * AttackAI[0] * Projectile.scale;
            float r = Projectile.velocity.ToRotation();
            if (player.direction == -1) r += MathHelper.Pi;
            player.itemRotation = r;
            float rot = MathHelper.TwoPi * 10.5f / player.itemAnimationMax * -Projectile.ai[1];
            if (player.itemAnimation < player.itemAnimationMax * 0.5f)
            {
                float use = player.itemAnimation / (float)player.itemAnimationMax;
                AttackAI[3] = rot * use;
            }
            else
            {
                if (Projectile.localAI[0] == 0f)
                {
                    Projectile.localAI[0] = 1f;
                    AttackPro();
                }
                float use = 1f - player.itemAnimation / (float)player.itemAnimationMax;
                AttackAI[3] = rot * use;
            }
            if (player.itemAnimation <= 1 || player.itemTime <= 1)
            {
                Projectile.Kill();
            }
            if (Projectile.localAI[1] == 0f)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Projectile.oldPos[i] = Projectile.position;
                    Projectile.oldRot[i] = Projectile.rotation;
                }
                Projectile.localAI[1] = 1f;
            }
        }
        public override void AttackPro()
        {
            SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
            base.AttackPro();
        }
	}
}
