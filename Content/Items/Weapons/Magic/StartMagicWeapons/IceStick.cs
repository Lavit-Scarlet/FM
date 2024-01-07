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
using FM.Content.Projectiles.Magic;
using FM.Content.Items.Weapons.Melee;
using FM.Globals;

namespace FM.Content.Items.Weapons.Magic.StartMagicWeapons
{
	public class IceStick : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 10;
			Item.shoot = ProjectileType<IceStickProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 5;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(0, 0, 1, 20);
			Item.staff[Item.type] = true;
			Item.rare = 0;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.mana = 4;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/StartMagicWeapons/IceStick_Glow").Value;
            }
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Stick>(), 1);
			recipe.AddIngredient(ItemID.IceTorch, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
