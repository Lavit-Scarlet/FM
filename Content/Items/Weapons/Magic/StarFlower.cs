using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.StarFlowerProjs;
using Terraria.Audio;
using FM.Globals;
using Microsoft.Xna.Framework.Graphics;

namespace FM.Content.Items.Weapons.Magic
{
	public class StarFlower : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 24;
			Item.shoot = ProjectileType<StarFlowerProj>();
			Item.shootSpeed = 5;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 0, 48, 0);
			Item.staff[Item.type] = true;
			Item.rare = 4;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/StarFlower") 
			{
				Volume = 0.5f
			};
			Item.autoReuse = true;
			Item.mana = 20;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/StarFlower_Glow").Value;
            }
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
			modPlayer.Shake += 6;
			float rot = (velocity).ToRotation();
			Projectile.NewProjectile(source, position + FMHelper.PolarVector(0, rot) + FMHelper.PolarVector(0f, rot + (float)Math.PI/2), velocity, ProjectileType<BigStarFlowerProj>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1);
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 64f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FallenStar, 4);
			recipe.AddIngredient(ItemID.Cloud, 6);
			recipe.AddIngredient(ItemID.SunplateBlock, 12);
			recipe.AddTile(TileID.SkyMill);
			recipe.Register();
		}
	}
}
