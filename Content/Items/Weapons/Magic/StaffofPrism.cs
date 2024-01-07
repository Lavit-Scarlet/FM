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
using FM.Content.Projectiles.Magic.StaffofPrismProjs;

namespace FM.Content.Items.Weapons.Magic
{
	public class StaffofPrism : ModItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.mana = 2;
			Item.shoot = ProjectileType<StaffofPrismProj>();
			Item.shootSpeed = 20;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 34;
			Item.height = 72;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = 5;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(0, 2, 82, 0);
			Item.rare = -12;
			Item.autoReuse = false;
			Item.channel = true;
			Item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 24);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddIngredient(ItemID.UnicornHorn, 1);
			recipe.AddIngredient(ItemID.PinkGel, 24);
			recipe.AddIngredient(ItemID.Gel, 24);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) 
		{
			Texture2D texture = TextureAssets.Item[Item.type].Value;

			Rectangle frame;

			if (Main.itemAnimations[Item.type] != null) 
			{
				frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
			}
			else 
			{
				frame = texture.Frame();
			}

			Vector2 frameOrigin = frame.Size() / 2f;
			Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
			Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

			float time = Main.GlobalTimeWrappedHourly;
			float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

			time %= 4f;
			time /= 2f;

			if (time >= 1f) 
			{
				time = 2f - time;
			}

			time = time * 0.5f + 0.5f;

			for (float i = 0f; i < 1f; i += 0.25f) 
			{
				float radians = (i + timer) * MathHelper.TwoPi;

				spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, Main.DiscoColor * 0.5f, rotation, frameOrigin, scale, SpriteEffects.None, 0);
			}
			for (float i = 0f; i < 1f; i += 0.34f) 
			{
				float radians = (i + timer) * MathHelper.TwoPi;

				spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, Main.DiscoColor * 0.5f, rotation, frameOrigin, scale, SpriteEffects.None, 0);
			}
			return true;
		}

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
            Texture2D texture = TextureAssets.Item[Item.type].Value;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, position + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, Main.DiscoColor * 0.5f, 0, origin, scale, SpriteEffects.None, 0);
            }
            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, position + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, Main.DiscoColor * 0.5f, 0, origin, scale, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(texture, position, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
        }


    }
}