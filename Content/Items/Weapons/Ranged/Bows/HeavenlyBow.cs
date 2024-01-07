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
using FM.Content.Projectiles.Ranged.Bows.HeavenlyBowProjs;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class HeavenlyBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.crit += 12;
			Item.DamageType = DamageClass.Ranged;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 6;
			Item.useAnimation = 14;
			Item.reuseDelay = 24;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 10;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 26, 50, 0);
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/HeavenlyBow_Glow").Value;
            }
		}
		public override bool? UseItem(Player player) 
		{
			if (!Main.dedServ && Item.UseSound.HasValue) 
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(4));
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(Main.rand.NextFloat(0f, 0f), rot) + FMHelper.PolarVector(Main.rand.NextFloat(-8f, 8f), rot + (float)Math.PI/2), perturbedSpeed, ProjectileType<HeavenlyBowProj>(), damage, knockback, player.whoAmI);
			return false;
		}
	}
}