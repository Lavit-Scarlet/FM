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
	public class ArmagemCore : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}
		public override void SetDefaults() 
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = 5;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.06f;
			player.GetCritChance(DamageClass.Generic) += 2;
			player.statManaMax2 += 20;
            player.statLifeMax2 += 20;
			player.maxMinions += 1;
			player.GetArmorPenetration(DamageClass.Generic) += 2;
			player.lifeRegen += 1;
			player.noKnockback = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 2);
			recipe.AddRecipeGroup("FM:IronBar", 2);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.Obsidian, 2);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}