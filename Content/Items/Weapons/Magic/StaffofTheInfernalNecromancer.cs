using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.SoTIN;

namespace FM.Content.Items.Weapons.Magic
{
	public class StaffofTheInfernalNecromancer : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 32;
			Item.shoot = ProjectileType<SotinFireboll>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 52;
			Item.useAnimation = 52;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 1, 37, 0);
			Item.staff[Item.type] = true;
			Item.rare = 5;
			Item.UseSound = SoundID.Item73;
			Item.autoReuse = true;
			Item.crit = 0;
			Item.mana = 20;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/StaffofTheInfernalNecromancer_Glow").Value;
            }
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 68f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
	}
}
