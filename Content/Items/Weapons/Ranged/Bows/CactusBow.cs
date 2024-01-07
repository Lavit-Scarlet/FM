using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class CactusBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.shoot = 1;
			Item.shootSpeed = 5;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 0, 12, 0);
			Item.rare = 1;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Cactus, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}