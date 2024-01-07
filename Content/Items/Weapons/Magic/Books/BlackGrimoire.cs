using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books.BlackGrimoireProjs;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class BlackGrimoire : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.shoot = ProjectileType<BlackGrimoireProj>();
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 42;
			Item.height = 36;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 10;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 14, 0, 0);
			Item.mana = 10;
			//Item.UseSound = SoundID.Item105;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/BlackGrimoire_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(1, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{	
			for (int i = 0; i < 3; i++)
        	{
        		float rot = (velocity).ToRotation();
            	Projectile.NewProjectile(source, position + FMHelper.PolarVector(Main.rand.NextFloat(-40f, -220f), rot) + FMHelper.PolarVector(Main.rand.NextFloat(-300f, 300f), rot + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI);
        	}
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 20);
			recipe.AddIngredient(ItemID.SoulofFright, 12);
			recipe.AddIngredient(ItemID.SoulofMight, 12);
			recipe.AddIngredient(ItemID.SoulofSight, 12);
			recipe.AddIngredient(ItemType<Materials.ScarletFragment>(), 300);
			recipe.AddIngredient(ItemType<Materials.FlanricCrystal>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}