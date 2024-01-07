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
using FM.Content.Projectiles.Melee.ChakramofFateProjs;

namespace FM.Content.Items.Weapons.Melee
{
	public class ChakramofFate : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 46;
			Item.shoot = ProjectileType<ChakramofFateProj>();
			Item.shootSpeed = 14f;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.width = 62;
			Item.height = 62;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
		}
		public override bool CanUseItem(Player player)
		{
			for (int i = 0; i < 1000; ++i) 
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot) 
				{
					return false;
				}
			}
			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.SlushofFate>(), 22);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}