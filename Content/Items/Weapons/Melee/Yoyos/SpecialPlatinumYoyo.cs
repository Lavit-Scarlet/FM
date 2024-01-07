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
using FM.Content.Projectiles.Melee.Yoyos.SpecialPlatinumYoyoProjs;

namespace FM.Content.Items.Weapons.Melee.Yoyos
{
	public class SpecialPlatinumYoyo : ModItem
	{
		public override void SetStaticDefaults() 
		{
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
		}

		public override void SetDefaults() 
		{
			Item.damage = 60;
            Item.useStyle = 5;
            Item.width = 30;
            Item.height = 30;

            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.shootSpeed = 14f;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 5, 40, 0);
            Item.rare = 6;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<SpecialPlatinumYoyoProj>();
		}
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Yoyos.PlatinumYoyo>());
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddIngredient(ItemID.SoulofMight, 4);
			recipe.AddIngredient(ItemID.SoulofFright, 4);
			recipe.AddIngredient(ItemID.SoulofSight, 4);
            recipe.AddIngredient(ItemID.Diamond, 1);
			recipe.AddIngredient(ItemID.Lens, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
