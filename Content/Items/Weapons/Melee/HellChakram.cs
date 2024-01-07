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
using FM.Content.Projectiles.Melee;

namespace FM.Content.Items.Weapons.Melee
{
	public class HellChakram : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.shoot = ProjectileType<HellChakramProj>();
			Item.shootSpeed = 12f;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.width = 62;
			Item.height = 62;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 20);
			recipe.AddIngredient(ItemID.SoulofNight, 4);
			recipe.AddIngredient(ItemID.SoulofLight, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}