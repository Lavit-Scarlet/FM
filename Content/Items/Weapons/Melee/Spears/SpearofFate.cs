using FM.Content.Projectiles.Melee.Spears.SpearofFateProjs;
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
	public class SpearofFate : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 5;
			Item.value = Item.sellPrice(0, 3, 50, 0);

			// Use Properties
			Item.useStyle = 5;
			Item.useAnimation = 24;
			Item.useTime = 24;
			Item.UseSound = SoundID.DD2_SonicBoomBladeSlash;
			Item.autoReuse = true; 

			// Weapon Properties
			Item.damage = 52;
			Item.knockBack = 5f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 5f;
			Item.shoot = ModContent.ProjectileType<SpearofFateProj>();
		}

		public override bool CanUseItem(Player player) 
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player) 
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, FMPlayer.ApplyAttackGeneric(player, Item.DamageType, Item.useTime));
			return false; 
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.SlushofFate>(), 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
