using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Knifes;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class PenetratingDarkness : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.shoot = ProjectileType<PenetratingDarknessProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.useStyle = 1;
			Item.knockBack = 1;
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 0, 14);
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 9999;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(3);
			recipe.AddIngredient(ItemID.DemoniteBar, 1);
			recipe.AddIngredient(ItemID.ShadowScale, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
