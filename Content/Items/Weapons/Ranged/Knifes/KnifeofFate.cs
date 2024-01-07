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
using FM.Content.Projectiles.Ranged.Knifes;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class KnifeofFate : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 34;
			Item.shoot = ProjectileType<KnifeofFateProj>();
			Item.shootSpeed = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.SlushofFate>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
