using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;

namespace FM.Globals
{
	public class FMGlobalNPC : GlobalNPC
	{
		public float DamageReduction = 0f;
		public float DRDown = 0f;
		public float DefDamageReduction = 0f;
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
		public override void ResetEffects(NPC npc)
		{
			DamageReduction = DefDamageReduction * (1f - DRDown);
			DRDown = 0f;
		}
		public override void SetDefaults(NPC npc)
		{
			DefDamageReduction = 0f;
			DamageReduction = 0f;
			DRDown = 0f;
		}
	}
}