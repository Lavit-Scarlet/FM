using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic;

namespace FM.Content.Items.Weapons.Magic
{
	public class HallowedFlower : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 70;
			Item.shoot = ProjectileType<HallowedFlowerProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.staff[Item.type] = true;
			Item.rare = 7;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.mana = 4;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 2);
			recipe.AddIngredient(ItemID.SoulofMight, 2);
			recipe.AddIngredient(ItemID.SoulofSight, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 54f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
	}
}
