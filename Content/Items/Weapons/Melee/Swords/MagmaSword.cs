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
using FM.Content.Projectiles.Melee.Swords.MagmaSwordProjs;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class MagmaSword : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 38;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<MagmaSwordWave>();
			Item.shootSpeed = 15;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.width = 17;
			Item.height = 61;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 2, 24, 0);
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Melee/Swords/MagmaSword_Glow").Value;
            }
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI);

			float adjustedItemScale = player.GetAdjustedItemScale(Item);
			Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ModContent.ProjectileType<MagmaSwordSwinging>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
			NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
            return false;
		}
	}
}