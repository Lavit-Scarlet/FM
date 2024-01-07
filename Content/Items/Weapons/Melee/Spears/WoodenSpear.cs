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
	public class WoodenSpear : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Melee;
			Item.width = 66;
			Item.height = 66;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.6f;
            Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<WoodenSpearProj>(); 
            Item.shootSpeed = 6.2f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			//Item.channel = true;
		}
        public override bool CanUseItem(Player player)
        {
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
    }
}
	
