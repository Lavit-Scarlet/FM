using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Content.Projectiles.Melee.Spears.BloodSpearProjs;
using Terraria.Audio;
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Melee.Spears.BloodSpearProjs
{
	public class BloodSpearProj : ModProjectile
	{
		protected virtual float HoldoutRangeMin => 80f;
		protected virtual float HoldoutRangeMax => 135f;

		public override void SetDefaults() 
		{
			Projectile.CloneDefaults(ProjectileID.Spear);
			Projectile.scale = 1;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
        }
		public override bool PreAI() 
		{
			Player player = Main.player[Projectile.owner]; 
			int duration = player.itemAnimationMax; 

			player.heldProj = Projectile.whoAmI; 
			
			if (Projectile.timeLeft > duration) 
			{
				Projectile.timeLeft = duration;
				Vector2 Vec = Projectile.velocity * 1f;
				Projectile.netUpdate = true;
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + Vec, Vec, ModContent.ProjectileType<BloodSpearProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
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
			return false;
		}

	}
}
