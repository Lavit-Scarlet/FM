using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FM.Globals
{
    public class FMBossDowned : ModSystem
    {
        public static bool downedCryoGuardian;
        public static bool downedArmagem;
        public static bool downedForestGuardian;
        public static bool downedPrimordialWorm;
        public static bool downedHellGuardian;

        public override void OnWorldLoad()
        {
            downedCryoGuardian = false;
            downedArmagem = false;
            downedForestGuardian = false;
            downedPrimordialWorm = false;
            downedHellGuardian = false;
        }

        public override void OnWorldUnload()
        {
            downedCryoGuardian = false;
            downedArmagem = false;
            downedForestGuardian = false;
            downedPrimordialWorm = false;
            downedHellGuardian = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            var downed = new List<string>();

            if (downedCryoGuardian)
                downed.Add("downedCryoGuardian");
            if (downedArmagem)
                downed.Add("downedArmagem");
            if (downedForestGuardian)
                downed.Add("downedForestGuardian");
            if (downedPrimordialWorm)
                downed.Add("downedPrimordialWorm");
            if (downedHellGuardian)
                downed.Add("downedHellGuardian");

            tag["downed"] = downed;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");

            downedCryoGuardian = downed.Contains("downedCryoGuardian");
            downedArmagem = downed.Contains("downedArmagem");
            downedForestGuardian = downed.Contains("downedForestGuardian");
            downedPrimordialWorm = downed.Contains("downedPrimordialWorm");
            downedHellGuardian = downed.Contains("downedHellGuardian");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = downedCryoGuardian;
            flags[1] = downedArmagem;
            flags[2] = downedForestGuardian;
            flags[3] = downedPrimordialWorm;
            flags[4] = downedHellGuardian;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedCryoGuardian = flags[0];
            downedArmagem = flags[1];
            downedForestGuardian = flags[2];
            downedPrimordialWorm = flags[3];
            downedHellGuardian = flags[4];
        }
    }
}