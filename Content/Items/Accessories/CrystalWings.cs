using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace FM.Content.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class CrystalWings : ModItem
	{
		public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			// These wings use the same values as the solar wings
			// Fly time: 180 ticks = 3 seconds
			// Fly speed: 9
			// Acceleration multiplier: 2.5
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(240, 11f, 3.2f);
		}

		public override void SetDefaults() 
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 13, 34, 0);
			Item.rare = 10;
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) 
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.DarkBar>(), 6);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 500);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 12);
			recipe.AddIngredient(ItemType<Materials.SpaceCatalyst>(), 2);
			recipe.AddIngredient(ItemID.Amethyst, 2);
			recipe.AddIngredient(ItemID.Topaz, 2);
			recipe.AddIngredient(ItemID.Sapphire, 2);
			recipe.AddIngredient(ItemID.Emerald, 2);
			recipe.AddIngredient(ItemID.Ruby, 2);
			recipe.AddIngredient(ItemID.Diamond, 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
