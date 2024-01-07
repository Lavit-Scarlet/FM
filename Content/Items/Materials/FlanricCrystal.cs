using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.GameContent;

namespace FM.Content.Items.Materials
{
	public class FlanricCrystal : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}
		public override void SetDefaults() 
		{
			Item.width = 14;
			Item.height = 34;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = 10;
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

				spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(255, 0, 0, 150), rotation, frameOrigin, scale, SpriteEffects.None, 0);
			}
			for (float i = 0f; i < 1f; i += 0.34f) 
			{
				float radians = (i + timer) * MathHelper.TwoPi;

				spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(255, 0, 0, 150), rotation, frameOrigin, scale, SpriteEffects.None, 0);
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

                spriteBatch.Draw(texture, position + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(255, 0, 0, 150), 0, origin, scale, SpriteEffects.None, 0);
            }
            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, position + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(255, 0, 0, 150), 0, origin, scale, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(texture, position, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}