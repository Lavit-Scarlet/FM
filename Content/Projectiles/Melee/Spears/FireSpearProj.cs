using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace FM.Content.Projectiles.Melee.Spears
{
	public class FireSpearProj : ModProjectile
	{
		protected virtual float HoldoutRangeMin => 80f;
		protected virtual float HoldoutRangeMax => 135f;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255);
        }

        public override void SetDefaults() 
		{
			Projectile.CloneDefaults(ProjectileID.Spear);
			Projectile.scale = 1;
		}

        public override bool PreAI() 
		{
			Player player = Main.player[Projectile.owner]; 
			int duration = player.itemAnimationMax; 

			player.heldProj = Projectile.whoAmI; 
			
			if (Projectile.timeLeft > duration) 
			{
				Projectile.timeLeft = duration;
			}

			Projectile.velocity = Vector2.Normalize(Projectile.velocity);

			float halfDuration = duration * 0.5f;
			float progress;


			if (Projectile.timeLeft < halfDuration) 
			{
				progress = Projectile.timeLeft / halfDuration;
			}
			else 
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

			if (Projectile.spriteDirection == -1) 
			{
				Projectile.rotation += MathHelper.ToRadians(45f);
			}
			else 
			{
				Projectile.rotation += MathHelper.ToRadians(135f);
			}
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
            Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1.2f;

			int dust1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
            Main.dust[dust1].noGravity = false;
			Main.dust[dust1].scale = .8f;
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
			}
        }
	}
}
