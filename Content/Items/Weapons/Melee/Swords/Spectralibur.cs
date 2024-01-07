using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Projectiles.Melee.Swords.SpectraliburProjs;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using FM.Globals;
using FM.Base;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class Spectralibur : ModItem
	{
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
		public override void SetDefaults()
		{
            // Common Properties
            Item.width = 56;
            Item.height = 56;
            Item.rare = 8;
            Item.value = Item.sellPrice(0, 6, 24, 0);

            // Use Properties
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = 22;
            Item.useTime = 22;
            /*Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/BigSlash") 
			{
				Volume = 0.5f,
			};*/
            Item.autoReuse = true;

            // Weapon Properties
            Item.damage = 68;
            Item.knockBack = 4;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;

            // Projectile Properties
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<SpectraliburShootProj>();
		}
		private bool XZ = false;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
            Vector2 velocity1 = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(-60, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity1, ProjectileType<SpectraliburShootProj>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<SpectraliburSlash>(), damage, knockback, player.whoAmI, 0f, XZ ? 1f : -1f) .ai[1] = XZ ? 1f : -1f;
            XZ = !XZ;
			return false;
		}

        
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Excalibur, 1);
			recipe.AddIngredient(ItemID.SpectreBar, 18);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}