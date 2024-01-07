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

namespace FM.Content.Items.Weapons.Ranged.Guns
{
	public class T32 : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.useAmmo = AmmoID.Bullet;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 62;
			Item.useAnimation = 62;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = 2;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/T32") 
			{
				Volume = 0.7f,
				PitchVariance = 0.0f,
				MaxInstances = 1,
			};
			Item.autoReuse = true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
		    for (int i = 0; i < 3; i++)
		    {
		    	Vector2 perturbedSpeed1 = velocity.RotatedByRandom(MathHelper.ToRadians(2));
		    	perturbedSpeed1 *= 1f - Main.rand.NextFloat(0.4f);
		    	Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed1.X, perturbedSpeed1.Y, type, damage, knockback, player.whoAmI);
		    }
			return false;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 4);
			recipe.AddRecipeGroup("FM:IronBar", 6);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}