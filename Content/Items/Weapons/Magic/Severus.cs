using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.SeverusProjs;

namespace FM.Content.Items.Weapons.Magic
{
	public class Severus : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 24;
			Item.shoot = ProjectileType<SeverusProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 1, 12, 0);
			Item.staff[Item.type] = true;
			Item.rare = 4;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.crit = 2;
			Item.mana = 10;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 45f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
	}
}
