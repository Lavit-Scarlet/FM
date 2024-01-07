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
using FM.Content.Projectiles.Other.ScarletGungnirProjs;
using FM.Content.Items.Materials;

namespace FM.Content.Items.Weapons.Other
{
	public class ScarletGungnir : ModItem
	{
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.damage = 666;
			//Item.DamageType = DamageClass.Ranged;
			Item.width = 94;
			Item.height = 94;
			Item.shoot = ProjectileType<ScarletGungnirProj>();
			Item.shootSpeed = 10f;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 20, 50, 0);
			Item.rare = 10;
			Item.autoReuse = true;
			Item.useAnimation = 81;
			Item.useTime = 81;
			Item.scale = 0;
			Item.UseSound = SoundID.Item60;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.SoulofFright, 12);
			recipe.AddIngredient(ItemID.SoulofMight, 12);
			recipe.AddIngredient(ItemID.SoulofSight, 12);
			recipe.AddIngredient(ItemType<ScarletFragment>(), 666);
			recipe.AddIngredient(ItemType<FlanricCrystal>(), 66);
			recipe.AddIngredient(ItemType<PureSoulofPower>(), 6);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
