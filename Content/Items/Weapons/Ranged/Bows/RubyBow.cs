using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using FM.Content.Projectiles.Ranged.Bows.RubyBowProjs;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class RubyBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = 5;
			Item.shoot = ModContent.ProjectileType<RubyBowProj>();
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 2;
			Item.autoReuse = true;
			Item.shootSpeed = 10;
			Item.UseSound = SoundID.Item5;
		}
		public override Vector2? HoldoutOffset()
		{
			return Vector2.Zero;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Ruby, 3);
			recipe.AddRecipeGroup("FM:PlatinumBar", 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
		    for (int i = 0; i < 1; i++)
		    {
		    	Vector2 perturbedSpeed1 = velocity.RotatedByRandom(MathHelper.ToRadians(2));
		    	Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed1.X, perturbedSpeed1.Y, ModContent.ProjectileType<RubyBowProj>(), damage, knockback, player.whoAmI);
		    }
			return false;
        }
	}
}