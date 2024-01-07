using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Magic.Books.SpectralKillerProjs;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class SpectralKiller : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.shoot = ProjectileType<SpectralKillerPricelProj>();
			Item.shootSpeed = 0f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 42;
			Item.height = 36;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = 5;
			Item.knockBack = 0;
			Item.rare = 10;
			//Item.UseSound = SoundID.Item122;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 10, 95, 0);
			Item.mana = 40;
		}

		public override bool CanUseItem(Player player)
    	{
     		FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
    		if (modPlayer.shootDelay == 0)
    		{
    			return true;
    		}
    		return false;
    	}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
			modPlayer.shootDelay = 180;
			Projectile.NewProjectile(source, position.X, position.Y, 0, 0, ProjectileType<SpectralKillerPricelProj>(), damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 12);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 6);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 160);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}