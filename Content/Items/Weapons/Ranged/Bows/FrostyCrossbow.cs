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
using FM.Content.Projectiles.Ranged.Bows;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class FrostyCrossbow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 5;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = 7;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.Item5; 
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-15, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, velocity.ToRotation() + (float)Math.PI/2), velocity, ProjectileType<FrostyCrossbowProj>(), damage, knockback, player.whoAmI);
			return false;
		}
	}
}