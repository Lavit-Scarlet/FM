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
using FM.Content.Projectiles.Magic.GaussRailGunProjs;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Items.Weapons.Magic
{
	public class GaussRailGun : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.shoot = ProjectileType<GaussRailGunLaser>();
			Item.shootSpeed = 2.4f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;			
			Item.useAnimation = 24;
			Item.reuseDelay = 24;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 4, 8, 0);
			Item.rare = 6;
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.mana = 15;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/GaussRailGun_Glow").Value;
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
			return new Vector2(-30, 0);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("FM:TitaniumBar", 6);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI);
            return false;
		}
	}
}