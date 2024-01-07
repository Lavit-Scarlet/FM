using FM.Content.Projectiles.Melee.Spears.HeavenlySpearProjs;
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

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class HeavenlySpear : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 10;
			Item.value = Item.sellPrice(0, 25, 50, 0);

			// Use Properties
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useAnimation = 30;
			Item.useTime = 30;
			//Item.UseSound = SoundID.Item60;
			Item.autoReuse = true; 

			// Weapon Properties
			Item.damage = 90;
			Item.knockBack = 5f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<HeavenlySpear_Slash>();
		}
        private bool XZ = false;
        private int AttackType = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(AttackType < 2)
            {
                Projectile.NewProjectileDirect(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 0f, XZ ? 1f : -1f)
                    .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                AttackType += 1;
            }
            else if(AttackType == 2)
            {
				float rot = (velocity).ToRotation();
				Projectile.NewProjectile(source, position + FMHelper.PolarVector(100, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<HeavenlySpearLaser>(), damage * 5, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<HeavenlySpearProj>(), 74, knockback, player.whoAmI, 0f, XZ ? 1f : -1f)
                    .ai[1] = XZ ? 1f : -1f;
                XZ = !XZ;
                AttackType = 0;
            }
            return false;
        }
	}
}
