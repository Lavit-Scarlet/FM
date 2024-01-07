using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.Audio;
using FM.Content.Projectiles.Magic.Books.HistoryBookRemakeProjs;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class HistoryBook : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 120;
			Item.shoot = ProjectileType<HistoryBookRemakeProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.rare = 10;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = false;
			Item.channel = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ItemID.SoulofFright, 2);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 200);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 6);
			recipe.Register();
		}
	}
}
