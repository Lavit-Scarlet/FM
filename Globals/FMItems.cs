using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using FM.Content.Projectiles.ReworkVanilla;
using FM.Globals;
using Terraria.GameContent.Creative;
using Terraria.Audio;

namespace FM.Globals
{
    public class FMItems : GlobalItem
    {
        public override void SetDefaults(Item item) 
        {
            if(item.type == ItemID.DiamondStaff)
            {
                item.StatsModifiedBy.Add(Mod);
				item.useTime = 8;
				item.useAnimation = 20;
                item.reuseDelay = 26;
                item.shoot = ProjectileType<DiamondStaffProj>();
            }
            if(item.type == ItemID.RubyStaff)
            {
                item.StatsModifiedBy.Add(Mod);
				item.useTime = 14;
				item.useAnimation = 14;
                item.mana = 5;
                item.shoot = ProjectileType<RubyStaffProj>();
            }
            if(item.type == ItemID.EmeraldStaff)
            {
                item.StatsModifiedBy.Add(Mod);
                item.shoot = ProjectileType<EmeraldStaffProj>();
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            if(item.type == ItemID.DiamondStaff)
            {
                SoundEngine.PlaySound(SoundID.Item43, player.Center);
				float rot = (velocity).ToRotation();
    			Projectile.NewProjectile(source, position + FMHelper.PolarVector(Main.rand.NextFloat(-8f, 8f), rot) + FMHelper.PolarVector(Main.rand.NextFloat(-12f, 12f), rot + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI);
                return false;
            }
            return true;
        }
        public override void ModifyShootStats(Item item, Terraria.Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if(item.type == ItemID.DiamondStaff)
            {
                Vector2 Offset = Vector2.Normalize(velocity) * 60f;
                if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
                {
                    position += Offset;
                }
            }
            if(item.type == ItemID.RubyStaff)
            {
                Vector2 Offset = Vector2.Normalize(velocity) * 60f;
                if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
                {
                    position += Offset;
                }
            }
            if(item.type == ItemID.EmeraldStaff)
            {
                Vector2 Offset = Vector2.Normalize(velocity) * 60f;
                if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
                {
                    position += Offset;
                }
            }
        }
    }
}