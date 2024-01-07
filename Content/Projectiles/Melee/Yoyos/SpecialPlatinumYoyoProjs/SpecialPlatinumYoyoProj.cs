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
using FM.Content.Projectiles.Melee.Yoyos.SpecialPlatinumYoyoProjs;
using FM.Globals;

namespace FM.Content.Projectiles.Melee.Yoyos.SpecialPlatinumYoyoProjs
{
	public class SpecialPlatinumYoyoProj : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			// Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 18f;
			// Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 380f;
			// Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 24f;
		}

		public override void SetDefaults() 
		{
            Projectile.extraUpdates = 0;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
		}
		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 12) 
			{
				Projectile.frameCounter = 0;
				float num = 8000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++) 
				{
					float num3 = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 640f && Main.npc[i].CanBeChasedBy(Projectile, false)) 
					{
						num2 = i;
						num = num3;
					}
				}
				if (num2 != -1) 
				{
					bool flag = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
					if (flag) 
					{
						Vector2 value = Main.npc[num2].Center - Projectile.Center;
						float num4 = 0.01f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4) 
						{
							num5 = num4 / num5;
						}
						value *= num5;
						FMHelper.PlaySound(SoundID.Item12, (int)Projectile.Center.X, (int)Projectile.Center.Y);
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, value.X, value.Y, ModContent.ProjectileType<SpecialPlatinumYoyoLaserProj>(), Projectile.damage, Projectile.knockBack / 2f, Projectile.owner, 0f, 0f);
					}
				}
			}
		}
	}
}
