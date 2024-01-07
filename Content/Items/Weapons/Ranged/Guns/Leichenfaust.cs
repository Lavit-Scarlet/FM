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
	public class Leichenfaust : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 240;
			Item.shoot = ProjectileType<LeichenfaustProjTest>();
			Item.shootSpeed = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 70;
			Item.height = 30;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 4, 80, 0);
			Item.rare = 6;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/LeichenfaustGun")
			{
				Volume = 0.55f
			};
			Item.autoReuse = true;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 54f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("FM:TitaniumBar", 14);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 5);
			recipe.AddIngredient(ItemID.SoulofLight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

	}
}