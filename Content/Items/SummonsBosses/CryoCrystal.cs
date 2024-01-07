using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.NPCs.Bosses.CryoGuardianBoss;
using Microsoft.Xna.Framework;

namespace FM.Content.Items.SummonsBosses
{
    public class CryoCrystal : ModItem
    {
        public override void SetStaticDefaults() 
		{
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}
        public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 100;
			Item.rare = 3;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
		}
        public override bool CanUseItem(Player player)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<CryoGuardian>()) && player.ZoneSnow)
				return true;
			return false;
        }

        public override bool? UseItem(Player player)
		{
            int type = ModContent.NPCType<CryoGuardian>();

            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.NewNPC(new EntitySource_BossSpawn(player), (int)player.position.X + 400, (int)player.position.Y + 800, type);
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return true;
		}

    }
}