using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FM.Content.Buffs.Debuff
{
    public class FlanricDisease : ModBuff
    {
        public override void SetStaticDefaults() 
        {
			Main.debuff[Type] = true;  
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
        public override void Update(Player player, ref int buffIndex) 
        {
			player.lifeRegen -= 10;
			player.GetDamage(DamageClass.Generic) -= .08f;
            player.GetCritChance(DamageClass.Generic) -= 4;
			player.GetArmorPenetration(DamageClass.Generic) -= 6;
			
			for (int k = 0; k < 1; k++)
			{
				int dust = Dust.NewDust(player.position, player.width, player.height, 219);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void Update(NPC npc, ref int buffIndex) 
        {
			npc.lifeRegen -= 10;
			
			for (int k = 0; k < 1; k++)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 219);
				Main.dust[dust].noGravity = true;
			}
		}
    }
}