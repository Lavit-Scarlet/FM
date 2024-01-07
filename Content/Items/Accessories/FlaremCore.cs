using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.GameContent;

namespace FM.Content.Items.Accessories
{
	public class FlaremCore : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}
		public override void SetDefaults() 
		{
			Item.width = 34;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 8, 0, 0);
			Item.rare = 7;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.10f;
            player.GetCritChance(DamageClass.Generic) += 4;
			player.statManaMax2 += 40;
            player.statLifeMax2 += 40;
			player.maxMinions += 2;
			player.GetArmorPenetration(DamageClass.Generic) += 4;
			player.lifeRegen += 2;
			player.noKnockback = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddRecipeGroup("FM:TitaniumBar", 2);
			recipe.AddIngredient(ItemID.SoulofNight, 20);
			recipe.AddIngredient(ItemID.SoulofFright, 10);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 120);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 8);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}