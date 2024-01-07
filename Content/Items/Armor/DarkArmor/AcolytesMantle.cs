using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Items.Materials;
using static Terraria.ModLoader.ModContent;

namespace FM.Content.Items.Armor.DarkArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class AcolytesMantle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acolyte's Mantle");
            //Tooltip.SetDefault("Increases magic damage by 5% and critical strike by 8\n" +
            //"Increases magic attack speed by 4%");
            //SacrificeTotal = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += .05f;
            player.GetCritChance(DamageClass.Magic) += 8;
            player.GetAttackSpeed(DamageClass.Magic) += 0.04f;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 26;
            Item.sellPrice(silver: 50);
            Item.rare = 4;
            Item.defense = 7;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<DarkBar>(), 6);
			recipe.AddIngredient(ItemID.Silk, 18);
            recipe.AddIngredient(ItemID.Bone, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}