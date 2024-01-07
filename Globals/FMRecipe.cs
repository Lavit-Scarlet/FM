using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
//using FM.Content.Items.Materials;

namespace FM.Globals
{
    public class FMRecipe : ModSystem
    {
        public override void AddRecipeGroups()
		{
            //Platinum Bar
            RecipeGroup.RegisterGroup("FM:PlatinumBar", new RecipeGroup(() => Lang.misc[37] + Lang.GetItemNameValue(ItemID.PlatinumBar), new int[]
            {
                ItemID.PlatinumBar,
                ItemID.GoldBar

            }));

            //Silver Bars
            RecipeGroup.RegisterGroup("FM:SilverBar", new RecipeGroup(() => Lang.misc[37] + Lang.GetItemNameValue(ItemID.SilverBar), new int[]
            {
                ItemID.SilverBar,
                ItemID.TungstenBar
            }));

            //Titanium Bars
            RecipeGroup.RegisterGroup("FM:TitaniumBar", new RecipeGroup(() => Lang.misc[37] + Lang.GetItemNameValue(ItemID.TitaniumBar), new int[]
            {
                ItemID.TitaniumBar,
                ItemID.AdamantiteBar
            }));

            //Evil Bars
            RecipeGroup.RegisterGroup("FM:EvilBar", new RecipeGroup(() => Lang.misc[37] + Lang.GetItemNameValue(ItemID.DemoniteBar), new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            }));

            //Iron Bar
            RecipeGroup.RegisterGroup("FM:IronBar", new RecipeGroup(() => Lang.misc[37] + Lang.GetItemNameValue(ItemID.IronBar), new int[]
            {
                ItemID.IronBar,
                ItemID.LeadBar
            }));

            //Cobalt Bar
            RecipeGroup.RegisterGroup("FM:CobaltBar", new RecipeGroup(() => Lang.misc[37] + Lang.GetItemNameValue(ItemID.CobaltBar), new int[]
            {
                ItemID.CobaltBar,
                ItemID.PalladiumBar
            }));

            //Mythril Bar
            RecipeGroup.RegisterGroup("FM:MythrilBar", new RecipeGroup(() => Lang.misc[37] + Lang.GetItemNameValue(ItemID.MythrilBar), new int[]
            {
                ItemID.MythrilBar,
                ItemID.OrichalcumBar
            }));

            //Shadow Scale
            RecipeGroup.RegisterGroup("FM:ShadowScale", new RecipeGroup(() => Lang.misc[37] + Lang.GetItemNameValue(ItemID.ShadowScale), new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            }));

            //Copper Bar
            RecipeGroup.RegisterGroup("FM:CopperBar", new RecipeGroup(() => Lang.misc[37] + Lang.GetItemNameValue(ItemID.CopperBar), new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            }));
		}
    }
}