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
	public class FireKnife : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 32;
			Item.shoot = ProjectileType<FireKnifeProj>();
			Item.shootSpeed = 10;
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
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 9999;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(3);
			recipe.AddIngredient(ItemID.HellstoneBar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
