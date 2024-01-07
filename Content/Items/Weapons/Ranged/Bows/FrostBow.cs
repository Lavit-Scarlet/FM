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
using FM.Content.Projectiles.Ranged.Ammo;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class FrostBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.shoot = 1;
			Item.shootSpeed = 8;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 1;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 18, 0);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(1, 0);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IceBlock, 30);
			recipe.AddIngredient(ItemID.Wood, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ProjectileType<IceArrowProj>();
			}
		}
	}
}