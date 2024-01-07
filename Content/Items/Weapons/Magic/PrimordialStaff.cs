using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles;
using FM.Content.Projectiles.Magic.PrimordialStaffProjs;

namespace FM.Content.Items.Weapons.Magic
{
	public class PrimordialStaff : ModItem
	{
        public const int ChargeFrames = 60;
        public const int CooldownFrames = 60;
        public const float GemDistance = 25f;
		public override void SetDefaults() 
		{
			Item.damage = 24;
			Item.shoot = ProjectileType<PrimordialStaffProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
            Item.useTime = ChargeFrames + CooldownFrames;
            Item.useAnimation = ChargeFrames + CooldownFrames;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 1, 12, 0);
			Item.staff[Item.type] = true;
			Item.rare = 4;
			//Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
            Item.noUseGraphic = true;
			Item.channel = true;
			Item.crit = 4;
			Item.mana = 20;
		}
	}

}
