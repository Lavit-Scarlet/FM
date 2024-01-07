using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Knifes.NightNightmareProjs;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using FM.Content.Items.Weapons.Ranged.Knifes;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class NightNightmare : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.shoot = ProjectileType<NightNightmareProj>();
			Item.shootSpeed = 14;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.rare = 5;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 2, 60, 0);
			Item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<CrimsonKnife>(), 60);
			recipe.AddIngredient(ItemType<FireKnife>(), 60);
			recipe.AddIngredient(ItemType<HerbalKnife>(), 60);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<PenetratingDarkness>(), 60);
			recipe.AddIngredient(ItemType<FireKnife>(), 60);
			recipe.AddIngredient(ItemType<HerbalKnife>(), 60);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
