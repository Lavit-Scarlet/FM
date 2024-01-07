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

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class Spitefang : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.useAmmo = AmmoID.Arrow;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 20;
			Item.useAnimation = 25;
			Item.reuseDelay = 36;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 6;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 3, 55, 0);
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
			return new Vector2(1, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (int i = 0; i < 1; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(10));

				int proj = Projectile.NewProjectile(source, position, perturbedSpeed, 811, damage, knockback, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].penetrate = 1;
				Main.projectile[proj].timeLeft = 300;
				Main.projectile[proj].extraUpdates = 1;
			}
			return false;
        }
	}
}