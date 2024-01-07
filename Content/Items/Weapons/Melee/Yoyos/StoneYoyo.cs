using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Melee.Yoyos;

namespace FM.Content.Items.Weapons.Melee.Yoyos
{
	public class StoneYoyo : ModItem
	{
		public override void SetStaticDefaults() 
		{
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
		}

		public override void SetDefaults() 
		{
			Item.damage = 18;
            Item.useStyle = 5;
            Item.width = 30;
            Item.height = 30;

            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 10f;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 0, 0, 62);
            Item.rare = 1;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<StoneYoyoProj>();
		}
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddIngredient(ItemID.WhiteString, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
