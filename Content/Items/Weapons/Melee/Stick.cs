using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FM.Content.Items.Weapons.Melee
{
	public class Stick : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 4;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = 10;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 1);
			//recipe.AddTile();
			recipe.Register();
		}
	}
}