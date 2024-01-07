using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using FM.Content.Items.Materials;
using static Terraria.ModLoader.ModContent;
using FM.Content.Buffs.Debuff;

namespace FM.Content.NPCs.FlanricMobs
{
    public class FlanricSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flanric Slime");
        }
		public override void SetDefaults()
		{
			NPC.aiStyle = 1;
            NPC.lifeMax = 100;  
            NPC.damage = 40; 
            NPC.defense = 10;  
            NPC.knockBackResist = 0.4f;
            NPC.width = 44;
            NPC.height = 32;
            AnimationType = NPCID.BlueSlime;
			Main.npcFrameCount[NPC.type] = 2;  
            NPC.value = Item.buyPrice(0, 0, 12, 0);
			NPC.alpha = 0;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<FlanricCrystal>(), 4));
            npcLoot.Add(ItemDropRule.Common(ItemType<ScarletFragment>(), 1, 0, 2));
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), Main.rand.Next(60, 180));
        }
        public override bool PreAI()
        {
            NPC.TargetClosest(true);
            return true;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 20; i++)
                {
                    int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                    Main.dust[num].velocity *= 4f;
				    Main.dust[num].noGravity = true;
                    Main.dust[num].scale = 1.2f;
                }
            }
            for (int i = 0; i < 6; i++)
            {
                int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) 
		{
            if(Main.hardMode)
            {
                return SpawnCondition.OverworldDaySlime.Chance * 0.34f;//Cavern = пешеры //Underworld = АД //Crimson = да //OverworldNightMonster = ночь // OverworldDaySlime = день
            }
            return 0f;
		}
    }
}