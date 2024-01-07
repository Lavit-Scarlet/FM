using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using FM.Content.Projectiles.Minions;

namespace FM.Content.Buffs.MinionBuffs
{
    public class CyberKnifeBuff : ModBuff
    {
        public override void SetStaticDefaults() 
        {
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<CyberKnifeProj>()] > 0) 
			{
				player.buffTime[buffIndex] = 18000;
			}
			else 
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
    }
}