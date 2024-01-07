using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books.KillerDollProjs;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class KillerDoll : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 44;
			Item.shoot = ProjectileType<KillerDollProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 42;
			Item.height = 36;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.rare = 5;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 3, 35, 0);
			Item.mana = 5;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/KillerDoll_Glow").Value;
            }
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (int I = 0; I < 1; I++) 
			{
				float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 90f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) 
				{
					position += spawnPlace;
				}
				Vector2 velocity1 = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
				Projectile.NewProjectile(source, position, velocity1, type, damage, knockback, player.whoAmI);
			}
			return false;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 40);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}
	}
}