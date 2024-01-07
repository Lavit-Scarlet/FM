using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.FlowerofTheNightProjs;
using FM.Content.Items.Weapons.Magic;

namespace FM.Content.Items.Weapons.Magic
{
	public class FlowerofTheNight : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 24;
			Item.shoot = ProjectileType<FlowerofTheNightProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 2, 80, 0);
			Item.staff[Item.type] = true;
			Item.rare = 4;
			Item.UseSound = SoundID.Item60;
			Item.autoReuse = true;
			Item.mana = 12;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 4);
			recipe.AddIngredient(ItemType<DemoniteFlower>(), 1);
			recipe.AddIngredient(ItemID.FlowerofFire, 1);
			recipe.AddIngredient(ItemType<IceFlower>(), 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 4);
			recipe.AddIngredient(ItemType<CriptaneFlower>(), 1);
			recipe.AddIngredient(ItemID.FlowerofFire, 1);
			recipe.AddIngredient(ItemType<IceFlower>(), 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
