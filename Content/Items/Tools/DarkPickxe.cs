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

namespace FM.Content.Items.Tools
{
    public class DarkPickxe : ModItem
    {
        public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 10;
			Item.useAnimation = 12;
			Item.pick = 86;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 1, 34, 0);
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.DarkBar>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}