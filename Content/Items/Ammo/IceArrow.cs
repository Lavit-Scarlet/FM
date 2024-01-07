using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Ranged.Ammo;

namespace FM.Content.Items.Ammo
{
    public class IceArrow : ModItem
    {
		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.rare = 1;
			Item.shoot = ProjectileType<IceArrowProj>();
			Item.shootSpeed = 5;
			Item.ammo = AmmoID.Arrow;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(10);
			recipe.AddIngredient(ItemID.WoodenArrow, 10);
			recipe.AddIngredient(ItemID.IceBlock, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

	}

}