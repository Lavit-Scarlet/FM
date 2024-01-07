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
using FM.Content.Projectiles.Melee.Swords.PrimordialSwordProjs;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class PrimordialSword : ModItem
	{
		public override void SetDefaults()
		{
                  // Common Properties
                  Item.width = 56;
                  Item.height = 56;
                  Item.rare = 4;
                  Item.value = Item.sellPrice(0, 1, 64, 0);

                  // Use Properties
                  Item.useStyle = 1;
                  Item.useAnimation = 24;
                  Item.useTime = 24;
                  Item.UseSound = SoundID.Item1;
                  Item.autoReuse = true;

                  // Weapon Properties
                  Item.damage = 24;
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
                  float adjustedItemScale = player.GetAdjustedItemScale(Item);
			Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ModContent.ProjectileType<PrimordialSwordSwinging>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
			NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                  return false;
            }
	}
}