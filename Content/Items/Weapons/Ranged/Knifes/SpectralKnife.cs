using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Knifes.SpectralKnifeProjs;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class SpectralKnife : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 26;
			Item.shoot = ProjectileType<SpectralKnifeTestProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 3, 20, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.crit += 12;
			Item.noUseGraphic = true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(6));
			Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<SpectralKnifeTestProj>(), damage, knockback, player.whoAmI);
			return false;
		}
	}
}
