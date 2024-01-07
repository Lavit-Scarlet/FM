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
	public class ShadowofOnyx : ModItem
	{
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.damage = 58;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 80;
			Item.height = 24;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 16f;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 14, 50, 0);
			Item.rare = 10;
			Item.autoReuse = true;
			Item.useAnimation = 16;
			Item.useTime = 16;
			//Item.UseSound = SoundID.Item36;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, 1);
		}
		public override bool CanConsumeAmmo(Item ammo, Player player) 
		{
			return Main.rand.NextFloat() >= 0.44f;
		}
		private int AttackType = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AttackType += 1;
			int numberProjectiles = 4;
			for (int i = 0; i < numberProjectiles; i++)
			{
				FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/Shotgun3"), (int)player.Center.X, (int)player.Center.Y);
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(12));
				perturbedSpeed *= 1f - Main.rand.NextFloat(0.4f);
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			if (AttackType == 3)
            {
				FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/Shotgun2"), (int)player.Center.X, (int)player.Center.Y, .50f);
                float numberProjectiles1 = 3;
    			float rotation = MathHelper.ToRadians(3);
    			for (int i = 0; i < numberProjectiles1; i++)
        		{
     				Vector2 perturbedSpeed1 = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles1 - 1))) / 1.4f;
    				int proj = Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed1.X, perturbedSpeed1.Y, 661, damage * 2, knockback, player.whoAmI);
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].timeLeft = 120;
    			}
				AttackType = 0;
			}
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.OnyxBlaster, 1);
			recipe.AddIngredient(ItemID.DarkShard, 2);
			recipe.AddIngredient(ItemID.SoulofNight, 18);
			recipe.AddIngredient(ItemID.FragmentVortex, 16);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
