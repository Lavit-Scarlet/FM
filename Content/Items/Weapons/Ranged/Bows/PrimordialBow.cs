using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class PrimordialBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 4;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 1, 35, 0);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(1, 0);
		}
	}
}