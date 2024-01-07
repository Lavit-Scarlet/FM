using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class FreezeMark : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 55;
			Item.shoot = ProjectileType<FreezeMarkProj>();
			Item.shootSpeed = 5f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.rare = 8;
			Item.UseSound = SoundID.Item60;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 9, 26, 0);
			Item.mana = 20;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int n = 8;
            int deviation = Main.rand.Next(0, 120);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                Vector2 perturbedSpeed = velocity.RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 5f;
                perturbedSpeed.Y *= 5f;
                Projectile.NewProjectile(source, player.Center, perturbedSpeed, ProjectileType<FreezeMarkProj>(), damage, knockback, player.whoAmI);
            }
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Books.IceStormBook>());
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 6);
			recipe.AddIngredient(ItemID.SoulofMight, 6);
			recipe.AddIngredient(ItemID.SoulofSight, 6);
			recipe.AddIngredient(ItemID.Ectoplasm, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}