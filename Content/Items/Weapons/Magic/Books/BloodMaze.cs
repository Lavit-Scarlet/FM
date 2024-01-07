using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books.BloodMazeProjs;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class BloodMaze : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 144;
			Item.shoot = ProjectileType<BloodMazeProj>();
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 11;
			Item.useAnimation = 11;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.rare = 10;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 9, 26, 0);
			Item.mana = 10;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/BloodMaze_Glow").Value;
            }
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(Main.rand.NextFloat(-14, 14), rot + (float)Math.PI/2), velocity, ProjectileType<BloodMazeProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 40f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ItemID.SoulofFright, 10);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 120);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 8);
			recipe.AddIngredient(ItemID.Ectoplasm, 8);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}