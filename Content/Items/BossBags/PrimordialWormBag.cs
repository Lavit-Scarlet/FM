using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using FM.Content.Items.Materials;
using FM.Content.Items.Weapons.Magic;
using FM.Content.Items.Weapons.Melee;
using FM.Content.Items.Weapons.Melee.Swords;
using FM.Content.Items.Weapons.Melee.Spears;
using FM.Content.Items.Weapons.Ranged.Bows;
using FM.Content.NPCs.Bosses.PrimordialWorm;

namespace FM.Content.Items.BossBags
{
    public class PrimordialWormBag : ModItem
    {
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 3;
			ItemID.Sets.BossBag[Item.type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Item.type] = true;
		}
		public override void SetDefaults() 
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = 5;
			Item.expert = true;
		}
		public override bool CanRightClick() 
		{
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot) 
		{
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<PrimordialWormHead>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrimordialThrowingAx>(), 6));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrimordialSword>(), 2));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrimordialBow>(), 3));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrimordialLance>(), 2));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrimordialStaff>(), 2));
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

				spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(231, 178, 28, 50), rotation, frameOrigin, scale, SpriteEffects.None, 0);
			}
			for (float i = 0f; i < 1f; i += 0.34f) 
			{
				float radians = (i + timer) * MathHelper.TwoPi;

				spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(231, 178, 28, 77), rotation, frameOrigin, scale, SpriteEffects.None, 0);
			}
			return true;
		}
    }
}