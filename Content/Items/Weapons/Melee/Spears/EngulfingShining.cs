using FM.Content.Projectiles.Melee.Spears.EngulfingShiningProjs;
using Terraria.Audio;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class EngulfingShining : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 10;
			Item.value = Item.sellPrice(0, 25, 50, 0);

			// Use Properties
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useAnimation = 10;
			Item.useTime = 10;
			//Item.UseSound = SoundID.Item60;
			Item.autoReuse = true; 

			// Weapon Properties
			Item.damage = 60;
			Item.knockBack = 2f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<EngulfingShiningSlash>();
		}
		public override float UseSpeedMultiplier(Player player)
        {
            float Mult = base.UseSpeedMultiplier(player);
            if (AttackType == 6) Mult *= .5f;
            if (AttackType == 7) Mult *= .5f;
            return Mult;
        }
        private bool XZ = false;
        private int AttackType = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (AttackType <= 2)
            {
                Item.useAnimation = 10;
		    	Item.useTime = 10;
            }
            else
            {
                Item.useAnimation = 20;
		    	Item.useTime = 20;
            }
            if(AttackType < 4)
            {
				Item.useStyle = ItemUseStyleID.Shoot;
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(14));
                Projectile.NewProjectileDirect(source, position, perturbedSpeed, ModContent.ProjectileType<EngulfingShiningSlash2>(), damage, knockback, player.whoAmI, 0f, XZ ? 1f : -1f) .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                AttackType += 1;
            }
            else if(AttackType < 6)
            {
                Item.useStyle = ItemUseStyleID.Rapier;
                Projectile.NewProjectileDirect(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 0f, XZ ? 1f : 0f) .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                AttackType += 1;
            }
            else if(AttackType < 8)
            {
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<EngulfingShiningSlash3>(), damage, knockback, player.whoAmI, 0f, XZ ? 1f : 0f) .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                AttackType += 1;
            }
            else if(AttackType == 8)
            {
                Projectile.NewProjectileDirect(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 0f, XZ ? 1f : -1f) .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                AttackType += 1;
            }
            else if(AttackType >= 9)
            {
                Projectile.NewProjectileDirect(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 0f, XZ ? 1f : -1f) .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                Item.useAnimation = 10;
		    	Item.useTime = 10;
                AttackType = 0;
            }
            return false;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<BaneSpear>(), 1);
			recipe.AddIngredient(ItemID.DarkLance, 1);
			recipe.AddIngredient(ItemType<Shining>(), 1);
			recipe.AddIngredient(ItemType<FullMoonSpear>(), 1);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TheRottedFork, 1);
			recipe.AddIngredient(ItemID.DarkLance, 1);
			recipe.AddIngredient(ItemType<Shining>(), 1);
			recipe.AddIngredient(ItemType<FullMoonSpear>(), 1);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
