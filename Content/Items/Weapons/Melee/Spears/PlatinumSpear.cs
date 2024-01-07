using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Melee.Spears;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class PlatinumSpear : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3.6f;
            Item.value = Item.sellPrice(0, 0, 24, 0);
			Item.rare = 1;
			Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<PlatinumSpearProj>(); 
            Item.shootSpeed = 5.5f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			//Item.channel = true;
		}
        public override bool CanUseItem(Player player)
        {
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, FMPlayer.ApplyAttackGeneric(player, Item.DamageType, Item.useTime));
			return false; 
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.PlatinumBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}
	
