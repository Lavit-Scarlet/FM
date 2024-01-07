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
using FM.Content.Projectiles.Melee.Swords.BloodSwordProjs;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class BloodSword : ModItem
	{
		public override void SetDefaults()
		{
                  // Common Properties
                  Item.width = 56;
                  Item.height = 56;
                  Item.rare = 6;
                  Item.value = Item.sellPrice(0, 4, 24, 0);

                  // Use Properties
                  Item.useStyle = 1;
                  Item.useAnimation = 24;
                  Item.useTime = 24;
                  Item.UseSound = SoundID.Item1;
                  Item.autoReuse = true;

                  // Weapon Properties
                  Item.damage = 60;
                  Item.knockBack = 4;
                  //Item.noUseGraphic = true;
                  Item.DamageType = DamageClass.Melee;
                  Item.noMelee = true;

                  // Projectile Properties
                  Item.shootSpeed = 15f;
                  Item.shoot = 1;
		}

            public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
            {
                  float rot = (velocity).ToRotation();
                  Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ModContent.ProjectileType<BloodSwordShootProj>(), damage, knockback, player.whoAmI);

                  float adjustedItemScale = player.GetAdjustedItemScale(Item);
			Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ModContent.ProjectileType<BloodSwordSwinging>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
			NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                  return false;
            }
            public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 15);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 40);
			recipe.AddIngredient(ItemID.SoulofNight, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
                          

	}

}