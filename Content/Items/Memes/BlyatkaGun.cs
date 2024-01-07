using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles;
using FM.Content.Projectiles.Other.BlyatkaGunProjs;
using FM.Globals;

namespace FM.Content.Items.Memes
{
	public class BlyatkaGun : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 61976;
			Item.shoot = ProjectileType<BlyatkaGunProj>();
			Item.shootSpeed = 15;
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 5;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.knockBack = 12;
			Item.rare = 0;
			//Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.scale = Main.rand.NextFloat(-0.1f, 1f);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
		public override bool? UseItem(Player player) 
		{
			if (!Main.dedServ) 
			{
				Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y - 10, player.width, player.height);
                CombatText.NewText(textPos, Main.DiscoColor, "Блять");
			}
			return null;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = new Vector2(player.Center.X + (float)Main.rand.Next(-100, 101), player.Center.Y + (float)Main.rand.Next(-100, 101) - 600);
            velocity = FMHelper.PolarVector(Item.shootSpeed, (Main.MouseWorld - position).ToRotation() + (float)Math.PI / 1000 - (float)Math.PI / 1000 * Main.rand.NextFloat());
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
	}
}