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
using FM.Content.Projectiles.Ranged.Bows.ArasBowProjs;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class ArosBow : ModItem
	{
		public override void SetDefaults()
		{
		    Item.damage = 47;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 4, 20, 0);
			Item.rare = 6;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.crit += 11;
			Item.UseSound = SoundID.Item5;
		}
		public override Vector2? HoldoutOffset()
		{
			return Vector2.Zero;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.damage = 47;
				Item.useAnimation = 30;
    			Item.useTime = 30;
				Item.UseSound = SoundID.Item5;
				Item.shootSpeed = 10;
				Item.shoot = ProjectileType<ArosAltProj>();

				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				if (modPlayer.shootDelay == 0)
					return true;
				return false;
			}
			else
			{
				Item.damage = 47;
    			Item.useAnimation = 30;
    			Item.useTime = 30;
				Item.UseSound = SoundID.Item5;
				Item.shootSpeed = 10;
				Item.shoot = 1;
			}
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				modPlayer.shootDelay = 120;
				Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, velocity.ToRotation() + (float)Math.PI/2), velocity * 2, ProjectileType<ArosAltProj>(), damage * 4, knockback, player.whoAmI);
				return player.altFunctionUse != 2;
			}
			else
			{
				int TypeProj = Main.rand.Next(2);
				switch(TypeProj)
    			{
    				case 0:
    					TypeProj = ProjectileType<ArosArrowMinProj>();
    					break;
     				case 1:
    					TypeProj = ProjectileType<ArosArrowProj>();
    					break;
    				default:
        				break;
    			}
				Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, velocity.ToRotation() + (float)Math.PI/2), velocity, TypeProj, damage, knockback, player.whoAmI);
				return false;
			}
		}
	}
}