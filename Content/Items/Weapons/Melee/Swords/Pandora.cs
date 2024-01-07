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
using FM.Content.Projectiles.Melee.Swords.PandoraProjs;
using System.IO;
using FM.Base;
using Terraria.Graphics.Shaders;
using FM.Content.Items.Weapons.Melee.Swords;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class Pandora : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 56;
			Item.shoot = ProjectileType<PandoraProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = Item.sellPrice(0, 6, 6, 0);
			Item.rare = 6;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.crit += 26;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox) 
		{
			if (Main.rand.NextBool(1)) 
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
			}
		}
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Slashing"), (int)player.Center.X, (int)player.Center.Y, .5f);
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<PandoraCutting>(), 60, 0, player.whoAmI, Main.rand.NextFloat(MathHelper.TwoPi), 1f);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, type, damage, knockback / 2, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Carnage>(), 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ItemID.SoulofFlight, 6);
			recipe.AddIngredient(ItemID.SoulofMight, 6);
			recipe.AddIngredient(ItemID.SoulofFright, 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<HerosDemonicSword>(), 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ItemID.SoulofFlight, 6);
			recipe.AddIngredient(ItemID.SoulofMight, 6);
			recipe.AddIngredient(ItemID.SoulofFright, 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}