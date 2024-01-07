using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.TerraFlowerProjs;
using FM.Content.Items.Weapons.Magic;

namespace FM.Content.Items.Weapons.Magic
{
	public class TerraFlower : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 80;
			Item.shoot = ProjectileType<TerraFlowerProj>();
			Item.shootSpeed = 15;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 8, 0, 0);
			Item.staff[Item.type] = true;
			Item.rare = 8;
			Item.UseSound = SoundID.Item60;
			Item.autoReuse = true;
			Item.mana = 14;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<FlowerofTheNight>(), 1);
			recipe.AddIngredient(ItemType<FlanricRose>(), 1);
			recipe.AddIngredient(ItemType<HallowedFlower>(), 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 16);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
