using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.GameContent;

namespace FM.Content.Items.Accessories
{
	public class ScarletSoul : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void SetDefaults() 
		{
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 16, 0, 0);
			Item.rare = 10;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.15f;
            player.GetCritChance(DamageClass.Generic) += 6;
			player.statManaMax2 += 60;
            player.statLifeMax2 += 60;
			player.maxMinions += 4;
			player.GetArmorPenetration(DamageClass.Generic) += 6;
			player.lifeRegen += 4;
			player.noKnockback = true;
		}
	}
}