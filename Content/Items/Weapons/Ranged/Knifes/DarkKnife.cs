using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using FM.Content.Projectiles.Ranged.Knifes;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class DarkKnife : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.shoot = ProjectileType<DarkKnifeProj>();
			Item.shootSpeed = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 0, 10);
			Item.noUseGraphic = true;
			Item.maxStack = 9999;
			Item.consumable = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(3);
			recipe.AddIngredient(ItemType<Materials.DarkBar>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
