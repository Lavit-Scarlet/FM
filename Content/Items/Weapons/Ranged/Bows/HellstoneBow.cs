using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using FM.Content.Projectiles.Ranged.Bows.HellstoneBowProjs;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Items.Weapons.Ranged.Bows
{
	public class HellstoneBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.useStyle = 5;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 1, 80, 0);
			Item.rare = 4;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.DD2_BallistaTowerShot;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Bows/HellstoneBow_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return Vector2.Zero;
		}
	    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for (int j = 0; j < 10; j++) 
			{
				ParticleManager.NewParticle<LineStreak_Long_ShortTime>(position, velocity.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(.2f, .4f), new Color(55, 55, 55), Main.rand.NextFloat(0.5f, 1f), 0, 0);
			}
		    for (int i = 0; i < 1; i++)
		    {
		    	Vector2 perturbedSpeed1 = velocity.RotatedByRandom(MathHelper.ToRadians(2));
		    	Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed1.X, perturbedSpeed1.Y, ModContent.ProjectileType<HellstoneBowProj>(), damage, knockback, player.whoAmI);
		    }
			return false;
        }
		/*public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = 41;
			}
		}*/
	}
}