using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Melee.Spears;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class PrimordialLance : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.DamageType = DamageClass.Melee;
			Item.width = 66;
			Item.height = 66;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 1, 60, 0);
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<PrimordialLanceProj>(); 
            Item.shootSpeed = 4f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override bool CanUseItem(Player player) 
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
    }
}
	
