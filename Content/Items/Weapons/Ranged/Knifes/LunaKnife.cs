using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Knifes;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class LunaKnife : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.shoot = ProjectileType<LunaKnifeProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.rare = 5;
            Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 3, 10, 0);
			Item.noUseGraphic = true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(6, rot) + FMHelper.PolarVector(12f, rot + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(32, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(6, rot) + FMHelper.PolarVector(-12f, rot + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI);
			return false;
		}
	}
}
