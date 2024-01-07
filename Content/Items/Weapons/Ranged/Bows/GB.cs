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
using FM.Content.Projectiles.Ranged.Bows.GalaxyBowProjs;
using FM.Content.Items.Weapons.Ranged.Bows;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class GB : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 180;
			Item.shoot = ProjectileType<GalaxyBowProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 5;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.rare = 11;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 22, 50, 0);
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/GB_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position = new Vector2(player.Center.X + (float)Main.rand.Next(-300, 301), player.Center.Y + (float)Main.rand.Next(-50, 51) - 800);
            velocity = FMHelper.PolarVector(Item.shootSpeed, (Main.MouseWorld - position).ToRotation() + (float)Math.PI / 1000 - (float)Math.PI / 1000 * Main.rand.NextFloat());
            Projectile.NewProjectile(source, position, velocity, ProjectileType<GalaxyBowProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<HeavenlyBow>(), 1);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 4);
			recipe.AddIngredient(ItemType<Materials.SpaceCatalyst>(), 3);
			recipe.AddIngredient(ItemType<Materials.PureSoulofPower>(), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}