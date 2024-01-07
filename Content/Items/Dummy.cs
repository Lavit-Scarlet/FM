using FM.Content.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FM.Content.Items
{
    public class Dummy : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.rare = ItemRarityID.Blue;
        }
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
        public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				if (player.altFunctionUse == 2)
				{
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						for (int i = 0; i < 200; i++)
						{
							if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<DummyNPC>())
							{
								NPC npc = Main.npc[i];
								npc.life = 0;
								npc.HitEffect(0, 10.0);
								npc.active = false;
							}
						}
					}
					else if (Main.netMode == NetmodeID.MultiplayerClient)
					{
						ModPacket packet = Mod.GetPacket(256);
						packet.Write(5);
						packet.Send(-1, -1);
					}
				}
				else if (NPC.CountNPCS(ModContent.NPCType<DummyNPC>()) < 50)
				{
					NPC.NewNPC(player.GetSource_ItemUse(Item, null), (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, ModContent.NPCType<DummyNPC>());
				}
			}
			return new bool?(true);
		}
	}
}
