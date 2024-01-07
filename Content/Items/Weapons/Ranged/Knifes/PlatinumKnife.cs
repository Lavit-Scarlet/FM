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
	public class PlatinumKnife : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.shoot = ProjectileType<PlatinumKnifeProj>();
			Item.shootSpeed = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 28;
			Item.height = 28;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 0, 12);
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 9999;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(3);
			recipe.AddIngredient(ItemID.PlatinumBar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
