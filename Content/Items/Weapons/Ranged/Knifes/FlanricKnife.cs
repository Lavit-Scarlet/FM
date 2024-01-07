using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Knifes;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class FlanricKnife : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 40;
			Item.shoot = ProjectileType<FlanricKnifeProj>();
			Item.shootSpeed = 14;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 0, 0, 26);
			Item.rare = 5;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 9999;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(3);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 4);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
