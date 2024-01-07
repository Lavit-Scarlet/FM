using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.NPCs.Bosses.PrimordialWorm;
using Microsoft.Xna.Framework;

namespace FM.Content.Items.SummonsBosses
{
    public class PrimordialController : ModItem
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
			Item.rare = 4;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
		}
        public override bool CanUseItem(Player player)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<PrimordialWormHead>()) && player.ZoneDesert)
				return true;
			return false;
        }

        public override bool? UseItem(Player player)
		{
            int type = ModContent.NPCType<PrimordialWormHead>();

            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.NewNPC(new EntitySource_BossSpawn(player), (int)player.position.X + 700, (int)player.position.Y + 2800, type);
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return true;
		}

    }
}