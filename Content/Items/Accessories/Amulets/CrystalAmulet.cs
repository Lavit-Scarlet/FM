using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace FM.Content.Items.Accessories.Amulets
{
	public class CrystalAmulet : ModItem
    {
        public override void SetDefaults() 
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 2, 76, 0);
			Item.rare = 5;
            Item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.GetDamage(DamageClass.Magic) += 0.12f;
            player.GetCritChance(DamageClass.Magic) += 6f;
            player.statManaMax2 += 40;
            player.manaCost -= 0.08f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.04f;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Chain, 1);
			recipe.AddIngredient(ItemID.CrystalShard, 2);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}