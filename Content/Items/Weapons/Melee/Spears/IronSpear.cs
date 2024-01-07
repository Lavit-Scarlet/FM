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
	public class IronSpear : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Melee;
			Item.width = 66;
			Item.height = 66;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<IronSpearProj>(); 
            Item.shootSpeed = 4f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override bool CanUseItem(Player player) 
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("FM:IronBar", 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}
	
