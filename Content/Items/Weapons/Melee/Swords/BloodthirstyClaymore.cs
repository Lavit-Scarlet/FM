using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Melee.Swords;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using FM.Globals;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class BloodthirstyClaymore : ModItem
	{
		private bool reverseSwing = false;

		public override void SetDefaults()
		{
            // Common Properties
            Item.width = 56;
            Item.height = 56;
            Item.rare = 6;
            Item.value = Item.sellPrice(0, 3, 0, 0);

            // Use Properties
            Item.useStyle = 5;
            Item.useAnimation = 48;
            Item.useTime = 48;
            Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/BigSlash") 
			{
				Volume = 0.5f,
			};
            Item.autoReuse = true;

            // Weapon Properties
            Item.damage = 80;
            Item.knockBack = 8;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;

            // Projectile Properties
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<BloodthirstyClaymoreSlashProj>();
			Item.crit += 26;
		}
		public override ModItem Clone(Item newEntity)
		{
			var clone = (BloodthirstyClaymore)base.Clone(newEntity);
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
