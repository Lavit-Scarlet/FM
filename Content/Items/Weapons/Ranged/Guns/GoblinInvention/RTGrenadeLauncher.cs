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
using FM.Globals;
using Terraria.Audio;
using FM.Content.Items.Ammo;
using FM.Content.Projectiles.Ranged.Ammo;

namespace FM.Content.Items.Weapons.Ranged.Guns.GoblinInvention
{
	public class RTGrenadeLauncher : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.shoot = ProjectileType<RTGrenadeProj>();
			Item.shootSpeed = 10;
			Item.useAmmo = ItemType<RTGrenade>();
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.useStyle = 5;
			Item.knockBack = 8;
			Item.value = Item.sellPrice(0, 4, 80, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item61;
			Item.autoReuse = true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 2);
		}
	    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(3));

				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false;
        }
	}
}