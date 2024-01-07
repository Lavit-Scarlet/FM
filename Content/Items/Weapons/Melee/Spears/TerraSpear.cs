using FM.Content.Projectiles.Melee.Spears.TerraSpearProjs;
using Terraria.Audio;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Items.Weapons.Melee.Spears;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class TerraSpear : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 8;
			Item.value = Item.sellPrice(0, 12, 50, 0);

			// Use Properties
			Item.useStyle = 5;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.UseSound = SoundID.Item60;
			Item.autoReuse = true; 

			// Weapon Properties
			Item.damage = 80;
			Item.knockBack = 5f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<TerraSpearProj>();
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
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<TerraSpearProj>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(60, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<TerraSpearShootProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<SpearOfTheSun>(), 1);
			recipe.AddIngredient(ItemType<TrueFullMoonSpear>(), 1);
			recipe.AddIngredient(ItemType<Dionaea>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
