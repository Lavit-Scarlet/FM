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
using Terraria.Audio;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class BookofTrueFire : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.shoot = ProjectileType<BookofTrueFireProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 42;
			Item.height = 36;
			Item.useTime = 6;
			Item.useAnimation = 18;
			Item.reuseDelay = 14;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.rare = 5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.mana = 16;
			Item.UseSound = SoundID.Item34;//20
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/BookofTrueFire_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (int j = 0; j < 10; j++) 
			{
				int dust = Dust.NewDust(position, 0, 0, 6);
				Main.dust[dust].velocity = velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(1.5f, 2.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(1.6f, 2f);
			}
			for (int i = 0; i < 1; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(8));
				//perturbedSpeed *= 1f - Main.rand.NextFloat(0.4f);
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false;
        }
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 25f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override bool? UseItem(Player player) 
		{
			if (!Main.dedServ && Item.UseSound.HasValue) 
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Fireblossom, 6);
			recipe.AddIngredient(ItemID.HellstoneBar, 3);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}
	}
}