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
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using FM.Content.Projectiles.Magic.StaffofPrismProjs;
using static Terraria.ModLoader.ModContent;

namespace FM.Content.Projectiles.Magic.StaffofPrismProjs
{
	public class StaffofPrismProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 72;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			float num = 1.57079637f;
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
			if (Projectile.type == ModContent.ProjectileType<StaffofPrismProj>())
			{
				Projectile.ai[0] += 1f;
				int num2 = 0;
				int num3 = 14;
				if (Projectile.ai[0] >= 180f)
				{
					num2++;
					num3 = 12;
				}
				if (Projectile.ai[0] >= 360f)
				{
					num2++;
					num3 = 10;
				}
				if (Projectile.ai[0] >= 520f)
				{
					num2++;
					num3 = 8;
				}
				int num4 = 1;
				Projectile.ai[1] += 1f;
				bool flag = false;
				if (Projectile.ai[1] >= num3 - num4 * num2)
				{
					Projectile.ai[1] = 0f;
					flag = true;
				}
				Projectile.frameCounter += 1 + num2;
				if (Projectile.frameCounter >= 4)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;
					if (Projectile.frame >= 4)
					{
						Projectile.frame = 0;
					}
				}
				if (Projectile.soundDelay <= 0)
				{
					Projectile.soundDelay = num3 - num4 * num2;
					if (Projectile.ai[0] != 1f)
					{
						SoundEngine.PlaySound(SoundID.Item43, player.position);
					}
				}
				if (flag && Main.myPlayer == Projectile.owner)
				{
					bool flag2 = player.channel && player.CheckMana(player.inventory[player.selectedItem].mana, true, false) && !player.noItems && !player.CCed;
					if (flag2)
					{
						float scaleFactor = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
						Vector2 value2 = vector;
						Vector2 value3 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - value2;
						if (player.gravDir == -1f)
						{
							value3.Y = Main.screenHeight - Main.mouseY + Main.screenPosition.Y - value2.Y;
						}
						Vector2 vector3 = Vector2.Normalize(value3);
						if (float.IsNaN(vector3.X) || float.IsNaN(vector3.Y))
						{
							vector3 = -Vector2.UnitY;
						}
						vector3 *= scaleFactor;
						if (vector3.X != Projectile.velocity.X || vector3.Y != Projectile.velocity.Y)
						{
							Projectile.netUpdate = true;
						}
						Projectile.velocity = vector3;
            			for (int I = 0; I < 1; I++)
            			{
							Vector2 perturbedSpeed = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10));
							Vector2 vector2_2 = Vector2.Normalize(Projectile.velocity) * 100;
            		    	int a = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, perturbedSpeed + vector2_2, ProjectileType<StaffofPrismLaser>(), Projectile.damage, 0f, player.whoAmI);
            			}
					}
					else
					{
						Projectile.Kill();
					}
				}
			}
			Projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - Projectile.Size / 2f;
			Projectile.rotation = Projectile.velocity.ToRotation() + num;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.timeLeft = 2;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction);
		}

	}
}
