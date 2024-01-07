﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using FM.Content.NPCs.Bosses.Void;

namespace FM.Content.NPCs.Bosses.Void
{
    public class VoidScreenShaderData : ScreenShaderData
    {
        private int CalIndex;

        public VoidScreenShaderData(string passName)
            : base(passName)
        {
        }

        private void UpdateCalIndex()
        {
            int CalType = ModContent.NPCType<Void>();
            if (CalIndex >= 0 && Main.npc[CalIndex].active && Main.npc[CalIndex].type == CalType)
            {
                return;
            }
            CalIndex = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == CalType)
                {
                    CalIndex = i;
                    break;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (CalIndex == -1)
            {
                UpdateCalIndex();
                if (CalIndex == -1)
                    Filters.Scene["FM:Void"].Deactivate();
            }
        }

        public override void Apply()
        {
            UpdateCalIndex();
            if (CalIndex != -1)
            {
                UseTargetPosition(Main.npc[CalIndex].Center);
            }
            base.Apply();
        }
    }
}
