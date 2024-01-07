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
	public class DarkBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.shoot = 1;
			Item.shootSpeed = 8;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 4;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 66, 0);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (int i = 0; i < 1; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(12));

				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.DarkBar>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}