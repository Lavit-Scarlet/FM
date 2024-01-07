using FM.Content.Projectiles.Melee.Spears.IceSpearProjs;
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
	public class IceSpear : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 0, 20, 0);

			// Use Properties
			Item.useStyle = 5;
			Item.useAnimation = 8;
			Item.useTime = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false; 

			// Weapon Properties
			Item.damage = 10;
			Item.knockBack = 1f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<IceSpearProj>();
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
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(14));
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), perturbedSpeed, ProjectileType<IceSpearProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IceBlock, 18);
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
