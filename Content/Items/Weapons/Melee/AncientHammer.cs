using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Melee;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using FM.Globals;


namespace FM.Content.Items.Weapons.Melee
{
	public class AncientHammer : ModItem
	{
		private bool reverseSwing = false;

		public override void SetDefaults()
		{
            // Common Properties
            Item.width = 68;
            Item.height = 68;
            Item.rare = 8;
            Item.value = Item.sellPrice(0, 12, 0, 0);

            // Use Properties
            Item.useStyle = 5;
            Item.useAnimation = 62;
            Item.useTime = 62;
            Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/BigSlash") 
			{
				Volume = 1f,
				Pitch = -0.62f
			};
            Item.autoReuse = true;

            // Weapon Properties
            Item.damage = 240;
            Item.knockBack = 8;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;

            // Projectile Properties
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<AncientHammerProj>();
			Item.crit += 14;
		}
		public override ModItem Clone(Item newEntity)
		{
			var clone = (AncientHammer)base.Clone(newEntity);
			clone.reverseSwing = reverseSwing;

			return clone;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
            Vector2 velocity1 = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<AncientHammerProj>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity1, ProjectileType<AncientHammerShootProj>(), damage / 2, knockback, player.whoAmI);
			return false;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = new Vector2(reverseSwing ? -1 : 1, 0);
			reverseSwing = !reverseSwing;
		}
        public int TimerCount() => 1;
	}
}
