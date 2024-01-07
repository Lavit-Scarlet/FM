using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Globals;

namespace FM.Content.NPCs
{
    public class DummyNPC : ModNPC
    {
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
		}
		public override void SetDefaults()
		{
            NPC.CloneDefaults(488);
            NPC.lifeMax = 50000;
            NPC.aiStyle = -1;
            NPC.width = 28;
            NPC.height = 50;
            NPC.immortal = false;
            NPC.npcSlots = 0f;
            NPC.dontCountMe = true;
		}
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}
        public override void AI()
		{
			NPC.defense = 0;
			NPC.GetGlobalNPC<FMGlobalNPC>().DamageReduction = 0f;
			NPC.ai[1] += 1f;
			if (NPC.ai[1] % 60f == 0f)
            {
				NPC.ai[2] = NPC.ai[0];
				NPC.ai[0] = 0f;
				NPC.netUpdate = true;
			}
            if (NPC.ai[1] >= 600f)
            {
				NPC.ai[1] = 0f;
				NPC.ai[3] = NPC.localAI[0];
				NPC.localAI[0] = 0f;
				NPC.netUpdate = true;
			}
			if(NPC.ai[1] % 5f == 0f)
            {
				NPC.ai[0] += NPC.lifeMax - NPC.life;
				NPC.localAI[0] += NPC.lifeMax - NPC.life;
				NPC.lifeMax = 10000000;
				NPC.life = NPC.lifeMax;
			}
            if (NPC.localAI[1] == 0f)
            {
				NPC.localAI[1] = 1f;
				return;
			}
			NPC.velocity *= 0f;
			NPC.position = NPC.oldPosition;
		}
		public override bool CheckDead()
		{
			return false;
		}
	}
}

