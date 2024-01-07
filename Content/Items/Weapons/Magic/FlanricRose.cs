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
using FM.Content.Projectiles.Magic.FlanricRoseProjs;

namespace FM.Content.Items.Weapons.Magic
{
	public class FlanricRose : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.shoot = ProjectileType<FlanricRoseProj>();
			Item.shootSpeed = 20f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 3, 80, 0);
			Item.rare = 5;
			Item.autoReuse = true;
			Item.mana = 20;
			Item.staff[Item.type] = true;
			Item.UseSound = SoundID.Item109;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(12));
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), perturbedSpeed, type, damage, knockback, player.whoAmI);
			return false;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 50f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 80);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}