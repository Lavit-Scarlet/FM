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
using FM.Content.Projectiles.Ranged.Bows.ArasBowProjs;

namespace FM.Content.Projectiles.Ranged.Bows.ArasBowProjs
{
	public class ArosArrowMinProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.extraUpdates = 2;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.Frostburn, 240, false);
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
			for (int i = 0; i < 30; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 68, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 2.2f;
				Main.dust[num].noGravity = true;
			}
			var player = Main.player[Projectile.owner];
		}
	}
}