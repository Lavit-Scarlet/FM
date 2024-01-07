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
using FM.Content.Projectiles.Ranged.Bows.ElegantBowProjs;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class ElegantBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 38;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 5;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(0, 4, 80, 0);
			Item.rare = 5;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.Item5;
		}
		public override Vector2? HoldoutOffset()
		{
			return Vector2.Zero;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int arrowType = Main.rand.Next(7);
			switch(arrowType)
			{
				case 0:
					arrowType = ProjectileType<ElegantBowBlueProj>();
					break;
				case 1:
					arrowType = ProjectileType<ElegantBowGreenProj>();
					break;
				case 2:
					arrowType = ProjectileType<ElegantBowLightBlueProj>();
					break;
				case 3:
					arrowType = ProjectileType<ElegantBowPinkProj>();
					break;
				case 4:
					arrowType = ProjectileType<ElegantBowRedProj>();
					break;
				case 5:
					arrowType = ProjectileType<ElegantBowWhiteProj>();
					break;
				case 6:
					arrowType = ProjectileType<ElegantBowYellowProj>();
					break;
				default:
     				break;
			}
			float rot = (velocity).ToRotation();
    		Projectile.NewProjectile(source, position + FMHelper.PolarVector(Main.rand.NextFloat(0f, 0f), rot) + FMHelper.PolarVector(Main.rand.NextFloat(-10f, 10f), rot + (float)Math.PI/2), velocity, arrowType, damage, knockback, player.whoAmI);
			return false;
		}
	}
}