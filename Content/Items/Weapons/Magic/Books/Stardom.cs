using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books.StardomProjs;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class Stardom : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.shoot = ProjectileType<StardomProj>();
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 5;
			Item.UseSound = SoundID.NPCDeath7;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 3, 26, 0);
			Item.mana = 10;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(10));
				perturbedSpeed *= 1f - Main.rand.NextFloat(0.4f);
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}