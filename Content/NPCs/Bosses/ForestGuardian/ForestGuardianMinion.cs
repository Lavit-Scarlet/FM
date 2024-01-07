using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FM.Content.NPCs.Bosses.ForestGuardian
{
	public class ForestGuardianMinion : ModNPC
	{
		public override void SetDefaults()
		{
			NPC.width = NPC.height = 34;
			NPC.alpha = 255;
			NPC.damage = 14;
			NPC.lifeMax = 8;
			NPC.defense = 0;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.DeathSound = SoundID.Grass;
			NPC.noTileCollide = true;
		}

		bool hasTarget = false;

		public override bool PreAI()
		{
			if (NPC.localAI[0] == 0)
				NPC.localAI[0] = (float)Math.Sqrt(NPC.velocity.X * NPC.velocity.X + NPC.velocity.Y * NPC.velocity.Y);

			if (NPC.alpha > 0)
				NPC.alpha -= 25;
			else
				NPC.alpha = 0;

			float dirX = NPC.position.X;
			float dirY = NPC.position.Y;
			float distance = 5000f;
			int target = 0;

			if (NPC.ai[1] == 0f && !hasTarget)
			{
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active && !Main.player[i].dead && (NPC.ai[1] == 0f || NPC.ai[1] == (float)(i + 1)))
					{
						Vector2 playerPosition = new Vector2(Main.player[i].position.X + (Main.player[i].width / 2), Main.player[i].position.Y + (Main.player[i].height / 2));
						Vector2 npcPosition = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
						float currentDistance = (playerPosition - npcPosition).Length();
						if (currentDistance < distance)
						{
							distance = currentDistance;
							dirX = playerPosition.X;
							dirY = playerPosition.Y;
							hasTarget = true;
							target = i;
						}
					}
				}
				if (hasTarget)
					NPC.ai[1] = (target + 1);
			}

			if (NPC.ai[1] > 0f)
			{
				int index = (int)(NPC.ai[1] - 1f);
				if (Main.player[index].active && !Main.player[index].dead)
				{
					Vector2 playerPosition = new Vector2(Main.player[index].position.X + (Main.player[index].width / 2), Main.player[index].position.Y + (Main.player[index].height / 2));
					Vector2 npcPosition = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
					float currentDistance = (npcPosition - playerPosition).Length();
					if (currentDistance < 5000f)
					{
						hasTarget = true;
						dirX = playerPosition.X;
						dirY = playerPosition.Y;
					}
				}
				else
				{
					NPC.ai[1] = 0f;
					hasTarget = false;
				}
			}

			if (hasTarget)
			{
				dirX -= NPC.Center.X;
				dirY -= NPC.Center.Y;
				float l = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				l = NPC.localAI[0] / l;
				dirX *= l;
				dirY *= l;
				float followSpeed = 25;
				NPC.velocity.X = (NPC.velocity.X * (followSpeed - 1) + dirX) / followSpeed;
				NPC.velocity.Y = (NPC.velocity.Y * (followSpeed - 1) + dirY) / followSpeed;
			}
			NPC.rotation += (float)NPC.spriteDirection * 0.3f;

			int dust5 = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Grass, NPC.velocity.X * 1.5f, NPC.velocity.Y * 1.5f);
			Main.dust[dust5].velocity *= 0f;
			Main.dust[dust5].noGravity = true;
			Main.dust[dust5].scale = 0.5f;
			return false;
		}
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Grass);
            }
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass);
                    Main.dust[dust].velocity *= 2;
                    Main.dust[dust].scale = 2;
				}
			}
        }
	}
	
}