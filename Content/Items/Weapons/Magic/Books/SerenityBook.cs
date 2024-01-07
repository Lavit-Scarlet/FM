using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Items.Weapons.Magic.Books;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class SerenityBook : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 92;
			Item.shoot = ProjectileType<SerenityBookProj>();
			Item.shootSpeed = 5;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 42;
			Item.height = 36;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 11;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.mana = 20;
			//Item.UseSound = SoundID.Item105;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/SerenityBook_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(1, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (int I = 0; I < 4; I++) 
			{
				float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 40f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) 
				{
					position += spawnPlace;
				}
				//Vector2 velocity1 = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
			return false;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Books.KillerDoll>());
			recipe.AddIngredient(ItemType<Books.SpectralKiller>());
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 300);
			recipe.AddIngredient(ItemType<Materials.SpaceCatalyst>(), 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}