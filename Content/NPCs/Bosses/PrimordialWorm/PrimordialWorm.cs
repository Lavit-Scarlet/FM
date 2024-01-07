using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using ReLogic.Content;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.Utilities;
using ParticleLibrary;
using FM.Particles;
using FM.Content.NPCs.Bosses.PrimordialWorm;
using FM.Globals;
using FM.Content.Items.Weapons.Magic;
using FM.Content.Items.Weapons.Melee;
using FM.Content.Items.Weapons.Melee.Swords;
using FM.Content.Items.Weapons.Melee.Spears;
using FM.Content.Items.Weapons.Ranged.Bows;
using FM.Content.Items.BossBags;

namespace FM.Content.NPCs.Bosses.PrimordialWorm
{
	[AutoloadBossHead]
    public class PrimordialWormHead : ModNPC
    {
		ref float CurrentAttackType => ref NPC.ai[0];
		ref float AttackTimer => ref NPC.ai[1];
		ref float AttackTypeCounter => ref NPC.ai[2];
		ref float DeathAnimation => ref NPC.ai[3];
		
		const float JustCrawling = 0;
		const float ChargeToPlayer = 1;
		const float ArcticBlasts = 2;
		const float PrimordialWormBreath = 3;
		const float RainingIcicles = 4;
		
		float SoundSlotPlayer;
		float SoundSlotNPC;
		float despawnCounter;
		
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frozen] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
        }
		
        public override void SetDefaults()
        {
			NPC.width = 72;
            NPC.height = 72;
			
			NPC.boss = true;
			NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Mobs/PrimordialWormDead")
			{
				Volume = 1f,
				Pitch = -0.25f
			};
			NPC.damage = 30;
			NPC.defense = 8;
			NPC.lifeMax = 3320;
			NPC.value = Item.buyPrice(0, 2, 36, 0);
			NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
			NPC.behindTiles = true;
			NPC.chaseable = true;
			NPC.CanBeChasedBy(this, true);
			NPC.netUpdate = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 25;
			NPC.Opacity = 0;
			NPC.scale = 1;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<PrimordialWormBag>()));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PrimordialThrowingAx>(), 6));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PrimordialSword>(), 2));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PrimordialBow>(), 3));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PrimordialLance>(), 2));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PrimordialStaff>(), 2));
        }

		public override bool CheckActive() 
		{
			return despawnCounter > 0 && DeathAnimation <= 0;
		}
		
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.85f * balance);
            NPC.damage = (int)(NPC.damage * 0.8f);
        }
		
		private int scanPlayerforDirection;
		
		private float spinCircumference = 550f;
		
		private int maxCharges = 4;
		
		private float chargeSpeed = 7f;
		
		private float chargeDuration = 60f;
		
		private float velMultiplier = 1;
		
		private float emitFlame;
		
		private float flameRot;
		
		private float flamesEmitted;
		
		private int chosenDirection;
		
		private int FTWTempestCounter;

		public override void OnSpawn(IEntitySource source)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.realLife = NPC.whoAmI;
				int latestNPC = NPC.whoAmI;
				int maxLength = 44;
				for (int i = 0; i < maxLength; ++i)
				{
					int type = ((i < 0 || i >= maxLength - 1) ? ModContent.NPCType<PrimordialWormTail>() : (i % 3 != 0) ? ModContent.NPCType<PrimordialWormBody>() : ModContent.NPCType<PrimordialWormBodyAlt>());
					latestNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)(NPC.Center.Y + NPC.height *  i), type, NPC.whoAmI, 0f, latestNPC);
					Main.npc[(int)latestNPC].realLife = NPC.whoAmI;
					Main.npc[(int)latestNPC].ai[3] = NPC.whoAmI;
				}
				NPC.netUpdate = true;
			}
			
			chosenDirection = Main.rand.NextBool(2) ? -1 : 1;
			CurrentAttackType = -1;
		}
		
        public override bool PreAI()
        {
			Player target = Main.player[NPC.target];
			Lighting.AddLight(NPC.Center, 0.3f * NPC.Opacity, 0f * NPC.Opacity, 0f * NPC.Opacity);
			if (CurrentAttackType == -1)
			{
				if (AttackTimer++ == 0)
				{
					NPC.velocity.Y = -0.5f;
				}
				if (AttackTimer >= 1)
				{
					NPC.velocity.Y += 0.14f;
					if (AttackTimer >= 2)
					{
						CurrentAttackType = JustCrawling;
						AttackTimer = -120;
						NPC.netUpdate = true;
					}
				}
			}
					
			if (NPC.target < 0 || NPC.target == 255 || target.dead)
			{
				NPC.TargetClosest();
			}
			
			int minTilePosX = (int)(NPC.position.X / 16.0) - 1;
            int maxTilePosX = (int)((NPC.position.X + NPC.width) / 16.0) + 2;
            int minTilePosY = (int)(NPC.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((NPC.position.Y + NPC.height) / 16.0) + 2;
            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;
			
			bool collision = false;
			for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    if (Main.tile[i, j] != null && (Main.tile[i, j].HasUnactuatedTile && (Main.tileSolid[(int)Main.tile[i, j].TileType] || Main.tileSolidTop[(int)Main.tile[i, j].TileType] && (int)Main.tile[i, j].TileFrameY == 0) || (int)Main.tile[i, j].LiquidAmount > 160))
                    {
                        Vector2 vector2;
						if ((AttackTimer > 360 && AttackTimer < 570 && CurrentAttackType == 0) || (CurrentAttackType == ChargeToPlayer))
						{
							vector2.X = (float)(i * 17);
							vector2.Y = (float)(j * 17);
						}
						else if (CurrentAttackType == RainingIcicles)
						{
							vector2.X = (float)(i * 18);
							vector2.Y = (float)(j * 18);
						}
						else if (despawnCounter > 0 && !target.ZoneUnderworldHeight)
						{
							vector2.X = (float)(i * 20);
							vector2.Y = (float)(j * 20);
						}
						else if (target.ZoneUnderworldHeight)
						{
							vector2.X = (float)(i * 12);
							vector2.Y = (float)(j * 12);
						}
						else
						{
							vector2.X = (float)(i * 16);
							vector2.Y = (float)(j * 16);
						}
                        
                        if (NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && (NPC.position.Y + NPC.height > (double)vector2.Y && NPC.position.Y < vector2.Y + 16.0))
                        {
                            collision = true;
                            if (Main.rand.Next(160) == 0 && Main.tile[i, j].HasUnactuatedTile)
                                WorldGen.KillTile(i, j, true, true, false);
                        }
                    }
                }
            }
			if (!collision)
			{
				NPC.localAI[1] = 1f;
				Rectangle rectangle1 = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
                int maxDistance = 800;
                bool playerCollision = true;
				if (NPC.position.Y > Main.player[NPC.target].position.Y)
				{
					for (int i = 0; i < 255; ++i)
					{
						if (Main.player[i].active)
						{
							Rectangle rectangle2 = new Rectangle((int)Main.player[i].position.X - maxDistance, (int)Main.player[i].position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
							if (rectangle1.Intersects(rectangle2))
							{
								playerCollision = false;
								break;
							}
						}
					}
					if (playerCollision)
						collision = true;
				}
			}
			else
			{
				NPC.localAI[1] = 0f;
			}
			
			float num17 = 19f;
			float speed = 0.14f;
			float turnSpeed = 0.2f;
			Vector2 vector3 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num21 = target.position.X + (float)target.width / 2;
			float num22 = target.position.Y + (float)target.height / 2;
			num21 = (int)(num21 / 16f) * 16;
			num22 = (int)(num22 / 16f) * 16;
			vector3.X = (int)(vector3.X / 16f) * 16;
			vector3.Y = (int)(vector3.Y / 16f) * 16;
			num21 -= vector3.X;
			num22 -= vector3.Y;
			float num23 = (float)Math.Sqrt((double)(num21 * num21 + num22 * num22));
			
			if (CurrentAttackType == JustCrawling || CurrentAttackType == RainingIcicles)
			{
				if (!collision)
				{
					NPC.TargetClosest();
					NPC.velocity.Y = NPC.velocity.Y + 0.15f;
					if (NPC.velocity.Y > num17)
					{
						NPC.velocity.Y = num17;
					}
					if ((double)(Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < (double)num17 * 0.4)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X = NPC.velocity.X - speed * 1.1f;
						}
						else
						{
							NPC.velocity.X = NPC.velocity.X + speed * 1.1f;
						}
					}
					else if (NPC.velocity.Y == num17)
					{
						if (NPC.velocity.X < num21)
						{
							NPC.velocity.X = NPC.velocity.X + speed;
						}
						else if (NPC.velocity.X > num21)
						{
							NPC.velocity.X = NPC.velocity.X - speed;
						}
					}
					else if (NPC.velocity.Y > 4f)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X = NPC.velocity.X + speed * 0.9f;
						}
						else
						{
							NPC.velocity.X = NPC.velocity.X - speed * 0.9f;
						}
					}
				}
				else
				{
					num23 = (float)Math.Sqrt((double)(num21 * num21 + num22 * num22));
					float num25 = Math.Abs(num21);
					float num26 = Math.Abs(num22);
					float num27 = num17 / num23;
					num21 *= num27;
					num22 *= num27;
					if (((NPC.velocity.X > 0f && num21 > 0f) || (NPC.velocity.X < 0f && num21 < 0f)) && ((NPC.velocity.Y > 0f && num22 > 0f) || (NPC.velocity.Y < 0f && num22 < 0f)))
					{
						if (NPC.velocity.X < num21)
						{
							NPC.velocity.X = NPC.velocity.X + turnSpeed;
						}
						else if (NPC.velocity.X > num21)
						{
							NPC.velocity.X = NPC.velocity.X - turnSpeed;
						}
						if (NPC.velocity.Y < num22)
						{
							NPC.velocity.Y = NPC.velocity.Y + turnSpeed;
						}
						else if (NPC.velocity.Y > num22)
						{
							NPC.velocity.Y = NPC.velocity.Y - turnSpeed;
						}
					}
					if ((NPC.velocity.X > 0f && num21 > 0f) || (NPC.velocity.X < 0f && num21 < 0f) || (NPC.velocity.Y > 0f && num22 > 0f) || (NPC.velocity.Y < 0f && num22 < 0f))
					{
						if (NPC.velocity.X < num21)
						{
							NPC.velocity.X = NPC.velocity.X + speed;
						}
						else if (NPC.velocity.X > num21)
						{
							NPC.velocity.X = NPC.velocity.X - speed;
						}
						if (NPC.velocity.Y < num22)
						{
							NPC.velocity.Y = NPC.velocity.Y + speed;
						}
						else if (NPC.velocity.Y > num22)
						{
							NPC.velocity.Y = NPC.velocity.Y - speed;
						}
						if ((double)Math.Abs(num22) < (double)num17 * 0.2 && ((NPC.velocity.X > 0f && num21 < 0f) || (NPC.velocity.X < 0f && num21 > 0f)))
						{
							if (NPC.velocity.Y > 0f)
							{
								NPC.velocity.Y = NPC.velocity.Y + speed * 2f;
							}
							else
							{
								NPC.velocity.Y = NPC.velocity.Y - speed * 2f;
							}
						}
						if ((double)Math.Abs(num21) < (double)num17 * 0.2 && ((NPC.velocity.Y > 0f && num22 < 0f) || (NPC.velocity.Y < 0f && num22 > 0f)))
						{
							if (NPC.velocity.X > 0f)
							{
								NPC.velocity.X = NPC.velocity.X + speed * 2f;
							}
							else
							{
								NPC.velocity.X = NPC.velocity.X - speed * 2f;
							}
						}
					}
					else if (num25 > num26)
					{
						if (NPC.velocity.X < num21)
						{
							NPC.velocity.X = NPC.velocity.X + speed * 1.1f;
						}
						else if (NPC.velocity.X > num21)
						{
							NPC.velocity.X = NPC.velocity.X - speed * 1.1f;
						}
						if ((double)(Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < (double)num17 * 0.5)
						{
							if (NPC.velocity.Y > 0f)
							{
								NPC.velocity.Y = NPC.velocity.Y + speed;
							}
							else
							{
								NPC.velocity.Y = NPC.velocity.Y - speed;
							}
						}
					}
					else
					{
						if (NPC.velocity.Y < num22)
						{
							NPC.velocity.Y = NPC.velocity.Y + speed * 1.1f;
						}
						else if (NPC.velocity.Y > num22)
						{
							NPC.velocity.Y = NPC.velocity.Y - speed * 1.1f;
						}
						if ((double)(Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < (double)num17 * 0.5)
						{
							if (NPC.velocity.X > 0f)
							{
								NPC.velocity.X = NPC.velocity.X + speed;
							}
							else
							{
								NPC.velocity.X = NPC.velocity.X - speed;
							}
						}
					}
				}
				velMultiplier = 1f;
			}

			else if ((CurrentAttackType == ChargeToPlayer && AttackTimer > chargeDuration * 2f) || (CurrentAttackType == PrimordialWormBreath && AttackTimer <= 240) || (Main.getGoodWorld && CurrentAttackType == PrimordialWormBreath && AttackTimer > 315))
			{
				if (NPC.target < 0 || NPC.target == 255 || !target.dead)
				{
					NPC.TargetClosest(true);
				}
				float num3 = 0.3f;
				float num11 = 0.05f;
				Vector2 vector = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num117 = target.position.X + (float)(target.width / 2);
				float num118 = target.position.Y + (float)(target.height / 2);
				num3 = 0.4f;
				num117 = (int)(num117 / 8f) * 8;
				num118 = (int)(num118 / 8f) * 8;
				vector.X = (int)(vector.X / 8f) * 8;
				vector.Y = (int)(vector.Y / 8f) * 8;
				num117 -= vector.X;
				num118 -= vector.Y;
				float num19 = (float)Math.Sqrt(num117 * num117 + num118 * num118);
				float num20 = num19;
				NPC.velocity.X += num117 * 0.0005f;
				NPC.velocity.Y += num118 * 0.0005f;
				NPC.rotation = (float)Math.Atan2(num118, num117) - 1.57f;

				if (NPC.velocity.X >= chargeSpeed * 2.9f)
					NPC.velocity.X = chargeSpeed * 2.9f;
				if (NPC.velocity.Y > chargeSpeed * 2.9f)
					NPC.velocity.Y = chargeSpeed * 2.9f;
				if (NPC.velocity.X <= -chargeSpeed * 2.9f)
					NPC.velocity.X = -chargeSpeed * 2.9f;
				if (NPC.velocity.Y < -chargeSpeed * 2.9f)
					NPC.velocity.Y = -chargeSpeed * 2.9f;

				if (NPC.collideX)
				{
					NPC.velocity.X = NPC.oldVelocity.X * (0f - num3);
					if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
					{
						NPC.velocity.X = 0.3f;
					}
					if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
					{
						NPC.velocity.X = -0.3f;
					}
				}
				if (NPC.collideY)
				{
					NPC.velocity.Y = NPC.oldVelocity.Y * (0f - num3);
					if (NPC.direction == -1 && NPC.velocity.Y > 0f && NPC.velocity.Y < 2f)
					{
						NPC.velocity.Y = 0.3f;
					}
					if (NPC.direction == 1 && NPC.velocity.Y < 0f && NPC.velocity.Y > -2f)
					{
						NPC.velocity.Y = -0.3f;
					}
					if (NPC.velocity.Y < num118)
					{
						NPC.velocity.Y += num11;
					}
					if (NPC.velocity.Y > num118)
					{
						NPC.velocity.Y -= num11;
					}
				}
			}
			
			NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;
			NPC.velocity *= velMultiplier;
			
			if (collision)
			{
				if (NPC.localAI[0] != 1f) {
					NPC.netUpdate = true;
				}
				NPC.localAI[0] = 1f;
			}
			else
			{
				if (NPC.localAI[0] != 0f) {
					NPC.netUpdate = true;
				}
				NPC.localAI[0] = 0f;
			}
			
			if ((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f))
			{
				NPC.netUpdate = true;
			}
			
			if (despawnCounter <= 0)
			{
				if (CurrentAttackType == JustCrawling) {
					JustBeingAGenericWorm();
				}
				if (CurrentAttackType == ChargeToPlayer) {
					PrimordialWormCharging();
				}
				if (CurrentAttackType == ArcticBlasts) {
					ArcticBlastAttack();
				}
				if (CurrentAttackType == PrimordialWormBreath) {
					PrimordialWormBreathAttack();
				}
				if (CurrentAttackType == RainingIcicles){
					RainingIciclesAttack();
				}
			}
			
			if (DeathAnimation >= 1)
			{
				DeathAnimation++;
				PrimordialWormDeathSequence();
			}
			
			//Retreats downwards if player target is dead
			if (!target.dead && !target.ZoneUnderworldHeight && despawnCounter < 200) {
				despawnCounter = 0;
			}
			else {
				despawnCounter++;
			}
			
			if (despawnCounter >= 200) {
				NPC.Opacity -= 0.05f;
			}
			else
			{
				if (NPC.Opacity < 1)
					NPC.Opacity += 0.1f;
				if (NPC.Opacity > 1)
					NPC.Opacity = 1;
			}
			
			if (despawnCounter >= 240) {
				NPC.active = false;
			}
			
			if (despawnCounter > 0 && DeathAnimation > 0)
			{
				CurrentAttackType = -1;
				AttackTimer = -180;
				
				if (target.ZoneUnderworldHeight) {
					NPC.velocity.Y += -0.3f;
				}
				else {
					NPC.velocity.Y += -0.2f;
				}
				
				NPC.netUpdate = true;
				velMultiplier = 1;
			}
			return false;
        }

		public void JustBeingAGenericWorm()
		{
			Player target = Main.player[NPC.target];
			
			if (AttackTimer < 600) {
				AttackTimer++;
			}
			
			if (AttackTimer == 360)
			{
			}
			
			if (AttackTimer >= 540)
			{
				NPC.TargetClosest();
			
				if (NPC.localAI[0] != 1f) {
					NPC.netUpdate = true;
				}
				NPC.localAI[0] = 1f;
			}
			
			if (AttackTimer >= 600 && (Vector2.Distance(NPC.position, target.position) <= spinCircumference * 1.5f) && NPC.localAI[0] == 1f) {
				CurrentAttackType = ChargeToPlayer;
			}
			
			FTWTempestCounter = 0;
		}

		public virtual void PrimordialWormCharging()
		{
			Player target = Main.player[NPC.target];

			AttackTimer++;
			
			if (AttackTimer >= chargeDuration * 2.70f) {
				AttackTimer = 0;
			}
			
			if (AttackTimer == 1f)
			{
				SoundSlotNPC = SoundEngine.PlaySound(SoundID.Roar, NPC.Center).ToFloat();
				if (!SoundEngine.TryGetActiveSound(SlotId.FromFloat(SoundSlotNPC), out var sound))
					SoundSlotNPC = SlotId.Invalid.ToFloat();
				else
					sound.Position = NPC.Center;

				Vector2 vector36 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num335 = target.position.X + (float)(target.width / 2) - vector36.X;
				float num336 = target.position.Y + (float)(target.height / 2) - vector36.Y;
				float num337 = (float)Math.Sqrt(num335 * num335 + num336 * num336);
				float num338 = num337;
				num337 = chargeSpeed / num337;
				num335 *= num337;
				num336 *= num337;
				NPC.velocity.X = num335 * 3.5f;
				NPC.velocity.Y = num336 * 3.5f;
				NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
				NPC.netUpdate = true;
					
				AttackTypeCounter++;
				velMultiplier = 1f;
			}
			
			if (AttackTimer <= chargeDuration * 1.35f)
			{
				velMultiplier *= 0.9998f;
				NPC.defDamage = 8;
			}
			else {
				NPC.defDamage = 0;
			}
			
			if (AttackTimer > chargeDuration && AttackTypeCounter >= maxCharges)
			{
				switch (Main.rand.Next(3))
				{
					case 0:
					{
						CurrentAttackType = ArcticBlasts;
						
						if (target.Center.X < NPC.Center.X)
							scanPlayerforDirection = -1;
						else if (target.Center.X >= NPC.Center.X)
							scanPlayerforDirection = 1;
					}
						break;
					case 1:
						CurrentAttackType = PrimordialWormBreath;
						break;
					default:
						CurrentAttackType = RainingIcicles;
						break;
				}
				
				AttackTimer = 0;
				AttackTypeCounter = 0;
			}
		}
		
		public void ArcticBlastAttack()
		{
			NPC.netUpdate = true;
			NPC.velocity = Vector2.Normalize(NPC.velocity) * chargeSpeed * 1.75f;
			NPC.velocity += NPC.velocity.RotatedBy(MathHelper.PiOver2 * scanPlayerforDirection) * NPC.velocity.Length() / spinCircumference;
			
			if (Main.getGoodWorld && (double)NPC.life < (double)NPC.lifeMax * 0.50) {
				FTWTempestCounter++;
			}
			
			if (FTWTempestCounter == 60)
			{
				int tempest = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(spinCircumference * scanPlayerforDirection, 0).RotatedBy(NPC.rotation), Vector2.Zero, 259, 30, 3, Main.myPlayer);
					Main.projectile[tempest].ai[1] = (int)NPC.whoAmI;
					
				FTWTempestCounter = 65;
			}
		}
		
		public void PrimordialWormBreathAttack()
		{
			Player target = Main.player[NPC.target];
			int maxFlameAttacks = 3;
			
			AttackTimer++;
			NPC.netUpdate = true;
			
			float idealSpeed = MathHelper.Lerp(2.5f, 0.5f, Utils.GetLerpValue(12f, 120f, (float)AttackTimer - 300, true));
			if (NPC.velocity.Length() != idealSpeed)
			{
				NPC.velocity = Utils.SafeNormalize(NPC.velocity, Vector2.UnitY) * MathHelper.Lerp(NPC.velocity.Length(), idealSpeed, 0.04f);
			}
			
			int expertLifeTime = (Main.expertMode ? 15 : 0);
			int flameLifeTime = 35;
			
			if ((double)NPC.life < (double)NPC.lifeMax * 0.75) {
				flameLifeTime = 40;
			}
			if ((double)NPC.life < (double)NPC.lifeMax * 0.50) {
				flameLifeTime = 45;
			}
			if ((double)NPC.life < (double)NPC.lifeMax * 0.25) {
				flameLifeTime = 50;
			}
			
			if (AttackTimer > 200 && AttackTimer <= 300)
			{
				flameRot = 0;
				if (velMultiplier > 0.5f);
				velMultiplier *= 0.999f;
				if (Main.rand.NextFloat() <= 0.5f)
				{
					float num627 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
					float num628 = Main.rand.NextFloat();
					Vector2 position7 = NPC.Center + new Vector2(0, -50).RotatedBy(NPC.rotation) + num627.ToRotationVector2() * (100f + 100f * num628);
					Vector2 vector154 = (num627 + (float)Math.PI).ToRotationVector2() * (8f + 8f * Main.rand.NextFloat() + 8f * num628);
				}
			}
			else if (AttackTimer > 300)
			{	
				velMultiplier = 0.85f;

				float moveSpeed = MathHelper.Lerp(2.5f, 0.7f, Utils.GetLerpValue(12f, 120f, (float)AttackTimer - 300, true));
				if (NPC.velocity.Length() != moveSpeed)
				{
					NPC.velocity = Utils.SafeNormalize(NPC.velocity, Vector2.UnitY) * MathHelper.Lerp(NPC.velocity.Length(), moveSpeed, 0.04f);
				}
			
				emitFlame++;
				if (emitFlame >= 6)
				{
					flameRot += 2;
					
					Vector2 offset = new Vector2(0, -10);
					int flame = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + offset.RotatedBy(NPC.rotation), offset.RotatedBy(NPC.rotation + MathHelper.ToRadians(flameRot)), 259, 20, 3, Main.myPlayer);
					
					flame = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + offset.RotatedBy(NPC.rotation), offset.RotatedBy(NPC.rotation + MathHelper.ToRadians(-flameRot)), 259, 20, 3, Main.myPlayer);
						
					emitFlame = 0;
					flamesEmitted++;
				}
				
				if (flamesEmitted >= 40)
				{
					flameRot = 0;
					flamesEmitted = 0;
					velMultiplier = 1;
					
					AttackTimer = 60;
					AttackTypeCounter++;
				}
			}
			
			if (AttackTimer == 300)
			{
			}
				
			if (AttackTypeCounter >= maxFlameAttacks) {
				ResetAI();
			}
		}
		
		public void RainingIciclesAttack()
		{
			Player target = Main.player[NPC.target];
			NPC.netUpdate = true;
			
			AttackTimer++;
			
			int expertIcicles = (Main.expertMode ? 8 : 0);
			int maxIcicles = 20;
			int FTWIcicles = (Main.getGoodWorld ? 15 : 0);
			int cooldownReduction = 0;
			int FTWCooldownReduction = (Main.getGoodWorld ? 5 : 0);
			
			if ((double)NPC.life < (double)NPC.lifeMax * 0.75)
			{
				expertIcicles = (Main.expertMode ? 10 : 0);
				maxIcicles = 22;
				cooldownReduction = 0;
			}
			if ((double)NPC.life < (double)NPC.lifeMax * 0.50)
			{
				expertIcicles = (Main.expertMode ? 14 : 0);
				maxIcicles = 24;
				cooldownReduction = 1;
			}
			if ((double)NPC.life < (double)NPC.lifeMax * 0.25)
			{
				expertIcicles = (Main.expertMode ? 18 : 0);
				maxIcicles = 26;
				cooldownReduction = 2;
			}
			
			if (AttackTypeCounter <= maxIcicles + FTWIcicles)
			{
				if (AttackTimer >= 20 - cooldownReduction + FTWCooldownReduction)
				{
					for (int i = 0; i < (Main.getGoodWorld ? 2 : 1); i++)
					{
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
                     	    direction.Normalize();
                     	    direction.X *= 15f;
                     	    direction.Y *= 15f;
                     	    
                     	    float A = (float)Main.rand.Next(-2, 2) * 1f;
                     	    float B = (float)Main.rand.Next(-2, 2) * 1f;
                            int damage = Main.expertMode ? 12 : 14;
                     	    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, 259, damage, 1, Main.myPlayer, 0, 0);
                        }
						AttackTypeCounter++;
						AttackTimer = 0;
					}
				}
			}
			else
			{
				if (AttackTimer >= 100) {
					ResetAI();
				}
			}
		}
		
		public void ResetAI()
		{
			CurrentAttackType = JustCrawling;
			AttackTimer = -180;
			AttackTypeCounter = 0;

			NPC.netUpdate = true;
			velMultiplier = 1;
		}
		
		public void PrimordialWormDeathSequence()
		{
			Player target = Main.player[NPC.target];
			
			CurrentAttackType = -1;
			AttackTimer = -180;
			AttackTypeCounter = 0;
			
			NPC.Opacity = 1;
			NPC.netUpdate = true;
			velMultiplier = 1;
			
			float idealSpeed = MathHelper.Lerp(2.1f, 0.7f, Utils.GetLerpValue(12f, 120f, (float)DeathAnimation, true));
			if (NPC.velocity.Length() != idealSpeed)
			{
				NPC.velocity = Utils.SafeNormalize(NPC.velocity, Vector2.UnitY) * MathHelper.Lerp(NPC.velocity.Length(), idealSpeed, 0.04f);
			}
			if (DeathAnimation > 0)
			{
				if (Main.rand.NextBool(20))
				{
					FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Mobs/PrimordialWormDead2"), (int)NPC.Center.X, (int)NPC.Center.Y, 8f, 0.25f);
					for (int i = 0; i < Main.rand.Next(1, 5); i++)
					{
						int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = Main.rand.NextFloat(0.5f, 2f);
						Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 8f);
					}
				}
			}
			if (DeathAnimation == 180)
			{
				target.ApplyDamageToNPC(NPC, 150, 0, 0, false);
				if (Main.netMode == NetmodeID.Server && NPC.whoAmI < Main.maxNPCs)
					NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
			}
		}
		
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life >= 1 && NPC.Opacity > 0)
            {
				for (int i = 0; i < Main.rand.Next(5, 10); i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(0.5f, 1f);
					Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 3f);
				}
			}
			
			if (NPC.life <= 1 && DeathAnimation < 180 && NPC.Opacity > 0)
            {
				NPC.life = 1;
				NPC.dontTakeDamage = true;
				NPC.damage = 0;
				NPC.netUpdate = true;
				
				if (DeathAnimation == 0)
				{
					DeathAnimation = 1;
				}
			}
        }
				
		public override bool CheckDead()
		{
			if (DeathAnimation < 180)
			{
				NPC.life = 1;
				return false;
			}
            Player player = Main.LocalPlayer;
			FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
			modPlayer.Shake += 12;
			ParticleManager.NewParticle<HollowCircle>(NPC.Center, Vector2.Zero, Color.Yellow, 1.6f, 0, 0);
            for (int i = 0; i < 50; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(NPC.Center, Main.rand.NextVector2Circular(30f, 30f), Color.Yellow, Main.rand.NextFloat(1f, 5f) * NPC.scale, 0, 0);
			}
			for (int i = 0; i < 30; i++)
			{
				ParticleManager.NewParticle<BloomCircle_FadingIn>(NPC.Center, new Vector2(Main.rand.NextFloat(-60, 60), Main.rand.NextFloat(-75f, -10f)), Color.Yellow, Main.rand.NextFloat(1f, 2f), 0, 0);
				ParticleManager.NewParticle<LineStreak_Long_Impact>(NPC.Center, new Vector2(Main.rand.NextFloat(-30f, 30f), Main.rand.NextFloat(-30f, 30f)), Color.Yellow, Main.rand.NextFloat(1f, 3f), 0, 0);
			}
			Vector2 goreVel = NPC.velocity;
			for (int i = 0; i < 2; i++)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormHeadGore").Type, NPC.scale);
			}
			
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormHeadGore2").Type, NPC.scale);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormHeadGore3").Type, NPC.scale);
			
			for (int i = 0; i < 16; i++)
			{
				int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(1f, 1.4f);
				Main.dust[dust].velocity *= Main.rand.NextFloat(1.5f, 4f);
			}
			return true;
		}
		
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1f;
            return null;
        }
    }
	
	public class PrimordialWormBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frozen] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
        }
		
        public override void SetDefaults()
        {
			NPC.width = 28;
            NPC.height = 28;
			
			NPC.boss = true;
			
			NPC.damage = 25;
			NPC.defense = 14;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.lifeMax = 3320;
			NPC.knockBackResist = 0.0f;
			NPC.lavaImmune = true;
            NPC.behindTiles = true;
			NPC.chaseable = true;
			NPC.CanBeChasedBy(this, true);
			NPC.dontCountMe = true;
			NPC.netUpdate = true;
			NPC.noTileCollide = true;
            NPC.noGravity = true;
			NPC.npcSlots = 2f;
			NPC.Opacity = 0;
			NPC.scale = 1;
			
			NPC.coldDamage = true;
        }
		
		public override bool CheckActive() 
		{
			return false;
		}
		
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.85f * balance);
            NPC.damage = (int)(NPC.damage * 0.8f);
        }
		
		private float DeathAnimation;
		
        public override bool PreAI()
        {
			Player target = Main.player[NPC.target];
			Lighting.AddLight(NPC.Center, 0.3f * NPC.Opacity, 0f * NPC.Opacity, 0f * NPC.Opacity);
            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];
            if (NPC.target < 0 || NPC.target == byte.MaxValue || target.dead)
                NPC.TargetClosest(true);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)NPC.ai[1]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }

            if (NPC.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - npcCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                // We calculate a new, correct distance.
                float dist = (length - (float)NPC.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
            }
			
			NPC wormHead = Main.npc[(int)NPC.ai[3]];
			NPC.Opacity = wormHead.Opacity;
			
			if (wormHead.ai[3] > 0)
				DeathAnimation++;
			if (DeathAnimation > 0)
			{
				if (Main.rand.NextBool(60))
				{
					for (int i = 0; i < Main.rand.Next(1, 5); i++)
					{
						int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = Main.rand.NextFloat(0.1f, 1f);
						Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 2f);
					}
				}
			}
			if (DeathAnimation == 179)
			{
				NPC.boss = false;
				
				Vector2 goreVel = NPC.velocity;
				for (int i = 0; i < 2; i++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormBodyGore").Type, NPC.scale);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormBodyGore2").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormBodyGore3").Type, NPC.scale);
				
				for (int i = 0; i < 11; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(1f, 2f);
					Main.dust[dust].velocity *= Main.rand.NextFloat(1.5f, 5f);
				}
			}
			
			return false;
        }

		public override void HitEffect(NPC.HitInfo hit)
        {
			if (NPC.life >= 1 && NPC.Opacity > 0)
            {
				for (int i = 0; i < Main.rand.Next(5, 10); i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(0.5f, 1f);
					Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 3f);
				}
			}

			NPC wormHead = Main.npc[(int)NPC.ai[3]];
			NPC.Opacity = wormHead.Opacity;
			
			if (NPC.life <= 1 && NPC.Opacity > 0)
            {
				NPC.life = 1;
				NPC.dontTakeDamage = true;
				NPC.damage = 0;
				NPC.netUpdate = true;

				if (wormHead.ai[3] == 0) {
					wormHead.ai[3] = 1;
				}
			}
        }
		
		public override bool CheckDead()
		{
			if (DeathAnimation < 180)
			{
				NPC.life = 1;
				return false;
			}
			return true;
		}
		
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }
	public class PrimordialWormBodyAlt : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frozen] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
        }
		
        public override void SetDefaults()
        {
			NPC.width = 28;
            NPC.height = 28;
			
			NPC.boss = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.damage = 25;
			NPC.defense = 100;
			NPC.lifeMax = 3320;
			NPC.knockBackResist = 0.0f;
			NPC.lavaImmune = true;
            NPC.behindTiles = true;
			NPC.chaseable = true;
			NPC.CanBeChasedBy(this, true);
			NPC.dontCountMe = true;
			NPC.netUpdate = true;
			NPC.noTileCollide = true;
            NPC.noGravity = true;
			NPC.npcSlots = 2f;
			NPC.Opacity = 0;
			NPC.scale = 1;
        }
		
		public override bool CheckActive() {
			return false;
		}
		
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.85f * balance);
            NPC.damage = (int)(NPC.damage * 0.8f);
        }
		
		private float DeathAnimation;
		
        public override bool PreAI()
        {
			Player target = Main.player[NPC.target];
			Lighting.AddLight(NPC.Center, 0.3f * NPC.Opacity, 0f * NPC.Opacity, 0f * NPC.Opacity);
            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];
            if (NPC.target < 0 || NPC.target == byte.MaxValue || target.dead)
                NPC.TargetClosest(true);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)NPC.ai[1]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }

            if (NPC.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - npcCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                // We calculate a new, correct distance.
                float dist = (length - (float)NPC.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
            }
			
			NPC wormHead = Main.npc[(int)NPC.ai[3]];
			NPC.Opacity = wormHead.Opacity;
			
			if (wormHead.ai[3] > 0)
				DeathAnimation++;
			if (DeathAnimation > 0)
			{
				if (Main.rand.NextBool(50))
				{
					for (int i = 0; i < Main.rand.Next(1, 5); i++)
					{
						int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = Main.rand.NextFloat(0.1f, 1f);
						Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 2f);
					}
				}
			}
			if (DeathAnimation == 179)
			{
				NPC.boss = false;
					
				Vector2 goreVel = NPC.velocity;
				for (int i = 0; i < 2; i++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormBodyAltGore").Type, NPC.scale);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormBodyGore2").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormBodyGore3").Type, NPC.scale);

				for (int i = 0; i < 11; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(1f, 2f);
					Main.dust[dust].velocity *= Main.rand.NextFloat(1.5f, 5f);
				}
			}
			return false;
        }
		
		public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life >= 1 && NPC.Opacity > 0)
            {
				for (int i = 0; i < Main.rand.Next(5, 10); i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(0.5f, 1f);
					Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 3f);
				}
			}
			
			NPC wormHead = Main.npc[(int)NPC.ai[3]];
			
			if (NPC.life <= 1 && NPC.Opacity > 0)
            {
				NPC.life = 1;
				NPC.dontTakeDamage = true;
				NPC.damage = 0;
				NPC.netUpdate = true;

				if (wormHead.ai[3] == 0) {
					wormHead.ai[3] = 1;
				}
			}
        }
		
		public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
		{
			modifiers.FinalDamage *= 0.1f;
		}
		
		public override bool CheckDead()
		{
			if (DeathAnimation < 180)
			{
				NPC.life = 1;
				return false;
			}
			return true;
		}
		
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }
	public class PrimordialWormTail : ModNPC
    {
		ref float AttackTimer => ref NPC.ai[0];
		
		float SoundSlotNPC;
		
        public override void SetStaticDefaults() 
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frozen] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
        }
		
        public override void SetDefaults()
        {
			NPC.width = 34;
            NPC.height = 34;
			NPC.lavaImmune = true;
			NPC.boss = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.damage = 30;
			NPC.lifeMax = 3320;
			NPC.defense = 8;
            NPC.knockBackResist = 0.0f;
            NPC.behindTiles = true;
			NPC.chaseable = true;
			NPC.CanBeChasedBy(this, true);
			NPC.dontCountMe = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.npcSlots = 2f;
            NPC.netUpdate = true;
			NPC.scale = 1;
        }
		
		public override bool CheckActive() {
			return false;
		}
		
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.85f * balance);
            NPC.damage = (int)(NPC.damage * 0.8f);
        }

		private int arcticBlastsFired;

		private bool iceRubbles;
		
		private int maxIceRubbles;
		
		private float DeathAnimation;
		
        public override bool PreAI()
        {   
			Lighting.AddLight(NPC.Center, 0.3f * NPC.Opacity, 0f * NPC.Opacity, 0f * NPC.Opacity);
			
			Player target = Main.player[NPC.target];
		
            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];
            if (NPC.target < 0 || NPC.target == byte.MaxValue || target.dead)
                NPC.TargetClosest(true);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)NPC.ai[1]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }

            if (NPC.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - npcCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - (float)NPC.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
            }

			if (Main.npc[(int)NPC.ai[3]].ai[0] == 2) {
				ArcticBlastAttack();
			}
			
			if (Main.npc[(int)NPC.ai[3]].ai[0] == 0 && Main.npc[(int)NPC.ai[3]].ai[1] > -60 && Main.npc[(int)NPC.ai[3]].ai[1] < 280 && Main.npc[(int)NPC.ai[3]].life <= (int)(NPC.lifeMax *  (Main.getGoodWorld ? 0.75f : 0.5f)) && !iceRubbles && NPC.ai[2] == 0 && Main.expertMode)
			{
				maxIceRubbles = Main.rand.Next(8, 10);
				iceRubbles = true;
				
				NPC.netUpdate = true;
			}
			
			if (Main.npc[(int)NPC.ai[3]].ai[1] >= 355)
			{
				iceRubbles = false;
				NPC.ai[2] = 0;
				
				NPC.netUpdate = true;
			}
				
			if (iceRubbles && Collision.CanHit(NPC.position, NPC.width, NPC.height, target.position, target.width, target.height))
			{
				AttackTimer++;
				if (AttackTimer >= 2)
				{
					if (Main.rand.NextBool(10))
					{
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
                     	    direction.Normalize();
                     	    direction.X *= 10f;
                     	    direction.Y *= 10f;
                     	    
                     	    float A = (float)Main.rand.Next(-10, 10) * 0.01f;
                     	    float B = (float)Main.rand.Next(-10, 10) * 0.01f;
                            int damage = Main.expertMode ? 12 : 14;
                     	    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, 259, damage, 1, Main.myPlayer, 0, 0);
                        }
					}
					
					AttackTimer = 0;
				}
				
				if (NPC.ai[2] >= maxIceRubbles)
				{
					NPC.ai[2] = 1;
					iceRubbles = false;
				}
			}
			
			NPC wormHead = Main.npc[(int)NPC.ai[3]];
			NPC.Opacity = wormHead.Opacity;
			
			if (wormHead.ai[3] > 0)
				DeathAnimation++;

			if (DeathAnimation == 1)
			{				
			}
				
			if (DeathAnimation > 0)
			{
				if (Main.rand.NextBool(40))
				{
					for (int i = 0; i < Main.rand.Next(1, 5); i++)
					{
						int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = Main.rand.NextFloat(0.1f, 1f);
						Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 2f);
					}
				}
			}

			if (DeathAnimation == 179)
            {
				NPC.boss = false;
				Vector2 goreVel = NPC.velocity;
				for (int i = 0; i < 2; i++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormTailGore").Type, NPC.scale);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormTailGore2").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVel, Mod.Find<ModGore>("PrimordialWormTailGore3").Type, NPC.scale);
			
				for (int i = 0; i < 12; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(1f, 2f);
					Main.dust[dust].velocity *= Main.rand.NextFloat(1.5f, 4f);
				}
			}
			
            return false;
        }
		
		public void ArcticBlastAttack()
		{
			Player target = Main.player[NPC.target];
			float minRotation = 0;
			float maxRotation = 30 - Vector2.Distance(NPC.position, target.position) * 0.02f;
			int expertBlast = (Main.expertMode ? 1 : 0);
			int FTWBlasts = (Main.getGoodWorld ? 3 : 0);
			int numBlasts = 5;
			int cooldownReduction = (Main.getGoodWorld ? 10 : 0);
			
			int totalBlasts = numBlasts + expertBlast + FTWBlasts;
			
			if ((double)NPC.life < (double)NPC.lifeMax * 0.75)
			{
				numBlasts = 6;
				cooldownReduction = (Main.getGoodWorld ? 15 : 5);
			}
			if ((double)NPC.life < (double)NPC.lifeMax * 0.50)
			{
				numBlasts = 7;
				cooldownReduction = (Main.getGoodWorld ? 20 : 10);
			}
			if ((double)NPC.life < (double)NPC.lifeMax * 0.25)
			{
				numBlasts = 8;
				cooldownReduction = (Main.getGoodWorld ? 25 : 15);
			}
							
			AttackTimer++;

			if (arcticBlastsFired < totalBlasts)
			{
				if (AttackTimer >= 40 - (cooldownReduction + expertBlast + FTWBlasts * 5))
				{
					Vector2 velocity = Vector2.Normalize(new Vector2((int)NPC.Center.X + 26 * NPC.spriteDirection, (int)NPC.Center.Y + 8) - target.Center);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2((int)NPC.Center.X + 26 * NPC.spriteDirection, (int)NPC.Center.Y + 8), -velocity.RotatedByRandom(MathHelper.ToRadians(Vector2.Distance(NPC.position, target.position) > 6000f ? minRotation : maxRotation)) * Vector2.Distance(NPC.position, target.position) * 0.024f, 259, 16, 3, Main.myPlayer);
					
					AttackTimer = 0;
					arcticBlastsFired++;
				}
			}
			
			else if (arcticBlastsFired == totalBlasts)
			{
				if (AttackTimer >= 90) {
					arcticBlastsFired++;
				}
			}
			
			else if (arcticBlastsFired >= totalBlasts)
			{
				Main.npc[(int)NPC.ai[3]].ai[0] = 0;
				Main.npc[(int)NPC.ai[3]].ai[1] = -180;
				Main.npc[(int)NPC.ai[3]].ai[2] = 0;
				
				AttackTimer = 0;
				arcticBlastsFired = 0;
			}
		}	
		
        public override void HitEffect(NPC.HitInfo hit)
        {
			hit.Damage += hit.Damage / 2;
			
            if (NPC.life >= 1 && NPC.Opacity > 0)
            {
				for (int i = 0; i < Main.rand.Next(5, 10); i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 222);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(0.5f, 1f);
					Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 3f);
				}
			}
			
			NPC wormHead = Main.npc[(int)NPC.ai[3]];
			
			if (NPC.life <= 1 && NPC.Opacity > 0)
            {
				NPC.life = 1;
				NPC.dontTakeDamage = true;
				NPC.damage = 0;
				NPC.netUpdate = true;

				if (wormHead.ai[3] == 0) {
					wormHead.ai[3] = 1;
				}
			}
        }
		
		public override bool CheckDead()
		{
			if (DeathAnimation < 180)
			{
				NPC.life = 1;
				return false;
			}
			return true;
		}

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }
}
