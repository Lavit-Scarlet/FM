using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.Audio;
using FM.Content.Projectiles.Magic;

namespace FM.Content.Items.Weapons.Magic
{
	public class WorldStaff : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 132;
			Item.shoot = ProjectileType<WorldStaffProj>();
			Item.shootSpeed = 13f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 14, 0, 0);
			Item.staff[Item.type] = true;
			Item.rare = 10;
			Item.UseSound = SoundID.Item72;
			Item.crit += 22;
			Item.mana = 20;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/WorldStaff_Glow").Value;
            }
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
            type = ModContent.ProjectileType<WorldStaffProj>();
			int i = Main.myPlayer;
			float num72 = Item.shootSpeed;
			int num73 = Item.damage;
			float num74 = Item.knockBack;
			num74 = player.GetWeaponKnockback(Item, num74);
			player.itemTime = Item.useTime;
			Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 value = Vector2.UnitX.RotatedBy(player.fullRotation, default(Vector2));
			Vector2 vector3 = Main.MouseWorld - vector2;
			float num78 = Main.mouseX + Main.screenPosition.X - vector2.X;
			float num79 = Main.mouseY + Main.screenPosition.Y - vector2.Y;
			if (player.gravDir == -1f)
			{
				num79 = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
			}
			float num80 = (float)Math.Sqrt(num78 * num78 + num79 * num79);
			float num81 = num80;
			if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f))
			{
				num78 = player.direction;
				num79 = 0f;
				num80 = num72;
			}
			else
			{
				num80 = num72 / num80;
			}
			num78 *= num80;
			num79 *= num80;
			int num107 = 3;
			for (int num108 = 0; num108 < num107; num108++)
			{
				vector2 = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(201) * -(float)player.direction + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
				vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
				vector2.Y -= 100 * num108;
				num78 = Main.mouseX + Main.screenPosition.X - vector2.X;
				num79 = Main.mouseY + Main.screenPosition.Y - vector2.Y;
				if (num79 < 0f)
				{
					num79 *= -1f;
				}
				if (num79 < 20f)
				{
					num79 = 20f;
				}
				num80 = (float)Math.Sqrt(num78 * num78 + num79 * num79);
				num80 = num72 / num80;
				num78 *= num80;
				num79 *= num80;
				float speedX4 = num78 + Main.rand.Next(-0, 0) * 0.02f;
				float speedY5 = num79 + Main.rand.Next(-0, 0) * 0.02f;
				Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, type, num73, num74, i, 0f, Main.rand.Next(1));
			}
			FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
			modPlayer.Shake += 6;
			return false;
		}
	}
}
