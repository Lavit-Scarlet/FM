using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using FM.Globals;
using FM.Content.Items.Materials;

namespace FM.Content.Items.Armor.PowerArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class PowerBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Power Plate");
            //Tooltip.SetDefault("Increases damage by 5%\n" +
            //"Increases critical strike chance by 8");
            //SacrificeTotal = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += .05f;
            player.GetCritChance(DamageClass.Generic) += 8;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 26;
            Item.sellPrice(silver: 75);
            Item.rare = 6;
            Item.defense = 16;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<ElectroBar>(), 14);
            recipe.AddRecipeGroup("FM:TitaniumBar", 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}