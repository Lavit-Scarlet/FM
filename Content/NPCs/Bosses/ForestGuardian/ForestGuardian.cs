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
using Terraria.Utilities;
using Terraria.UI;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.UI;
using System;
using FM.Content.Items.Materials;
using static Terraria.ModLoader.ModContent;
using FM.Content.NPCs.Bosses.ForestGuardian.ForestGuardianProjs;
using FM.Content.NPCs.Bosses.ForestGuardian;
using FM.Content.Items.Weapons.Magic.Books;
using FM.Content.Items.BossBags;
using FM.Content.Items.Weapons.Melee.Spears;

namespace FM.Content.NPCs.Bosses.ForestGuardian
{
	[AutoloadBossHead]
    public class ForestGuardian : ModNPC
    {
        public int currentSpread;
        public override void SetDefaults()
        {
            NPC.damage = 10;
            NPC.defense = 0;
            NPC.lifeMax = 777;
            NPC.value = Item.buyPrice(0, 0, 70, 0);
            NPC.knockBackResist = 0.1f;
            NPC.HitSound = SoundID.Grass;
            NPC.DeathSound = SoundID.NPCDeath15;
            NPC.width = 62;
			NPC.height = 62;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.npcSlots = 10f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<ForestGuardianBag>()));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ForestBook>(), 1));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.Wood, 1, 20, 40));
			notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.Acorn, 1, 5, 10));
        }
        public override bool PreAI()
        {
            if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(false);
				NPC.velocity.Y = -20;
			}
            if (NPC.ai[0] == 0)
            {
                if (NPC.ai[2] == 0)
				{
					NPC.TargetClosest(true);
					NPC.ai[2] = NPC.Center.X >= Main.player[NPC.target].Center.X ? -1f : 1f;
				}
				NPC.TargetClosest(true);

				Player player = Main.player[NPC.target];
				if (!player.active || player.dead)
				{
					NPC.TargetClosest(false);
					NPC.velocity.Y = -20;
				}

				float currentXDist = Math.Abs(NPC.Center.X - player.Center.X);
				if (NPC.Center.X < player.Center.X && NPC.ai[2] < 0)
					NPC.ai[2] = 0;
				if (NPC.Center.X > player.Center.X && NPC.ai[2] > 0)
					NPC.ai[2] = 0;

				float accelerationSpeed = 0.1F;
				float maxXSpeed = 7;
				NPC.velocity.X = NPC.velocity.X + NPC.ai[2] * accelerationSpeed;
				NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -maxXSpeed, maxXSpeed);

				float yDist = player.position.Y - (NPC.position.Y + NPC.height);
				if (yDist < 0)
					NPC.velocity.Y = NPC.velocity.Y - 0.2F;
				if (yDist > 150)
					NPC.velocity.Y = NPC.velocity.Y + 0.2F;
				NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -6, 6);
				NPC.rotation = NPC.velocity.X * 1f;

				if ((currentXDist < 500 || NPC.ai[3] < 0) && NPC.position.Y < player.position.Y)
				{
					++NPC.ai[3];
					int cooldown = 15;
					if (NPC.life < NPC.lifeMax * 0.75)
						cooldown = 154;
					if (NPC.life < NPC.lifeMax * 0.5)
						cooldown = 13;
					if (NPC.life < NPC.lifeMax * 0.25)
						cooldown = 12;
					cooldown++;
					if (NPC.ai[3] > cooldown)
						NPC.ai[3] = -cooldown;

					if (NPC.ai[3] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 position = NPC.Center;

						float speedX = player.Center.X - NPC.Center.X;
						float speedY = player.Center.Y - NPC.Center.Y;
						float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						float speed = 10;

						if (NPC.life < NPC.lifeMax * 0.25f)
							speed = 16f;
						else if (NPC.life < NPC.lifeMax * 0.5f)
							speed = 14f;
						else if (NPC.life < NPC.lifeMax * 0.75f)
							speed = 12f;

						float num12 = speed / length;
						speedX *= num12;
						speedY *= num12;
						int damage = Main.expertMode ? 6 : 9;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), position.X, position.Y, speedX, speedY, ModContent.ProjectileType<ForestGuardianMiniProj>(), damage, 0, Main.myPlayer);
					}
				}
				else if (NPC.ai[3] < 0)
					NPC.ai[3]++;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[1] += Main.rand.Next(1, 4);
					if (NPC.ai[1] > 800 && currentXDist < 600)
						NPC.ai[0] = -1;
				}
            }
			else if (NPC.ai[0] == 1)
			{
				if (NPC.ai[2] == 0)
				{
					NPC.TargetClosest(true);
					NPC.ai[2] = NPC.Center.X >= Main.player[NPC.target].Center.X ? -1f : 1f;
				}
				NPC.TargetClosest(true);
				Player player = Main.player[NPC.target];

				if (NPC.Center.X < player.Center.X && NPC.ai[2] < 0)
					NPC.ai[2] = 0;
				if (NPC.Center.X > player.Center.X && NPC.ai[2] > 0)
					NPC.ai[2] = 0;

				float accelerationSpeed = 0.07f;
				float maxXSpeed = 5;
				NPC.velocity.X = NPC.velocity.X + NPC.ai[2] * accelerationSpeed;
				NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -maxXSpeed, maxXSpeed);

				float yDist = player.position.Y - (NPC.position.Y + NPC.height);
				if (yDist < 0)
					NPC.velocity.Y = NPC.velocity.Y - 0.2F;
				if (yDist > 150)
					NPC.velocity.Y = NPC.velocity.Y + 0.2F;
				NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -6, 6);

				NPC.rotation = NPC.velocity.X * 2f;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[3]++;
					if (NPC.ai[3] % 15 == 0 && NPC.ai[3] <= 50)
					{
						Vector2 pos = NPC.Center;
						if (!WorldGen.SolidTile((int)(pos.X / 16), (int)(pos.Y / 16)))
						{
							Vector2 dir = player.Center - pos;
							dir.Normalize();
							dir *= 8;
							int damage = Main.expertMode ? 8 : 10;
							Projectile.NewProjectile(NPC.GetSource_FromAI(), pos.X, pos.Y, dir.X, dir.Y, ModContent.ProjectileType<ForestGuardianProj>(), damage, 0, Main.myPlayer);
							currentSpread++;
						}
					}

					int cooldown = 100;
					if (NPC.life < NPC.lifeMax * 0.75)
						cooldown = 90;
					if (NPC.life < NPC.lifeMax * 0.5)
						cooldown = 80;
					if (NPC.life < NPC.lifeMax * 0.25)
						cooldown = 70;
					if (NPC.life < NPC.lifeMax * 0.1)
						cooldown = 60;
					if (NPC.ai[3] >= cooldown)
						NPC.ai[3] = 0;

					NPC.ai[1] += Main.rand.Next(1, 4);
					if (NPC.ai[1] > 600.0)
						NPC.ai[0] = -1f;
				}
			}
			else if (NPC.ai[0] == 2)
			{
				if (NPC.velocity.X > 0)
					NPC.velocity.X -= 0.1f;
				if (NPC.velocity.X < 0)
					NPC.velocity.X += 0.1f;
				if (NPC.velocity.X > -0.2f && NPC.velocity.X < 0.2f)
					NPC.velocity.X = 0;
				if (NPC.velocity.Y > 0)
					NPC.velocity.Y -= 0.1f;
				if (NPC.velocity.Y < 0)
					NPC.velocity.Y += 0.1f;
				if (NPC.velocity.Y > -0.2f && NPC.velocity.Y < 0.2f)
					NPC.velocity.Y = 0;

				NPC.rotation += (float)NPC.spriteDirection * 0.1f;

				NPC.ai[3]++;
				if (NPC.ai[3] >= 60)
				{
					if (NPC.ai[3] % 52 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = 1.9f;
						int dust1 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass);
						Main.dust[dust1].noGravity = true;
						Main.dust[dust1].scale = 1.9f;
						int dust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass);
						Main.dust[dust2].noGravity = true;
						Main.dust[dust2].scale = 1.9f;
						int dust3 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass);
						Main.dust[dust3].noGravity = true;
						Main.dust[dust3].scale = 1.9f;
						Vector2 direction = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360));
						int newNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ForestGuardianMinion>());
						Main.npc[newNPC].velocity = direction * 5;
					}
				}

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[1] += Main.rand.Next(1, 4);
					if (NPC.ai[1] > 500)
						NPC.ai[0] = -1f;
				}
			}
			else if (NPC.ai[0] == 3)
			{
				NPC.velocity.Y -= 0.1F;
				NPC.alpha += 2;
				if (NPC.alpha >= 255)
					NPC.active = false;
				if (NPC.velocity.X > 0)
					NPC.velocity.X -= 0.2F;
				if (NPC.velocity.X < 0)
					NPC.velocity.X += 0.2F;
				if (NPC.velocity.X > -0.2F && NPC.velocity.X < 0.2F)
					NPC.velocity.X = 0;

				NPC.rotation = NPC.velocity.X * 2f;
			}

			int dust5 = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Grass, NPC.velocity.X * 1.5f, NPC.velocity.Y * 1.5f);
			Main.dust[dust5].velocity *= 0f;
			Main.dust[dust5].noGravity = true;
			Main.dust[dust5].scale = 0.5f;

			if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(true);
				if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
				{
					NPC.ai[0] = 3;
					NPC.ai[3] = 0;
				}
			}

			if (NPC.ai[0] != -1)
				return false;
            
			int num = Main.rand.Next(3);
			NPC.TargetClosest(true);
			if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) > 1000)
				num = 0;
			NPC.ai[0] = num;
			NPC.ai[1] = 0;
			NPC.ai[2] = 0;
			NPC.ai[3] = 0;

			return true;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Grass);
            }
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass);
                    Main.dust[dust].velocity *= 4;
                    Main.dust[dust].scale = 3;
				}
			}
        }
    }
    
}