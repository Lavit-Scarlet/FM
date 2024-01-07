using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Items.Placeable.MusicBoxes;
using FM.Content.Tiles;

namespace FM.Content.Items.Placeable.MusicBoxes
{
	public class ArmagemMusicBoxItem : ModItem
	{
		public override void SetStaticDefaults()
		{
            ItemID.Sets.CanGetPrefixes[Type] = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
			Item.ResearchUnlockCount = 1;

			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/Armagem"), ModContent.ItemType<ArmagemMusicBoxItem>(), ModContent.TileType<ArmagemMusicBoxTile>());
		}
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<ArmagemMusicBoxTile>(), 0);
			Item.createTile = ModContent.TileType<ArmagemMusicBoxTile>();
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MusicBox, 1);
			recipe.AddIngredient(ItemType<Materials.PureSoulofPower>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}