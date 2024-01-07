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

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class DamnBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 68;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 5;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 5, 30, 0);
			Item.rare = 6;
			Item.autoReuse = true;
			Item.shootSpeed = 12;
			Item.UseSound = SoundID.Item102;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/DamnBow_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return Vector2.Zero;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			{
	    		float rot = (velocity).ToRotation();
	    		Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<DamnBowProj>(), damage, knockback, player.whoAmI);
				return false;
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ShadowFlameBow, 1);
			recipe.AddRecipeGroup("FM:TitaniumBar", 4);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}