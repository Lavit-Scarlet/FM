using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Content.Projectiles.Melee.Spears.HeavenlySpearProjs;
using FM.Globals;
using Terraria.Audio;

namespace FM.Content.Projectiles.Melee.Spears.HeavenlySpearProjs
{
	public class HeavenlySpearProj : ModProjectile
	{
		// Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 80f;
		protected virtual float HoldoutRangeMax => 150f;
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
				FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/HeavenlyBlast"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f);
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
