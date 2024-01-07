using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.Audio;
using FM.Content.Projectiles.Ranged.Guns;

namespace FM.Content.Items.Weapons.Ranged.Guns
{
	public class FrostPistol : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.useAmmo = AmmoID.Bullet;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useAnimation = 22;
			Item.useTime = 10;
			Item.reuseDelay = 24;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 1, 14, 0);
			Item.rare = 3;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
		}
		public override bool? UseItem(Player player) 
		{
			if (!Main.dedServ && Item.UseSound.HasValue) 
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0);
		}
		public override bool CanConsumeAmmo(Item ammo, Player player) 
		{
			return Main.rand.NextFloat() >= 0.12f;
		}
	    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(4));

				Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileType<FrostPistolProj>(), damage, knockback, player.whoAmI);
			}
			return false;
        }
	}
}