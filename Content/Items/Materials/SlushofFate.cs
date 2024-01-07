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
	public class SlushofFate : ModItem
	{
		public override void SetDefaults() 
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(0, 0, 32, 0);
			Item.rare = 5;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Materials/SlushofFate_Glow").Value;
            }
		}
	}
}