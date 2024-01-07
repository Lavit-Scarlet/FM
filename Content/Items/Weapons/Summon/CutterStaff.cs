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
using FM.Content.Buffs.MinionBuffs;
using FM.Content.Projectiles.Minions;

namespace FM.Content.Items.Weapons.Summon
{
	public class CutterStaff : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Cutting Staff"); 
			//Tooltip.SetDefault("Summons 3 cyber knives that attack your enemies uses 3 minion slots\n" +
			//"You can still summon a minion if you don't have enough minion slots\n" +
			//"It doesn't work well because I don't like the summoner");

			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults() 
		{
			Item.damage = 52;
			Item.shoot = ProjectileType<CyberKnifeProj>();
			Item.buffType = ModContent.BuffType<CyberKnifeBuff>();
			Item.DamageType = DamageClass.Summon;
			//Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 3, 2, 0);
			Item.staff[Item.type] = true;
			Item.rare = 6;
			Item.UseSound = SoundID.Item44;
			Item.autoReuse = false;
			Item.mana = 10;
			Item.shootSpeed = 10;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) 
		{
			position = Main.MouseWorld;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			player.AddBuff(Item.buffType, 2);
			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("FM:TitaniumBar", 8);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
