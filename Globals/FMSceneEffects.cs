using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using FM.Content.NPCs.Bosses.Armagem;
using FM.Content.NPCs.Bosses.Void;

namespace FM.Globals
{
    public class ArmagemSkyScene : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override void SpecialVisuals(Terraria.Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("FM:ArmagemSky", isActive);
            if (isActive)
                SkyManager.Instance.Activate("FM:ArmagemSky");
            else
                SkyManager.Instance.Deactivate("FM:ArmagemSky");
        }
        public override bool IsSceneEffectActive(Terraria.Player player)
        {
            return Terraria.NPC.AnyNPCs(ModContent.NPCType<ArmagemHead>());
        }
    }
    /*public class VoidSkyScene : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override void SpecialVisuals(Terraria.Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("FM:VoidSky", isActive);
            if (isActive)
                SkyManager.Instance.Activate("FM:VoidSky");
            else
                SkyManager.Instance.Deactivate("FM:VoidSky");
        }
        public override bool IsSceneEffectActive(Terraria.Player player)
        {
            return Terraria.NPC.AnyNPCs(ModContent.NPCType<Void>());
        }
    }*/
    public class VoidCloneBackgroundScene : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

        public override bool IsSceneEffectActive(Terraria.Player player) => Terraria.NPC.AnyNPCs(ModContent.NPCType<Void>());

        public override void SpecialVisuals(Terraria.Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("FM:Void", isActive);
        }
    }
}