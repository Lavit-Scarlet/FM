using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using FM.Globals;
using FM.Content.Projectiles.Melee.Swords.HeavenlySwordProjs;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class HeavenlySword : ModItem
	{
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
		public override void SetDefaults()
		{
			Item.damage = 140;
			Item.shoot = ProjectileType<HeavenlySwordProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 17, 0, 0);
			Item.rare = 10;
			//Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
            Item.noMelee = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Melee/Swords/HeavenlySword_Glow").Value;
            }
		}
		private bool XZ = false;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 1;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileType<HeavenlySwordShootProj>(), damage, knockback, player.whoAmI);
            }
			Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<HeavenlySwordSlash>(), damage, knockback, player.whoAmI, 0f, XZ ? 1f : -1f) .ai[1] = XZ ? 1f : -1f;
            XZ = !XZ;
            return false;
        }
    }
}