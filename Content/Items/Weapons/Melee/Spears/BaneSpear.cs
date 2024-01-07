using Terraria.Audio;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Melee.Spears.BaneSpearProjs;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class BaneSpear : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;
			Item.width = 66;
			Item.height = 66;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<BaneSpearProj>(); 
            Item.shootSpeed = 10f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
        public override bool CanUseItem(Player player)
        {
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float rot = (velocity).ToRotation();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(10));
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), perturbedSpeed, ProjectileType<BaneSpearProj>(), damage, knockback, player.whoAmI);
			return false; 
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}
	
