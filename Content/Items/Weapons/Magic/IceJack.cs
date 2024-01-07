using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic;
using Terraria.Audio;
using System.Collections.Generic;

namespace FM.Content.Items.Weapons.Magic
{
	public class IceJack : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 60;
			Item.shoot = ProjectileType<IceJackProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 4, 10, 0);
			Item.staff[Item.type] = true;
			Item.rare = 5;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.mana = 10;
		}
		public override bool? UseItem(Player player) 
		{
			if (!Main.dedServ && Item.UseSound.HasValue) 
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}
		public void LimitPointToPlayerReachableArea(ref Vector2 pointPoisition)
		{
			Player player = Main.player[Item.playerIndexTheItemIsReservedFor];
			
			Vector2 center = player.Center;
			Vector2 vector = pointPoisition - center;
			float num = Math.Abs(vector.X);
			float num2 = Math.Abs(vector.Y);
			float num3 = 1f;
			if (num > 960f)
			{
				float num4 = 960f / num;
				if (num3 > num4)
				{
					num3 = num4;
				}
			}
			if (num2 > 600f)
			{
				float num5 = 600f / num2;
				if (num3 > num5)
				{
					num3 = num5;
				}
			}
			Vector2 vector2 = vector * num3;
			pointPoisition = center + vector2;
		}
	
		private Point FindSharpTearsSpot(Vector2 targetSpot)
		{
			Player player = Main.player[Item.playerIndexTheItemIsReservedFor];
			
			Point point = targetSpot.ToTileCoordinates();
			Vector2 center = player.Center;
			Vector2 endPoint = targetSpot;
			int samplesToTake = 3;
			float samplingWidth = 4f;
			Collision.AimingLaserScan(center, endPoint, samplingWidth, samplesToTake, out var vectorTowardsTarget, out var samples);
			float num = float.PositiveInfinity;
			for (int i = 0; i < samples.Length; i++)
			{
				if (samples[i] < num)
				{
					num = samples[i];
				}
			}
			targetSpot = center + vectorTowardsTarget.SafeNormalize(Vector2.Zero) * num;
			point = targetSpot.ToTileCoordinates();
			Rectangle value = new Rectangle(point.X, point.Y, 1, 1);
			value.Inflate(18, 32);
			Rectangle value2 = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
			value2.Inflate(-100, -100);
			value = Rectangle.Intersect(value, value2);
			List<Point> list = new List<Point>();
			List<Point> list2 = new List<Point>();
			for (int j = value.Left; j <= value.Right; j++)
			{
				for (int k = value.Top; k <= value.Bottom; k++)
				{
					if (!WorldGen.SolidTile(j, k))
					{
						continue;
					}
					Vector2 value3 = new Vector2(j * 16 + 8, k * 16 + 8);
					if (!(Vector2.Distance(targetSpot, value3) > 4600f))
					{
						if (FindSharpTearsOpening(j, k, j > point.X, j < point.X, k > point.Y, k < point.Y))
						{
							list.Add(new Point(j, k));
						}
						else
						{
							list2.Add(new Point(j, k));
						}
					}
				}
			}
			if (list.Count == 0 && list2.Count == 0)
			{
				list.Add((player.Center.ToTileCoordinates().ToVector2() + Main.rand.NextVector2Square(-2f, 2f)).ToPoint());
			}
			List<Point> list3 = list;
			if (list3.Count == 0)
			{
				list3 = list2;
			}
			int index = Main.rand.Next(list3.Count);
			return list3[index];
		}

		private bool FindSharpTearsOpening(int x, int y, bool acceptLeft, bool acceptRight, bool acceptUp, bool acceptDown)
		{
			if (acceptLeft && !WorldGen.SolidTile(x - 1, y))
			{
				return true;
			}
			if (acceptRight && !WorldGen.SolidTile(x + 1, y))
			{
				return true;
			}
			if (acceptUp && !WorldGen.SolidTile(x, y - 1))
			{
				return true;
			}
			if (acceptDown && !WorldGen.SolidTile(x, y + 1))
			{
				return true;
			}
			return false;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 12; i++)
			{
				Vector2 pointPoisition2 = Main.MouseWorld;
				LimitPointToPlayerReachableArea(ref pointPoisition2);
				Vector2 vector24 = pointPoisition2 + Main.rand.NextVector2Circular(8f, 8f);
				Vector2 vector25 = FindSharpTearsSpot(vector24).ToWorldCoordinates(Main.rand.Next(14), Main.rand.Next(14));
				Vector2 vector26 = (vector24 - vector25).SafeNormalize(-Vector2.UnitY) * 16f;
				var stalagmite = Projectile.NewProjectile(source, vector25.X, vector25.Y, vector26.X, vector26.Y, type, Item.damage, Item.knockBack * 0.75f, player.whoAmI, 0f, Main.rand.NextFloat() * 0.5f + 0.5f);	
				Main.projectile[stalagmite].originalDamage = Item.damage;
			}
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IceBlock, 100);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
