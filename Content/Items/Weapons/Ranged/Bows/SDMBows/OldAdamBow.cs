using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Bows;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;

namespace FM.Content.Items.Weapons.Ranged.Bows.SDMBows
{
	public class OldAdamBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.shoot = ProjectileType<AdamBowProj>();
			Item.shootSpeed = 10;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useAnimation = 14;
			Item.useTime = 6;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.rare = 6;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 4, 55, 0);
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/SDMBows/OldAdamBow_Glow").Value;
            }
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
			return new Vector2(-4, 0);
		}
		public int AttackMode;
        public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
            {
                Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/CooldownOver");
            }
            else
            {
                switch (AttackMode)
                {
                    case 0:
			    		Item.shootSpeed = 10;
						Item.UseSound = SoundID.Item5;
                        break;
                    case 1:
			    		Item.shootSpeed = 6;
						Item.UseSound = SoundID.Item5;
                        break;
                }
            }
			return true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (player.altFunctionUse == 2)
            {
                player.itemAnimationMax = 5;
                player.itemTime = 5;
                player.itemAnimation = 5;

                AttackMode++;
                if (AttackMode >= 2)
                    AttackMode = 0;

                switch (AttackMode)
                {
                    case 0:
                        CombatText.NewText(player.getRect(), Color.Red, Language.GetTextValue("Mods.FM.Items.OldAdamBow.Mode1"), true, false);
                        break;
                    case 1:
                        CombatText.NewText(player.getRect(), Color.Red, Language.GetTextValue("Mods.FM.Items.OldAdamBow.Mode2"), true, false);
                        break;
                }
            }
			else
			{
				switch (AttackMode)
				{
					case 0:
					    type = ModContent.ProjectileType<AdamBowProj>();
			    		int i = Main.myPlayer;
						float num72 = Item.shootSpeed;
						int num73 = Item.damage;
						float num74 = Item.knockBack;
						num74 = player.GetWeaponKnockback(Item, num74);
						player.itemTime = Item.useTime;
						Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
						Vector2 value = Vector2.UnitX.RotatedBy(player.fullRotation, default(Vector2));
						Vector2 vector3 = Main.MouseWorld - vector2;
						float num78 = Main.mouseX + Main.screenPosition.X - vector2.X;
						float num79 = Main.mouseY + Main.screenPosition.Y - vector2.Y;
						if (player.gravDir == -1f)
						{
							num79 = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
						}
						float num80 = (float)Math.Sqrt(num78 * num78 + num79 * num79);
						float num81 = num80;
						if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f))
						{
							num78 = player.direction;
							num79 = 0f;
							num80 = num72;
						}
						else
						{
							num80 = num72 / num80;
						}
						num78 *= num80;
						num79 *= num80;
						int num107 = 1;
						for (int num108 = 0; num108 < num107; num108++)
						{
							vector2 = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(201) * -(float)player.direction + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
							vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
							vector2.Y -= 100 * num108;
							num78 = Main.mouseX + Main.screenPosition.X - vector2.X;
							num79 = Main.mouseY + Main.screenPosition.Y - vector2.Y;
							if (num79 < 0f)
							{
								num79 *= -1f;
							}
							if (num79 < 20f)
							{
								num79 = 20f;
							}
							num80 = (float)Math.Sqrt(num78 * num78 + num79 * num79);
							num80 = num72 / num80;
							num78 *= num80;
							num79 *= num80;
							float speedX4 = num78 + Main.rand.Next(-0, 0) * 0.02f;
							float speedY5 = num79 + Main.rand.Next(-0, 0) * 0.02f;
							Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, type, num73, num74, i, 0f, Main.rand.Next(1));
						}
					break;
					case 1:
			    		float rot = (velocity).ToRotation();
						Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(6));
						int proj = Projectile.NewProjectile(source, position + FMHelper.PolarVector(2, rot) + FMHelper.PolarVector(Main.rand.NextFloat(-8, 8), rot + (float)Math.PI/2), perturbedSpeed, ModContent.ProjectileType<AdamBowProj>(), damage, knockback, player.whoAmI);
						Main.projectile[proj].tileCollide = true;
					break;
				}

			}
			return false;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string shotType = "";
            switch (AttackMode)
            {
                case 0:
                    shotType = Language.GetTextValue("Mods.FM.Items.OldAdamBow.Mode1");
                    break;
                case 1:
                    shotType = Language.GetTextValue("Mods.FM.Items.OldAdamBow.Mode2");
                    break;
            }
            TooltipLine line = new(Mod, "ShotName", shotType)
            {
                OverrideColor = Color.Red,
            };
            tooltips.Add(line);
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DaedalusStormbow, 1);
			recipe.AddRecipeGroup("FM:EvilBar", 5);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofMight, 6);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 100);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}