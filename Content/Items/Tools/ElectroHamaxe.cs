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
    public class ElectroHamaxe : ModItem
    {
        public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.axe = 28;
			Item.hammer = 90;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 20;
			Item.useAnimation = 22;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = Item.sellPrice(0, 4, 34, 0);
			Item.rare = 6;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Tools/ElectroHamaxe_Glow").Value;
            }
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("FM:TitaniumBar", 6);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}