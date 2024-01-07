using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Knifes;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class ElectroKnife : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 52;
			Item.shoot = ProjectileType<ElectroKnifeProj>();
			Item.shootSpeed = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.rare = 6;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 0, 60);
			Item.noUseGraphic = true;
			Item.maxStack = 9999;
			Item.consumable = true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(3);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<ElectroKnifeProj>(), damage, knockback, player.whoAmI);
			}
            return false;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(3);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
