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
using FM.Content.Projectiles.Melee.Swords.ReaperProjs;
using FM.Content.Items.Weapons.Melee.Swords;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class Reaper : ModItem
	{
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.shoot = ProjectileType<ReaperWaveProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 12, 6, 0);
			Item.rare = 9;
			//Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
		}
		private bool XZ = false;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
            Vector2 velocity1 = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(-60, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity1, ProjectileType<ReaperWaveProj>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<ReaperSlash>(), damage, knockback, player.whoAmI, 0f, XZ ? 1f : -1f) .ai[1] = XZ ? 1f : -1f;
            XZ = !XZ;
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Spectralibur>(), 1);
			recipe.AddIngredient(ItemID.TheHorsemansBlade, 1);
			recipe.AddIngredient(ItemID.SpookyWood, 12);
			recipe.AddIngredient(ItemID.FragmentSolar, 10);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 30);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}