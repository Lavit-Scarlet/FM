using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Melee.Spears.FullMoonSpearProjs;
using FM.Globals;
using static Terraria.ModLoader.ModContent;
using FM.Content.Items.Weapons.Melee.Spears;

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class FullMoonSpear : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 28;
			Item.DamageType = DamageClass.Melee;
			Item.width = 66;
			Item.height = 66;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 5;
			Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 3, 60, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<FullMoonSpearProj>(); 
            Item.shootSpeed = 6.2f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
        public override bool CanUseItem(Player player)
        {
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FullMoonSpearProj>(), damage, knockback, player.whoAmI, FMPlayer.ApplyAttackGeneric(player, Item.DamageType, Item.useTime));
			return false; 
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<BaneSpear>(), 1);
			recipe.AddIngredient(ItemID.DarkLance, 1);
			recipe.AddIngredient(ItemType<HerbalSpear>(), 1);
			recipe.AddIngredient(ItemType<FireSpear>(), 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TheRottedFork, 1);
			recipe.AddIngredient(ItemID.DarkLance, 1);
			recipe.AddIngredient(ItemType<HerbalSpear>(), 1);
			recipe.AddIngredient(ItemType<FireSpear>(), 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
    }
}
	
