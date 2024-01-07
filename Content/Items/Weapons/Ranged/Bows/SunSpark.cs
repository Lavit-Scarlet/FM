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
using FM.Content.Projectiles.Ranged.Bows.SunSparkProjs;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class SunSpark : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 5;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 7, 30, 0);
			Item.rare = 8;
			Item.autoReuse = true;
			Item.shootSpeed = 10;
			Item.UseSound = SoundID.Item5;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/SunSpark_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return Vector2.Zero;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
	    	float rot = (velocity).ToRotation();
	    	Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<SunSparkProj>(), damage, knockback, player.whoAmI);
			return false;
		}
	}
}