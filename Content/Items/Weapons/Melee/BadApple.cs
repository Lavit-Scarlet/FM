using FM.Content.Projectiles.Melee.BadAppleProjs;
using Terraria.Audio;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee
{
	public class BadApple : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 10;
			Item.value = Item.sellPrice(0, 15, 50, 0);

			// Use Properties
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useAnimation = 18;
			Item.useTime = 18;
			//Item.UseSound = SoundID.Item60;
			Item.autoReuse = true; 

			// Weapon Properties
			Item.damage = 44;
			Item.knockBack = 2f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<BadAppleSlash>();
		}
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<BadAppleProj>()] < 1;
        }
		public override float UseSpeedMultiplier(Player player)
        {
            float Mult = base.UseSpeedMultiplier(player);
            if (AttackType == 2) Mult *= .5f;
            if (AttackType == 3) Mult *= .5f;
            return Mult;
        }
        private bool XZ = false;
        private int AttackType = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(AttackType < 2)
            {
                Item.useStyle = ItemUseStyleID.Rapier;
                Projectile.NewProjectileDirect(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 0f, XZ ? 1f : -1f) .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                AttackType += 1;
            }
            else if(AttackType < 4)
            {
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<BadAppleBigSlash>(), damage * 2, knockback, player.whoAmI, 0f, XZ ? 1f : 0f) .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                AttackType += 1;
            }
            else if(AttackType == 4)
            {
                Item.useStyle = 1;
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<BadAppleProj>(), damage, knockback, player.whoAmI, 0f, XZ ? 1f : 0f) .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                AttackType = 0;
            }
            return false;
        }
	}
}
