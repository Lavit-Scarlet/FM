using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.StaffofColdBloodProjs;

namespace FM.Content.Items.Weapons.Magic
{
	public class StaffofColdBlood : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 240;
			Item.shoot = ProjectileType<StaffofColdBloodProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = 5;
			Item.knockBack = 8;
			Item.value = Item.sellPrice(1, 0, 0, 0);
			Item.staff[Item.type] = true;
			Item.rare = 11;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.mana = 40;
		}
	}
}
