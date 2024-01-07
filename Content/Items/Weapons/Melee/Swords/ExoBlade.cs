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
using FM.Content.Projectiles.Melee.Swords.ExoBladeProjs;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class ExoBlade : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 46;
			Item.shoot = 1;
			Item.shootSpeed = 24;
			Item.DamageType = DamageClass.Melee;
			Item.width = 77;
			Item.height = 77;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 5, 24, 0);
			Item.rare = 6;
			Item.crit = 6;
			Item.UseSound = SoundID.Item15;
			Item.autoReuse = false;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Melee/Swords/ExoBlade_Glow").Value;
            }
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float rot = (velocity).ToRotation();
            Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ModContent.ProjectileType<ExoBladeSlash>(), damage, knockback, player.whoAmI);

            //float adjustedItemScale = player.GetAdjustedItemScale(Item);
	    	//Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ModContent.ProjectileType<ExoBladeSwinging>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
	    	//NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
            return false;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("FM:TitaniumBar", 2);
			recipe.AddIngredient(ItemType<Materials.ElectroBar>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}