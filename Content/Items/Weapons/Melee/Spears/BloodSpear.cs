using FM.Content.Projectiles.Melee.Spears.BloodSpearProjs;
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
	public class BloodSpear : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 5;
			Item.value = Item.sellPrice(0, 3, 50, 0);

			// Use Properties
			Item.useStyle = 5;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true; 

			// Weapon Properties
			Item.damage = 80;
			Item.knockBack = 6f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 14f;
			Item.shoot = ModContent.ProjectileType<BloodSpearProj>();
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
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<BloodSpearProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 26);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 60);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
