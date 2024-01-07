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
	public class FlaremBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 120;
			Item.shoot = ProjectileType<OmegaBowProj>();
			Item.shootSpeed = 1;
			Item.DamageType = DamageClass.Ranged;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.rare = 10;
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(1, 0, 55, 0);
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/FlaremBow_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position = new Vector2(player.Center.X + (float)Main.rand.Next(-300, 301), player.Center.Y + (float)Main.rand.Next(-50, 51) - 800);
            velocity = FMHelper.PolarVector(Item.shootSpeed, (Main.MouseWorld - position).ToRotation() + (float)Math.PI / 1000 - (float)Math.PI / 1000 * Main.rand.NextFloat());
            Projectile.NewProjectile(source, position, velocity, ProjectileType<OmegaBowProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DaedalusStormbow, 1);
			recipe.AddRecipeGroup("FM:IronBar", 4);
			recipe.AddRecipeGroup("FM:TitaniumBar", 4);
			recipe.AddIngredient(ItemID.FragmentVortex, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 6);
			recipe.AddIngredient(ItemType<Materials.DarkBar>(), 4);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 120);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 8);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}