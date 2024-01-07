using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class MarbleBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.shoot = 1;
			Item.shootSpeed = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 2;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 55, 0);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(1, 0);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MarbleBlock, 14);
            recipe.AddRecipeGroup("FM:PlatinumBar", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}