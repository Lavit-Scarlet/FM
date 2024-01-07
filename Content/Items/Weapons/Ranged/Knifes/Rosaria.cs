using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Knifes.RosariaProjs;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class Rosaria : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 56;
			Item.shoot = ProjectileType<RosariaProj>();
			Item.shootSpeed = 14;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.rare = 7;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 4, 10, 0);
			Item.noUseGraphic = true;
		}
	}
}
