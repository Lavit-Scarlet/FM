using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using FM.Content.Items.Weapons.Ranged.Guns.GoblinInvention;
using FM.Content.Items.Ammo;
using FM.Content.Items.Weapons.Ranged.Bows;
using FM.Content.Items.Weapons.Magic.Books;

namespace FM.Globals
{
    public class FMShopNpc : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.GoblinTinkerer)
            {
                shop.Add<RTAuto>();
                shop.Add<RTSnaiper>();
                shop.Add<RTGrenadeLauncher>(Condition.InExpertMode);
                shop.Add<RTGrenade>(Condition.InExpertMode);
            }
            if (shop.NpcType == NPCID.Painter)
            {
                shop.Add<ElegantBow>(Condition.InExpertMode);
            }
            if (shop.NpcType == NPCID.Painter)
            {
                shop.Add<Stardom>(Condition.InExpertMode);
            }
        }
    }
}
