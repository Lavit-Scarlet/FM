using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Bows.FlanProjs;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace FM.Content.Items.Weapons.Ranged.Bows.SDMBows
{
	public class FlanBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useAnimation = 16;
			Item.useTime = 4;
			Item.reuseDelay = 20;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 6;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 3, 55, 0);
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/SDMBows/FlanBow_Glow").Value;
            }
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.damage = 60;
				Item.useAnimation = 12;
    			Item.useTime = 12;
     			Item.reuseDelay = 0;
				Item.UseSound = SoundID.Item8;
				Item.shootSpeed = 8;
				Item.shoot = ProjectileType<Flan175AltProj>();

				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				if (modPlayer.shootDelay == 0)
					return true;
				return false;
			}
			else
			{
				Item.damage = 40;
    			Item.useAnimation = 16;
    			Item.useTime = 4;
    			Item.reuseDelay = 20;
				Item.UseSound = SoundID.Item5;
				Item.shootSpeed = 16;
				Item.shoot = ProjectileType<Flan175Proj>();
			}
			return true;
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
			return new Vector2(-8, 0);
		}
		int TimerShoot = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			TimerShoot++;
			if (player.altFunctionUse == 2)
			{
				TimerShoot = 0;
				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				modPlayer.shootDelay = 300;
				int n = 6;
                int deviation = Main.rand.Next(0, 360);
                for (int i = 0; i < n; i++)
                {
                    float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                    Vector2 perturbedSpeed = velocity.RotatedBy(rotation);
                    Projectile.NewProjectile(source, player.Center, perturbedSpeed, ProjectileType<Flan175AltProj>(), damage, 0, player.whoAmI);
                }
				return player.altFunctionUse != 2;
			}
			else
			{
				for (int i = 0; i < 1; i++)
    			{
    				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(4) * 2);
    				Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileType<Flan175Proj>(), damage, knockback, player.whoAmI);
    			}
				if(TimerShoot == 15)
				{
					TimerShoot = 0;
					for (int l = 0; l < 3; l++) 
        			{
						SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot, player.position);
        				float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
        				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 80f;
        				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) 
        				{
        					position += spawnPlace;
        				}

        				Vector2 velocity1 = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
        				int proj = Projectile.NewProjectile(source, position, velocity1, 706, damage * 2, knockback, player.whoAmI);
						Main.projectile[proj].timeLeft = 120;
						Main.projectile[proj].penetrate = 1;
         				for (float num2 = 0.0f; (double)num2 < 40; ++num2) 
        				{
        					int dustIndex = Dust.NewDust(position, 8, 8, 6, 0f, 0f, 0, default(Color), 1f);
							Main.dust[dustIndex].scale = 2;
         					Main.dust[dustIndex].noGravity = true;
        					Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 2.8f;
        				}
        			}
				}
    			return false;
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.SoulofFright, 6);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 60);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}