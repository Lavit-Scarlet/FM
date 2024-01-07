using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class GraniteBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.rare = 2;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 45, 0);
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/GraniteBow_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(1, 0);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GraniteBlock, 30);
            recipe.AddRecipeGroup("FM:PlatinumBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}