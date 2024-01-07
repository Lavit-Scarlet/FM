using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles.Melee.Swords.HerosDemonicSwordProjs;
using System;
using ReLogic.Content;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class HerosDemonicSword : ModItem
	{
        private bool reverseSwing = false;
		public override void SetDefaults()
		{
            // Common Properties
            Item.width = 50;
            Item.height = 50;
            Item.rare = 3;
            Item.value = Item.sellPrice(0, 1, 24, 0);

            // Use Properties
            Item.useStyle = 5;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            // Weapon Properties
            Item.damage = 18;
            Item.knockBack = 2;
			Item.crit += 6;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;

            // Projectile Properties
            Item.shootSpeed = 15f;
            Item.shoot = ModContent.ProjectileType<HerosDemonicSwordProj>();
		}
		public override ModItem Clone(Item newEntity)
		{
			var clone = (HerosDemonicSword)base.Clone(newEntity);
			clone.reverseSwing = reverseSwing;

			return clone;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = new Vector2(reverseSwing ? -1 : 1, 0);
			reverseSwing = !reverseSwing;
		}
        public int TimerCount() => 1;
	}
}
