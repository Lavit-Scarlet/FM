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
using FM.Content.NPCs.Bosses.CryoGuardianBoss.CryoGuardianProjs;
using FM.Content.Items.Weapons.Magic;
using FM.Content.Items.BossBags;
using FM.Content.Items.Weapons.Melee.Spears;
using FM.Content.Items.Placeable.MusicBoxes;
using FM.Content.Items.Weapons.Ranged.Guns;
using FM.Content.Items.Weapons.Ranged.Bows;

namespace FM.Content.NPCs.Bosses.CryoGuardianBoss
{
    [AutoloadBossHead]
    public class CryoGuardian : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 18;
            NPC.defense = 20;
            NPC.lifeMax = 2000;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath15;
            NPC.width = 58;
			NPC.height = 58;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
			NPC.npcSlots = 15;
			NPC.value = Item.buyPrice(0, 2, 0, 0);
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/CryoGuardian");
			NPC.buffImmune[24] = true;
			NPC.buffImmune[44] = true;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CryoGuardianBag>()));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<IceFlower>(), 1));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SpearofCryoGuardian>(), 2));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Severus>(), 12));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FrostPistol>(), 3));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FrostyCrossbow>(), 3));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CryoGuardianMusicBoxItem>(), 15));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.IceBlock, 1, 20, 40));
        }
		int timer = 0;
		int delay = 0;
        int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 300f;
        private int chosenDirection;
        public override void OnSpawn(IEntitySource source)
        {
            chosenDirection = Main.rand.NextBool(2) ? -1 : 1;
        }

        public override void AI()
        {
			NPC.TargetClosest(true);
			NPC.rotation = NPC.velocity.X * 1f;

			Player player = Main.player[NPC.target];

			if (NPC.Center.X >= player.Center.X && moveSpeed >= -120)
				moveSpeed--;
			else if (NPC.Center.X <= player.Center.X && moveSpeed <= 120)
				moveSpeed++;

			NPC.velocity.X = moveSpeed * 0.10f;

			if (NPC.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30)
			{
				moveSpeedY--;
				HomeY = 350f;
			}
			else if (NPC.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
				moveSpeedY++;

			NPC.velocity.Y = moveSpeedY * 0.13f;

			timer++;

			if (timer == 200 || timer == 250 || timer == 300 || timer == 320 || timer == 340 || timer == 360|| timer == 380 || timer == 400 && NPC.life >= (NPC.lifeMax / 2)) //16 rot
			{
                SoundEngine.PlaySound(SoundID.Item28, NPC.position);

                int n = 16;
                int deviation = Main.rand.Next(0, 60);
                for (int i = 0; i < n; i++)
                {
                    float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(120, 120).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= 12f;
                    perturbedSpeed.Y *= 12f;
					int damage = Main.expertMode ? 12 : 16;
					if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ProjectileType<CryoGuardianProj>(), damage, 1, Main.myPlayer, 0, 0);
                }
			}
			else if (timer == 300 || timer == 400 || timer == 450 || timer == 500 || timer == 550)//20rot
			{
				SoundEngine.PlaySound(SoundID.Item60, NPC.position);
                int n = 20;
                int deviation = Main.rand.Next(0, 60);
                for (int i = 0; i < n; i++)
                {
                    float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(120, 120).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= 12f;
                    perturbedSpeed.Y *= 12f;
			    	int damage = Main.expertMode ? 15 : 18;
			    	if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ProjectileType<CryoGuardianBigAirProj>(), damage, 1, Main.myPlayer, 0, 0);
                }
			}
			else if (timer == 650 || timer == 700|| timer == 750 || timer == 800 || timer == 850)//star
			{
				SoundEngine.PlaySound(SoundID.Item28, NPC.position);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
                    Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
                    direction.Normalize();
                    direction.X *= 10f;
                    direction.Y *= 10f;
                    
                    float A = (float)Main.rand.Next(-50, 50) * 0.16f;
                    float B = (float)Main.rand.Next(-50, 50) * 0.16f;
                    int damage = Main.expertMode ? 12 : 14;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<CryoGuardianStarProj>(), damage, 1, Main.myPlayer, 0, 0);
				}
			}
			if (timer == 750)
				HomeY = -5f;
				
			else if ((timer >= 900 && timer <= 1200))
			    NPC.velocity = Vector2.Zero;
			if (timer == 940 || timer == 980 || timer == 1020 || timer == 1060 || timer == 1100 || timer == 1120 || timer == 1140 || timer == 1160)
			{
				SoundEngine.PlaySound(SoundID.Item60, NPC.position);

				int n = 8;
                int deviation = Main.rand.Next(0, 180);
                for (int i = 0; i < n; i++)
                {
                    float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(120, 120).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= 10f;
                    perturbedSpeed.Y *= 10f;
			    	int damage = Main.expertMode ? 12 : 16;
			    	if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ProjectileType<CryoGuardianBigProj>(), damage, 1, Main.myPlayer, 0, 0);
                    NPC.netUpdate = true;
                }
				
			}
			else if (timer == 1250 || timer == 1260 || timer == 1270 || timer == 1280  || timer == 1290  || timer == 1300  || timer == 1305  || timer == 1310  || timer == 1315  || timer == 1320)
			{
				SoundEngine.PlaySound(SoundID.Item28, NPC.position);
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
                    Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
                    direction.Normalize();
                    direction.X *= 14f;
                    direction.Y *= 14f;
                    {
                        float A = (float)Main.rand.Next(-50, 50) * 0.01f;
                        float B = (float)Main.rand.Next(-50, 50) * 0.01f;
						int damage = Main.expertMode ? 10 : 14;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<CryoGuardianProj>(), damage, 1, Main.myPlayer, 0, 0);
                    }
				}
			}
			if (timer >= 1350)
				timer = 0;

			if (!player.active || player.dead)
			{
				delay++;
				NPC.TargetClosest(false);
				NPC.velocity.Y = 17;
				//timer = 0;
			}
			if (NPC.life <= NPC.lifeMax * .4f)
			{
				if (Main.expertMode)
				{
					if (Main.rand.NextBool(14) && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int offsetX = Main.rand.Next(-200, 200) * 5;
						int damage = Main.expertMode ? 18 : 22;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + offsetX, player.Center.Y - 1200, 0f, 10f, ModContent.ProjectileType<CryoGuardianBigAirProj>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}
			float maxWeatherFactor = (Main.expertMode ? 0.5f : 0.25f);
			float weatherFormula = maxWeatherFactor * ((NPC.lifeMax * 0.00025f) - (NPC.life * 0.00025f));
			if (NPC.active)
			{
				Main.raining = true;
				Main.rainTime = 300f;
				Main.maxRaining = weatherFormula * 0.5f + 0.5f;
				Main.windSpeedCurrent = weatherFormula * chosenDirection;
			}
			else
			{
				Main.raining = false;
				Main.rainTime = 0f;
				Main.maxRaining = 0f;
				Main.windSpeedCurrent = 0f;
			}
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 80);
            }
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 80);
                    Main.dust[dust].velocity *= 2;
                    Main.dust[dust].scale = 2;
                }
                int CGDG = Mod.Find<ModGore>("CryoGuardianDeathGore").Type;
                int CGDG1 = Mod.Find<ModGore>("CryoGuardianDeathGore1").Type;
                int CGDG2 = Mod.Find<ModGore>("CryoGuardianDeathGore2").Type;
                var entitySource = NPC.GetSource_Death();
                {
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CGDG);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CGDG1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CGDG1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CGDG1);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CGDG1);

                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CGDG2);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CGDG2);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CGDG2);
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, CGDG2);
                }
            }
        }
    }
}