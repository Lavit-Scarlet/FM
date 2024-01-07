using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;

namespace FM.Content.Items.Materials
{
	public class ElectroBar : ModItem
	{
		public override void SetDefaults() 
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.rare = 6;
			Item.createTile = ModContent.TileType<Tiles.ElectroBarTile>();
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useTurn = true;
			Item.placeStyle = 0;
			Item.useStyle = 1;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.ElectroOre>(), 4);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.Register();
		}
	}
}