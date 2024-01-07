using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books.ForestBookProjs;
using FM.Globals;
using Terraria.Audio;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class ForestBook : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.shoot = 1;
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 5;
			Item.knockBack = 0;
			Item.rare = 2;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 26, 0);
			Item.mana = 2;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(10));
				perturbedSpeed *= 1f - Main.rand.NextFloat(0.4f);
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<ForestBookMiniProj>(), damage, knockback, player.whoAmI);
				if (Main.rand.NextBool(6))
				{
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<ForestBookProj>(), damage * 2, knockback, player.whoAmI);
				}
			}
			return false;
		}
	}
}