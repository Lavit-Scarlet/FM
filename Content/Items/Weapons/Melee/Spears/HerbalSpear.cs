using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Melee.Spears.HerbalSpearProjs;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class HerbalSpear : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3.6f;
            Item.value = Item.sellPrice(0, 0, 84, 0);
			Item.rare = 3;
			Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<HerbalSpearProj>(); 
            Item.shootSpeed = 5.2f;
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
			recipe.AddIngredient(ItemID.JungleSpores, 16);
            recipe.AddIngredient(ItemID.Stinger, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}
	
