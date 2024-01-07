using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;
using FM.Content.Tiles;
using FM.Globals;

namespace FM.Common
{
    class OreGeneration : ModSystem
    {
        public bool GeneratedElectroOre;

        public override void OnWorldLoad()
        {
            GeneratedElectroOre = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["genned"] = GeneratedElectroOre;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            GeneratedElectroOre = tag.GetBool("genned");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = GeneratedElectroOre;
            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            GeneratedElectroOre = flags[0];
        }
        
        public override void PreUpdateWorld()
        {
            if (Terraria.NPC.downedMechBoss1 && Terraria.NPC.downedMechBoss2 && Terraria.NPC.downedMechBoss3 && !GeneratedElectroOre)
            {
                for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX - 500);

    				int y = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 200);

                    WorldGen.OreRunner(x, y, WorldGen.genRand.Next(6, 12), WorldGen.genRand.Next(5, 6), (ushort)TileType<ElectroOreTile>());
                    
                }
                string Ekey = "Under the ground something sparkles";
                
                Color messageColor = new Color(90, 175, 235);
                if (Main.netMode == NetmodeID.Server)
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Ekey), messageColor);
                }
                else if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText(Language.GetTextValue(Ekey), messageColor);
                }
                GeneratedElectroOre = true;
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

			if (ShiniesIndex != -1) 
			{
				tasks.Insert(ShiniesIndex + 1, new PZOrePass("Project Zero Ores", 237.4298f));
			}
		}
    }
    public class PZOrePass : GenPass
	{
		public PZOrePass(string name, float loadWeight) : base(name, loadWeight) 
		{
		}
		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) 
		{
			progress.Message = "Project Zero Ores";

			for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++) 
			{
				int x = WorldGen.genRand.Next(0, Main.maxTilesX);

				int y = WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.maxTilesY);

				WorldGen.TileRunner(x, y, WorldGen.genRand.Next(12, 14), WorldGen.genRand.Next(2, 6), ModContent.TileType<DarkOreTile>());
			}
		}
	}
}