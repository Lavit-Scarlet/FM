using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.CrystalStaffProjs;

namespace FM.Content.Items.Weapons.Magic
{
	public class CrystalStaff : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 50;
			Item.shoot = ProjectileType<CrystalStaffProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 4, 75, 0);
			Item.staff[Item.type] = true;
			Item.rare = 5;
			Item.UseSound = SoundID.Item82;
			Item.autoReuse = true;
			Item.crit = 0;
			Item.mana = 10;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 5);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 34);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

}
