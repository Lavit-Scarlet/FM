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
    public class ArosArrowEndProj : ModProjectile
    {
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.extraUpdates = 2;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 240, false);
		}
		int timer = 0;
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			timer++;
			if(timer == 10)
			{
				Projectile.penetrate = 1;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 80, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 4f;
				Main.dust[num].noGravity = true;
			}
		}

	}
}