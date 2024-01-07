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
using Terraria.Audio;
using FM.Content.Items.Weapons.Melee;
using FM.Globals;

namespace FM.Content.Items.Weapons.Magic.StartMagicWeapons
{
	public class WaterStick : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 8;
			Item.shoot = 22;
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 5;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(0, 0, 1, 15);
			Item.staff[Item.type] = true;
			Item.rare = 0;
			Item.UseSound = SoundID.Item13;
			Item.autoReuse = true;
			Item.mana = 2;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/StartMagicWeapons/WaterStick_Glow").Value;
            }
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Stick>(), 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
