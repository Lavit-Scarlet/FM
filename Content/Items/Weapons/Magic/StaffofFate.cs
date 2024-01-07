using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using FM.Globals;
using FM.Content.Projectiles.Magic;

namespace FM.Content.Items.Weapons.Magic
{
	public class StaffofFate : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 32;
			Item.shoot = ProjectileType<StaffofFateProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 3, 2, 0);
			Item.staff[Item.type] = true;
			Item.rare = 5;
			Item.UseSound = SoundID.Item82;
			Item.autoReuse = false;
			Item.crit = 0;
			Item.mana = 10;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/StaffofFate_Glow").Value;
            }
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for(int i = -1; i<= 1; i++)
			{
				Vector2 perturbedSpeed = new Vector2(-velocity.X, -velocity.Y).RotatedBy(MathHelper.ToRadians(10) * i) * (i != 0 ? 0.9f : 1);
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false; 
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.SlushofFate>(), 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
