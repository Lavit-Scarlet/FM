using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.Audio;
using FM.Content.Projectiles.Ranged.Bows;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class FlanricBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.rare = 6;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 3, 55, 0);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(1, 0);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 12);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 60);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (int i = 0; i < 2; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(6));

				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false;
        }
	}
}