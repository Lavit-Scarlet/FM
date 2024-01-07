using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Melee.Swords;
using System;
using ReLogic.Content;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class FrostSword : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.shoot = ProjectileType<FrostSwordProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(6));
				perturbedSpeed *= 1f - Main.rand.NextFloat(0.4f);

				Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileType<FrostSwordProj>(), damage / 2, knockback / 2, player.whoAmI);
			}
			return false;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IceBlock, 28);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
