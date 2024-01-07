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
using FM.Content.Projectiles.Melee.Swords;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class DarkSword : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<DarkSwordProj>();
			Item.shootSpeed = 10;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.width = 17;
			Item.height = 61;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 1, 24, 0);
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.crit = 0;
			Item.scale = 1f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.DarkBar>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}