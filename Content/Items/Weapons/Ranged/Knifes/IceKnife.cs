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
	public class IceKnife : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.shoot = ProjectileType<IceKnifeProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.useStyle = 1;
			Item.knockBack = 1;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 0, 05);
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 9999;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(6);
			recipe.AddIngredient(ItemID.Wood, 1);
			recipe.AddIngredient(ItemID.IceBlock, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
