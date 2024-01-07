using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Localization;

namespace FM.Content.Tiles
{
	public class ElectroOreTile : ModTile
	{
		public override void SetStaticDefaults() 
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 410;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 575;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(216, 246, 253), name);

			DustType = 206;
			HitSound = SoundID.Tink;
			MineResist = 6f;
			MinPick = 200;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 8f;
            g = 8f;
            b = 8f;
        }
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			//float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Content/Tiles/ElectroOreTile_Glow").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.1f;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 2; k++)
			{
				Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.1f * k;
				Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 1f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}
