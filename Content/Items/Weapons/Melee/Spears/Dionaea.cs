using FM.Content.Projectiles.Melee.Spears.DionaeaProjs;
using Terraria.Audio;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Spears
{
	public class Dionaea : ModItem
	{
		public override void SetDefaults() 
		{
			// Common Properties
			Item.rare = 7;
			Item.value = Item.sellPrice(0, 7, 50, 0);

			// Use Properties
			Item.useStyle = 5;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true; 

			// Weapon Properties
			Item.damage = 72;
			Item.knockBack = 6f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;

			// Projectile Properties
			Item.shootSpeed = 14f;
			Item.shoot = ModContent.ProjectileType<DionaeaProj>();
		}

		public override bool CanUseItem(Player player) 
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player) 
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<DionaeaProj>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(60, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<DionaeaShootProj>(), damage, knockback, player.whoAmI);
			return false;
		}
	}
}
