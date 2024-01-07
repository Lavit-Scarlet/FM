using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using FM.Content.Projectiles;
using FM.Globals;

namespace FM.Content.Items.Weapons
{
	public class TestItem : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 100000;
			Item.shoot = 14;
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.staff[Item.type] = true;
			Item.rare = 0;
			Item.autoReuse = true;
		}
	}
}
