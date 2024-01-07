using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;

namespace FM.Content.Items.Materials
{
	public class PureSoulofPower : ModItem
	{
		public override void SetStaticDefaults() 
		{
			ItemID.Sets.AnimatesAsSoul[Item.type] = false;

			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void SetDefaults() 
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(0, 7, 0, 0);
			Item.rare = 10;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Materials/PureSoulofPower_Glow").Value;
            }
		}


	}
}