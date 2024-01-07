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
using FM.Content.Projectiles.Magic;

namespace FM.Content.Items.Weapons.Magic
{
	public class IceLaserRifle : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.shoot = ProjectileType<IceLaserRifleProj>();
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 2, 80, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.mana = 4;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 1);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(1));
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), perturbedSpeed, type, damage, knockback, player.whoAmI);
			return false;
		}
	}
}