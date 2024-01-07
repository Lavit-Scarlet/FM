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

namespace FM.Content.Projectiles.Ranged.Bows.HellstoneBowProjs
{
	public class HellstoneBowProj2 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 40;
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
		    int num = 5;
		    for (int k = 0; k < 2; k++)
		    {
		    	int index2 = Dust.NewDust(Projectile.position, 4, 4, 174, 0.0f, 0.0f, 0, new Color(), 1f);
		    	Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
		    	Main.dust[index2].scale = 1f;
		    	Main.dust[index2].velocity *= 1f;
		    	Main.dust[index2].noGravity = true;
		    	Main.dust[index2].noLight = false;
		    }
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 174);
				Main.dust[num].velocity *= 1.8f;
				Main.dust[num].scale = 1f;
				Main.dust[num].noGravity = true;
			}
		}
	}
}
