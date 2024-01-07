using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic;

namespace FM.Content.Items.Weapons.Magic
{
	public class WaterStaff : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 32;
			Item.shoot = ProjectileType<WaterStaffProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.staff[Item.type] = true;
			Item.rare = 4;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = false;
			Item.mana = 20;
			Item.channel = true;
		}
	}
}
