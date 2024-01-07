using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using FM.Globals;
using FM.Content.Projectiles.Melee.Swords.FuryofTheGalaxyProjs;
using FM.Content.Items.Weapons.Melee.Swords;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class FuryofTheGalaxy : ModItem
	{
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
		public override void SetDefaults() 
		{
			Item.damage = 180;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Melee;
			Item.width = 77;
			Item.height = 77;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 8;
			Item.value = Item.sellPrice(0, 30, 0, 0);
			Item.rare = 11;
			Item.UseSound = SoundID.Item105;
			Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Melee/Swords/FuryofTheGalaxy_Glow").Value;
            }
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.damage = 180;
				Item.useTime = 34;
			    Item.useAnimation = 34;
				Item.shootSpeed = 10;
				Item.shoot = ProjectileType<FuryofTheGalaxyAltProj>();
				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				if (modPlayer.shootDelay == 0)
					return true;
				return false;
			}
			else
			{
				Item.damage = 180;
				Item.useTime = 34;
			    Item.useAnimation = 34;
				Item.shootSpeed = 10;
				Item.shoot = ProjectileType<FuryofTheGalaxyProj>();
			}
			return true;
		}
        private bool XZ = false;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (player.altFunctionUse == 2)
			{
				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				modPlayer.shootDelay = 600;
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 14, 0, ModContent.ProjectileType<FuryofTheGalaxyAltProj>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, -14, 0, ModContent.ProjectileType<FuryofTheGalaxyAltProj>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0, 14, ModContent.ProjectileType<FuryofTheGalaxyAltProj>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0, -14, ModContent.ProjectileType<FuryofTheGalaxyAltProj>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 10, 10, ModContent.ProjectileType<FuryofTheGalaxyAltProj>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 10, -10, ModContent.ProjectileType<FuryofTheGalaxyAltProj>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, -10, -10, ModContent.ProjectileType<FuryofTheGalaxyAltProj>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, -10, 10, ModContent.ProjectileType<FuryofTheGalaxyAltProj>(), damage = 360, knockback, Main.myPlayer);

				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 14, 0, ModContent.ProjectileType<FuryofTheGalaxyAltProj2>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, -14, 0, ModContent.ProjectileType<FuryofTheGalaxyAltProj2>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0, 14, ModContent.ProjectileType<FuryofTheGalaxyAltProj2>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0, -14, ModContent.ProjectileType<FuryofTheGalaxyAltProj2>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 10, 10, ModContent.ProjectileType<FuryofTheGalaxyAltProj2>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 10, -10, ModContent.ProjectileType<FuryofTheGalaxyAltProj2>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, -10, -10, ModContent.ProjectileType<FuryofTheGalaxyAltProj2>(), damage = 360, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, -10, 10, ModContent.ProjectileType<FuryofTheGalaxyAltProj2>(), damage = 360, knockback, Main.myPlayer);
				return player.altFunctionUse != 2;
			}
			else
			{
				int i = Main.myPlayer;
				float num72 = Item.shootSpeed * 2;
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
					vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 300);
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
					float speedX4 = num78 + Main.rand.Next(-60, 61) * 0.00f;
					float speedY5 = num79 + Main.rand.Next(-60, 61) * 0.00f;
					Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, ModContent.ProjectileType<FuryofTheGalaxyProj>(), num73, num74, i, 0f, Main.rand.Next(0));
				}
				Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<FuryofTheGalaxySlash>(), damage, knockback, player.whoAmI, 0f, XZ ? 1f : -1f)
                .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
				return false;
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<HeavenlySword>(), 1);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 6);
			recipe.AddIngredient(ItemType<Materials.SpaceCatalyst>(), 4);
			recipe.AddIngredient(ItemType<Materials.PureSoulofPower>(), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}