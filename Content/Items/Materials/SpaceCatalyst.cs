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
	public class SpaceCatalyst : ModItem
	{
		public override void SetStaticDefaults() 
		{
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 7));
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void SetDefaults() 
		{
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = 11;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FragmentNebula, 2);
			recipe.AddIngredient(ItemID.FragmentSolar, 2);
			recipe.AddIngredient(ItemID.FragmentVortex, 2);
			recipe.AddIngredient(ItemID.FragmentStardust, 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}