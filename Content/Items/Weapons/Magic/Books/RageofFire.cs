using Terraria;
using System;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class RageofFire : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.shoot = ProjectileType<RageofFireProj>();
			Item.shootSpeed = 12f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 4;
			Item.useAnimation = 8;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.rare = 5;
			Item.UseSound = SoundID.Item34;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 3, 76, 0);
			Item.mana = 5;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/RageofFire_Glow").Value;
            }
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(15));
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), perturbedSpeed, type, damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 2);
			recipe.AddIngredient(ItemID.Fireblossom, 4);
			recipe.AddIngredient(ItemID.SoulofNight, 2);
			recipe.AddIngredient(ItemID.SoulofLight, 2);
			recipe.AddIngredient(ItemID.SoulofSight, 4);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}
	}
}