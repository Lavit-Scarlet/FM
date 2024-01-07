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

namespace FM.Content.Items.Weapons.Ranged.Guns.GoblinInvention
{
	public class RTAuto : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.useAmmo = AmmoID.Bullet;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 1, 80, 0);
			Item.rare = 3;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/Thompson");
			Item.autoReuse = true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-13, 0);
		}
	    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(1));

				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false;
        }
	}
}