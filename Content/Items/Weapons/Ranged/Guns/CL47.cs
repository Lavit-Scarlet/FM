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

namespace FM.Content.Items.Weapons.Ranged.Guns
{
	public class CL47 : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 4;
			Item.shoot = 1;
			Item.shootSpeed = 8;
			Item.useAmmo = AmmoID.Bullet;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useAnimation = 18;
			Item.useTime = 4;
			Item.reuseDelay = 22;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 0, 14, 0);
			Item.rare = 1;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/CL47") 
			{
				Volume = 0.6f,
				PitchVariance = 0.0f,
				MaxInstances = 1,
			};
			Item.autoReuse = true;
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
			return new Vector2(-1, 0);
		}
		public override bool CanConsumeAmmo(Item ammo, Player player) 
		{
			return Main.rand.NextFloat() >= 0.25f;
		}
	    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(24));

				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("FM:PlatinumBar", 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}