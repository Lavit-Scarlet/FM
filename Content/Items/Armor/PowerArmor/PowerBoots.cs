using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using FM.Globals;
using FM.Content.Items.Materials;

namespace FM.Content.Items.Armor.PowerArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PowerBoots : ModItem
    {
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Power Boots");
			//Tooltip.SetDefault("Increases damage by 5%\n" +
			//"Increases movement speed by 10%");

			//SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.sellPrice(silver: 45);
			Item.rare = 6;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += .05f;
			player.moveSpeed += .10f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<ElectroBar>(), 10);
            recipe.AddRecipeGroup("FM:TitaniumBar", 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}