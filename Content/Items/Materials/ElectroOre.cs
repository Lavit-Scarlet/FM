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
	public class ElectroOre : ModItem
	{
		public override void SetDefaults() 
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 6;
			Item.createTile = ModContent.TileType<Tiles.ElectroOreTile>();
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useTurn = true;
			Item.placeStyle = 0;
			Item.useStyle = 1;
		}
	}
}