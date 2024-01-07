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
	public class ElectroBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 52;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = 5;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 4, 80, 0);
			Item.rare = 7;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.Item72;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/ElectroBow_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return Vector2.Zero;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 2; i++)
			{
				ParticleManager.NewParticle<HollowCircle_Small2>(position, velocity * 3, Color.White, 0.15f, 0, 0);
				ParticleManager.NewParticle<HollowCircle_Small2>(position, velocity * 4, Color.White, 0.2f, 0, 0);
				ParticleManager.NewParticle<HollowCircle_Small2>(position, velocity * 5, Color.White, 0.3f, 0, 0);
				ParticleManager.NewParticle<HollowCircle_Small2>(position, velocity * 6.2f, Color.White, 0.36f, 0, 0);
			}
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, velocity.ToRotation() + (float)Math.PI/2), velocity, ProjectileType<ElectroBowProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 16);
			recipe.AddRecipeGroup("FM:TitaniumBar", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}