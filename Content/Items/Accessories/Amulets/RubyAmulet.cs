using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace FM.Content.Items.Accessories.Amulets
{
	public class RubyAmulet : ModItem
    {
        public override void SetDefaults() 
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 36, 0);
			Item.rare = 2;
            Item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.GetDamage(DamageClass.Magic) += 0.08f;
            player.GetCritChance(DamageClass.Magic) += 4f;
            player.statManaMax2 += 40;
            player.manaCost -= 0.06f;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Chain, 1);
			recipe.AddIngredient(ItemID.Ruby, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}