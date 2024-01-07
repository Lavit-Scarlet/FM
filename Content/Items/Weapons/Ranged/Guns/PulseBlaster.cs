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
	public class PulseBlaster : ModItem
	{
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.damage = 48;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 80;
			Item.height = 24;
			Item.shoot = 1;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/PlasmaShot") 
			{
				Volume = .3f,
				Pitch = .76f
			};
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 1f;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = 6;
			Item.autoReuse = true;
			Item.useAnimation = 8;
			Item.useTime = 8;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Guns/PulseBlaster_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, -1);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(4));
			Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<PulseBlasterProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 50f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Guns.GaussRifle>(), 1);
			recipe.AddRecipeGroup("FM:TitaniumBar", 2);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
