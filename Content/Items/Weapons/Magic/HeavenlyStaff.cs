using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic;
using ParticleLibrary;
using FM.Particles;
using Terraria.Audio;

namespace FM.Content.Items.Weapons.Magic
{
	public class HeavenlyStaff : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 360;
			Item.shoot = ProjectileType<HeavenlyStaffProj>();
			Item.shootSpeed = 12;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 8;
			Item.useAnimation = 38;
			Item.reuseDelay = 42;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 22, 0, 0);
			Item.staff[Item.type] = true;
			Item.rare = 10;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/HeavenlyShoot") 
			{
				Volume = 0.4f,
				Pitch = 1f
			};
			Item.autoReuse = true;
			Item.mana = 20;
		}
		public override bool? UseItem(Player player) 
		{
			if (!Main.dedServ && Item.UseSound.HasValue) 
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            ParticleManager.NewParticle<BloomCircle_Perfect>(position, Vector2.Zero, Color.White, 2f, 0, 0);
            ParticleManager.NewParticle<HollowCircle_Small>(position, Vector2.Zero, Color.White, 0.2f, 0, 0);
			float rot = (velocity).ToRotation();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(120));
			Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			return false;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 102f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
	}
}
