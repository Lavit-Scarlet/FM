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
using FM.Content.Projectiles.Magic.FireStaffProjs;
using FM.Content.Items.Weapons.Magic.StartMagicWeapons;

namespace FM.Content.Items.Weapons.Magic
{
	public class FireStaff : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 12;
			Item.shoot = ProjectileType<FireStaffProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.staff[Item.type] = true;
			Item.rare = 2;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.mana = 6;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/FireStaff_Glow").Value;
            }
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<FireStick>(), 1);
			recipe.AddRecipeGroup("FM:PlatinumBar", 4);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
