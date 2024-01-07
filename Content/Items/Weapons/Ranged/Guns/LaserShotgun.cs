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
using FM.Content.Projectiles.Ranged.Guns;

namespace FM.Content.Items.Weapons.Ranged.Guns
{
	public class LaserShotgun : ModItem
	{
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.damage = 45;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 80;
			Item.height = 24;
			Item.shoot = ProjectileType<LaserShotgunProj>();
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/Railgun") 
			{
				Volume = 1f,
				Pitch = 0.40f,
				MaxInstances = 1,
			};
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 5f;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 6, 50, 0);
			Item.rare = 7;
			Item.autoReuse = true;
			Item.useAnimation = 32;
			Item.useTime = 32;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Guns/LaserShotgun_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, 4);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int numberProjectiles = 5;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(12));

				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<LaserShotgunProj>(), damage, knockback, player.whoAmI);
			}
			return false;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 52f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ChlorophyteBar, 2);
			recipe.AddRecipeGroup("FM:TitaniumBar", 4);
			recipe.AddIngredient(ItemID.SoulofNight, 6);
			recipe.AddIngredient(ItemID.SoulofLight, 6);
			recipe.AddIngredient(ItemType<Materials.SlushofFate>(), 2);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
