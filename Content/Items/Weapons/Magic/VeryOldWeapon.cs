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
using FM.Content.Projectiles.Magic;

namespace FM.Content.Items.Weapons.Magic
{
	public class VeryOldWeapon : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.shoot = ProjectileType<ArmagenProj>();
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 4;
			Item.useAnimation = 4;
			Item.useStyle = 5;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(0, 22, 80, 0);
			Item.rare = 11;
			Item.UseSound = SoundID.Item33;
			Item.autoReuse = true;
			Item.mana = 2;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/VeryOldWeapon_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-23, 1);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(2));
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(25, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), perturbedSpeed, ProjectileType<ArmagenProj>(), damage, knockback, player.whoAmI);
            return false;
		}
	}
}