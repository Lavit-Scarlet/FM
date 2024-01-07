using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using FM.Globals;
using FM.Content.Projectiles.Melee.Swords;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class SwordofFate : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 38;
			Item.shoot = ProjectileType<SwordofFateProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 2, 3, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Melee/Swords/SwordofFate_Glow").Value;
            }
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Materials.SlushofFate>(), 24);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}