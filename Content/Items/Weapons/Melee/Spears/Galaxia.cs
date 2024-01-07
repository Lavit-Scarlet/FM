using FM.Content.Projectiles.Melee.Spears.GalaxiaProjs;
using Terraria.Audio;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class Galaxia : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 11;
			Item.value = Item.sellPrice(0, 26, 50, 0);

			// Use Properties
			Item.useStyle = 5;
			Item.useAnimation = 4;
			Item.useTime = 4;
			//Item.reuseDelay = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true; 

			// Weapon Properties
			Item.damage = 180;
			Item.knockBack = 2f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<GalaxiaProj>();
		}

		public override bool CanUseItem(Player player) 
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player) 
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(14));
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), perturbedSpeed, ProjectileType<GalaxiaProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Spears.HeavenlySpear>(), 1);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 4);
			recipe.AddIngredient(ItemType<Materials.SpaceCatalyst>(), 2);
			recipe.AddIngredient(ItemType<Materials.PureSoulofPower>(), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
