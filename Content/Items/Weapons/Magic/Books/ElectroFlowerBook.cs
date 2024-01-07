using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class ElectroFlowerBook : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 68;
			Item.shoot = ProjectileType<ElectroFlowerBookProj>();
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = 5;
			Item.knockBack = 1;
			Item.rare = 6;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 5, 55, 0);
			Item.mana = 20;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 6);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}

	}
}