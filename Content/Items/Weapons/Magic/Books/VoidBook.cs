using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.Audio;
using FM.Content.Projectiles.Magic.Books;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class VoidBook : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.shoot = ProjectileType<VoidBookProj>();
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 5;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Void") 
			{
				Volume = .8f,
				Pitch = .16f
			};
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 2, 28, 0);
			Item.mana = 20;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/VoidBook_Glow").Value;
            }
		}
		public override bool CanUseItem(Player player)
		{
			if (Main.dayTime)
			{
				Item.damage = 12;
				Item.useTime = 60;
				Item.useAnimation = 60;
				Item.mana = 40;
			}
			else
			{
				Item.damage = 26;
				Item.useTime = 28;
				Item.useAnimation = 28;
				Item.mana = 20;
			}
			return true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int j = 0; j < 16; j++) 
			{
				ParticleManager.NewParticle<BloomCircle_PerfectNotRandScale>(position, velocity.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(.6f, 1.6f), Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(1f, 1.5f), 0, 0);
			}
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			return false;
		}
	}
}