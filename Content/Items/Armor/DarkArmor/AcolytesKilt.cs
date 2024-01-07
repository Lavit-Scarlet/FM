using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using FM.Content.Items.Materials;
using static Terraria.ModLoader.ModContent;

namespace FM.Content.Items.Armor.DarkArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class AcolytesKilt : ModItem
    {
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Acolyte's Kilt");
			//Tooltip.SetDefault("Increases magic damage by 5% and movement speed by 8%");

			//SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.sellPrice(silver: 30);
			Item.rare = 4;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += .05f;
			player.moveSpeed += .08f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<DarkBar>(), 4);
			recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(ItemID.Bone, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}