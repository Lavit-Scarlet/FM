using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Ranged.Knifes;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace FM.Content.Items.Weapons.Ranged.Knifes
{
	public class CactusKnife : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.shoot = ProjectileType<CactusKnifeProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.useStyle = 1;
			Item.knockBack = 1;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 0, 01);
			Item.noUseGraphic = true;
			Item.maxStack = 9999;
			Item.consumable = true;
		}
		public override bool? UseItem(Player player)
        {
            /*int CKnife = player.FindItem(ItemType<CactusKnife>());
            if (CKnife >= 0)
            {
                player.inventory[CKnife].stack--;
                if (player.inventory[CKnife].stack <= 0)
				{
					player.inventory[CKnife] = new Item();
				}
            }*/
			if (!Main.dedServ && Item.UseSound.HasValue) 
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
            return null;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(3);
			recipe.AddIngredient(ItemID.Cactus, 1);
			recipe.Register();
		}
	}
}
