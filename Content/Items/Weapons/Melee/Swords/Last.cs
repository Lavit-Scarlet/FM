using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using FM.Content.Projectiles.Melee.Swords.LastProjs;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class Last : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 140;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<LastProj>();
			Item.shootSpeed = 15;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.width = 17;
			Item.height = 61;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = Item.sellPrice(0, 14, 24, 0);
			Item.rare = 10;
			Item.UseSound = SoundID.DD2_PhantomPhoenixShot;
			Item.autoReuse = true;
			Item.crit += 12;
			Item.scale = 1f;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Melee/Swords/Last_Glow").Value;
            }
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FragmentSolar, 18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI);

			float adjustedItemScale = player.GetAdjustedItemScale(Item);
			Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ModContent.ProjectileType<LastSwinging>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
			NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
            return false;

		}
	}
}