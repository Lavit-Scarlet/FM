using FM.Content.Projectiles.Melee.Spears.TrueFullMoonSpearProjs;
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
	public class TrueFullMoonSpear : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 10;
			Item.value = Item.sellPrice(0, 10, 50, 0);

			// Use Properties
			Item.useStyle = 5;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true; 

			// Weapon Properties
			Item.damage = 65;
			Item.knockBack = 6f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<TrueFullMoonSpearProj>();
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
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<TrueFullMoonSpearProj>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(60, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<TrueFullMoonSpearShootProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<FullMoonSpear>(), 1);
			recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddIngredient(ItemID.SoulofFright, 8);
            recipe.AddIngredient(ItemID.SoulofSight, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
