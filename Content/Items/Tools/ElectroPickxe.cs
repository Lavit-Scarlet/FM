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

namespace FM.Content.Items.Tools
{
    public class ElectroPickxe : ModItem
    {
        public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 10;
			Item.useAnimation = 12;
			Item.pick = 200;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 5, 34, 0);
			Item.rare = 6;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Tools/ElectroPickxe_Glow").Value;
            }
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("FM:TitaniumBar", 4);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}