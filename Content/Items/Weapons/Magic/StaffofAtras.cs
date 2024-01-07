using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic;

namespace FM.Content.Items.Weapons.Magic
{
	public class StaffofAtras : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 44;
			Item.shoot = ProjectileType<StaffofAtrasProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.staff[Item.type] = true;
			Item.rare = 4;
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.crit = 0;
			Item.mana = 8;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("FM:TitaniumBar", 12);
			recipe.AddRecipeGroup("FM:IronBar", 4);
			recipe.AddIngredient(ItemID.SoulofNight, 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

}
