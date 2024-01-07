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
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Items.Weapons.Ranged.Guns
{
	public class SoulShotgun : ModItem
	{
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.damage = 32;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 80;
			Item.height = 24;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 10f;
			Item.useStyle = 5;
			Item.knockBack = 8;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.rare = 6;
			Item.autoReuse = true;
			Item.useAnimation = 60;
			Item.useTime = 60;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/FireShotgun") 
			{
				Volume = 0.5f
			};
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Guns/SoulShotgun_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-15, 0);
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.damage = 32;
				//Item.shoot = 1;
				Item.shootSpeed = 10;
				
				Item.useStyle = 5;
				Item.useTime = 10;
				Item.useAnimation = 60;
				Item.autoReuse = false;
				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				if (modPlayer.shootDelay == 0)
					return true;
				return false;
			}
			else
			{
				Item.damage = 32;
				//Item.shoot = 1;
				Item.shootSpeed = 10;
				
				Item.useStyle = 5;
				Item.useTime = 60;
				Item.useAnimation = 60;
				Item.autoReuse = true;
			}
			return true;
		}
		int timer = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				for (int j = 0; j < 10; j++) 
				{
					ParticleManager.NewParticle<LineStreak_Long_ShortTime>(position, velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(.5f, 1f), new Color(255, 50, 50), Main.rand.NextFloat(0.5f, 1f), 0, 0);
					ParticleManager.NewParticle<LineStreak_Long_ShortTime>(position, velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(.5f, 1f), new Color(255, 100, 100), Main.rand.NextFloat(1f, 2f), 0, 0);
					ParticleManager.NewParticle<LineStreak_Fire>(position, velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(0.2f, .66f), new Color(255, 0, 0), Main.rand.NextFloat(4f, 5f), 0, 0);

					int dust = Dust.NewDust(position, 0, 0, 182);
					Main.dust[dust].velocity = velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(1.5f, 2.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(1f, 2f);
				}
				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				modPlayer.shootDelay = 600;
				modPlayer.Shake += 10;
				FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/FireShotgun"), (int)player.Center.X, (int)player.Center.Y, .5f);
				int numberProjectiles2 = 4;
		    	for (int i = 0; i < numberProjectiles2; i++)
		    	{
		    		Vector2 perturbedSpeed1 = velocity.RotatedByRandom(MathHelper.ToRadians(10));
		    		perturbedSpeed1 *= 1f - Main.rand.NextFloat(0.3f);
		    		Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed1.X, perturbedSpeed1.Y, ProjectileType<SoulShotgunRocketProj2>(), damage * 2, knockback, player.whoAmI);
		     	}
		    	float dir = velocity.ToRotation();
		    	player.velocity = FMHelper.PolarVector(5f, dir + (float)Math.PI);

				return player.altFunctionUse != 2;
			}
			else
			{
				for (int j = 0; j < 10; j++) 
				{
					ParticleManager.NewParticle<LineStreak_Long_ShortTime>(position, velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(.5f, 1f), new Color(255, 50, 50), Main.rand.NextFloat(0.5f, 1f), 0, 0);
					ParticleManager.NewParticle<LineStreak_Long_ShortTime>(position, velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(.5f, 1f), new Color(255, 100, 100), Main.rand.NextFloat(1f, 2f), 0, 0);
					ParticleManager.NewParticle<LineStreak_Fire>(position, velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(0.2f, .66f), new Color(255, 0, 0), Main.rand.NextFloat(4f, 5f), 0, 0);

					int dust = Dust.NewDust(position, 0, 0, 182);
					Main.dust[dust].velocity = velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(1.5f, 2.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(1f, 2f);
				}
				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				modPlayer.Shake += 8;
				const int NumProjectiles = 8;
		    	for (int i = 0; i < NumProjectiles; i++) 
		    	{
		    		Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(14));
		    		newVelocity *= 1f - Main.rand.NextFloat(0.3f);
		    		Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
		    	}
		    	int numberProjectiles2 = 3;
		    	for (int i = 0; i < numberProjectiles2; i++)
		    	{
		    		Vector2 perturbedSpeed1 = velocity.RotatedByRandom(MathHelper.ToRadians(8));
		    		perturbedSpeed1 *= 1f - Main.rand.NextFloat(0.3f);
		    		Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed1.X, perturbedSpeed1.Y, ProjectileType<SoulShotgunRocketProj2>(), damage * 2, knockback, player.whoAmI);
		     	}
		    	float dir = velocity.ToRotation();
		    	player.velocity = FMHelper.PolarVector(4f, dir + (float)Math.PI);
		     	return false;
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Shotgun, 1);
			recipe.AddRecipeGroup("FM:TitaniumBar", 4);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 8);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 30);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
