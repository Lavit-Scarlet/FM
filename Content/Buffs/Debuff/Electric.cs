using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FM.Content.Buffs.Debuff
{
    public class Electric : ModBuff
    {
        public override void SetStaticDefaults() 
        {
			Main.debuff[Type] = true;  
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
        public override void Update(NPC npc, ref int buffIndex) 
        {
			npc.lifeRegen -= 40;
			
			for (int k = 0; k < 1; k++)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric);
				Main.dust[dust].scale = 0.5f;
			}
		}
    }
}