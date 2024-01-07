using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Melee;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using FM.Globals;
using FM.Content.Projectiles.Melee;


namespace FM.Content.Items.Weapons.Melee
{
	public class PrimordialThrowingAx : ModItem
	{
		public override void SetDefaults()
		{
            // Common Properties
            Item.width = 50;
            Item.height = 50;
            Item.rare = 4;
            Item.value = Item.sellPrice(0, 1, 35, 0);

            // Use Properties
            Item.useStyle = 1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            // Weapon Properties
            Item.damage = 24;
            Item.knockBack = 4;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;

            // Projectile Properties
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<PrimordialThrowingAxProj>();
		}
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
	}
}
