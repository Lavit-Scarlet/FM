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
    public class DarkHammer : ModItem
    {
        public override void SetDefaults()
		{
			Item.damage = 26;
			Item.DamageType = DamageClass.Melee;
			Item.hammer = 70;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 1, 2, 0);
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.DarkBar>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}