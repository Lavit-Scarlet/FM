using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Ranged.Ammo;

namespace FM.Content.Items.Ammo
{
    public class RTGrenade : ModItem
    {
		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.rare = 5;
			Item.ammo = Item.type;
		}
	}
}