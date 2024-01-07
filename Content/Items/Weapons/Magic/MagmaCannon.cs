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
using FM.Content.Projectiles.Magic.MagmaCannonProjs;

namespace FM.Content.Items.Weapons.Magic
{
	public class MagmaCannon : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.mana = 20;
			Item.shoot = ProjectileType<MagmaCannonProj>();
			Item.shootSpeed = 8;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 64;
			Item.useAnimation = 64;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 2, 8, 0);
			Item.rare = 4;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/MagmaCannon") 
			{
				Volume = 0.4f,
				PitchVariance = 0.0f,
				MaxInstances = 1,
			};
			Item.autoReuse = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/MagmaCannon_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 50f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(ItemID.LavaBucket, 2);
			recipe.AddIngredient(ItemID.Obsidian, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}