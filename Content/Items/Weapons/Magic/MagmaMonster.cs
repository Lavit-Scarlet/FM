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
using FM.Content.Projectiles.Magic.MagmaCannonProjs;

namespace FM.Content.Items.Weapons.Magic
{
	public class MagmaMonster : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.mana = 12;
			Item.shoot = ProjectileType<MagmaCannonProj>();
			Item.shootSpeed = 12;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 4, 8, 0);
			Item.rare = 6;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/MagmaCannon") 
			{
				Volume = 0.4f,
				Pitch = 0f
			};
			Item.autoReuse = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/MagmaMonster_Glow").Value;
            }
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(2));
			Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<MagmaCannonProj>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-40, 3);
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 62f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Magic.MagmaCannon>(), 1);
			recipe.AddIngredient(ItemID.LavaBucket, 4);
			recipe.AddRecipeGroup("FM:TitaniumBar", 6);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ItemID.SoulofFright, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}