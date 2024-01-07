using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace FM.Content.NPCs.Bosses.EpsilonBoss
{
    public class Epsilon : ModNPC
    {
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 4;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.defense = 0;
            NPC.lifeMax = 11200;
            NPC.value = Item.buyPrice(0, 8, 0, 0);
            NPC.knockBackResist = 0f;
            //NPC.HitSound = SoundID.Dig;
            //NPC.DeathSound = SoundID.NPCDeath43;
            NPC.width = 64;
			NPC.height = 64;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.boss = true;
            NPC.lavaImmune = true;
        }
        int direction = 1;
        public override void ReceiveExtraAI(BinaryReader reader)
		{
			direction = reader.ReadInt32();
		}
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(direction);
		}
        bool Move1 = false;
        Vector2 aimTo = Vector2.Zero;
        
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= (NPC.ai[0] == 2 ? 3 : 4))
			{
				NPC.frameCounter = 0;
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
			}
			NPC.spriteDirection = NPC.direction;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			drawColor = NPC.GetNPCColorTintedByBuffs(Color.White);
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

            Texture2D glowMask = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;
			Vector2 drawOrigin = new(glowMask.Width / 2, glowMask.Height / 2);
			Color color = Color.White;
			color.A = 0;
            spriteBatch.Draw(glowMask, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), null, color, NPC.rotation, drawOrigin, NPC.scale * 2.4f, effects, 0);
			return false;
		}
        public override void AI()
        {
            NPC.TargetClosest(true);
            bool expertMode = Main.expertMode; 
            Player player = Main.player[NPC.target];
            Vector2 dist = player.Center - NPC.Center;
			Vector2 direction = player.Center - NPC.Center;
			NPC.rotation = NPC.velocity.X * 0.07f;
			NPC.netUpdate = true;
			if (Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) < 9000) 
			{
				float speed = expertMode ? 21f : 18f; //made more aggressive.  expert mode is more.  dusking base value is 7
				float acceleration = expertMode ? 0.16f : 0.13f; //made more aggressive.  expert mode is more.  dusking base value is 0.09
				Vector2 vector2 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
				float xDir = player.position.X + (player.width / 2) - vector2.X;
				float yDir = (float)(player.position.Y + (player.height / 2) - 400) - vector2.Y;
				float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);
				if (length > 400f) 
				{
					++speed;
					acceleration += 0.08F;
					if (length > 600f) 
					{
						++speed;
						acceleration += 0.08F;
						if (length > 800f) 
						{
							++speed;
							acceleration += 0.08F;
						}
					}
				}
				float num10 = speed / length;
				xDir *= num10;
				yDir *= num10;
				if (NPC.velocity.X < xDir) 
				{
					NPC.velocity.X = NPC.velocity.X + acceleration;
					if (NPC.velocity.X < 0 && xDir > 0)
						NPC.velocity.X = NPC.velocity.X + acceleration;
				}
				else if (NPC.velocity.X > xDir) 
				{
					NPC.velocity.X = NPC.velocity.X - acceleration;
					if (NPC.velocity.X > 0 && xDir < 0)
						NPC.velocity.X = NPC.velocity.X - acceleration;
				}
				if (NPC.velocity.Y < yDir) 
				{
					NPC.velocity.Y = NPC.velocity.Y + acceleration;
					if (NPC.velocity.Y < 0 && yDir > 0)
						NPC.velocity.Y = NPC.velocity.Y + acceleration;
				}
				else if (NPC.velocity.Y > yDir) 
				{
					NPC.velocity.Y = NPC.velocity.Y - acceleration;
					if (NPC.velocity.Y > 0 && yDir < 0)
						NPC.velocity.Y = NPC.velocity.Y - acceleration;
				}
			}
        }
    }
}