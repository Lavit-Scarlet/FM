using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using FM.Content.Projectiles.Ranged.Bows;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class BowofFate : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 3, 12, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/BowofFate_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(2, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<BowofFateProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.SlushofFate>(), 24);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}