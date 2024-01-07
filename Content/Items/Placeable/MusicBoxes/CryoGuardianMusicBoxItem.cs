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
	public class CryoGuardianMusicBoxItem : ModItem
	{
		public override void SetStaticDefaults()
		{
            ItemID.Sets.CanGetPrefixes[Type] = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
			Item.ResearchUnlockCount = 1;

			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/CryoGuardian"), ModContent.ItemType<CryoGuardianMusicBoxItem>(), ModContent.TileType<CryoGuardianMusicBoxTile>());
		}
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<CryoGuardianMusicBoxTile>(), 0);
			Item.createTile = ModContent.TileType<CryoGuardianMusicBoxTile>();
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
	}
}