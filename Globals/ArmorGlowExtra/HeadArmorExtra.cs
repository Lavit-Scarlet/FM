using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace FM.Globals.ArmorGlowExtra
{
    public class HeadArmorExtra : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Terraria.Player drawPlayer = drawInfo.drawPlayer;
            if (OnHeadDraw.HeadDictionary.ContainsKey(drawPlayer.head))
            {
                Color color12 = drawInfo.colorArmorHead;
                bool glowmask = OnHeadDraw.HeadDictionary[drawPlayer.head].glowmask;
                if (glowmask)
                    color12 = Color.White;

                Texture2D texture = OnHeadDraw.HeadDictionary[drawPlayer.head].texture;
                int useShader = OnHeadDraw.HeadDictionary[drawPlayer.head].useShader;

                Vector2 Position = drawInfo.Position;
                Vector2 origin = new(drawPlayer.legFrame.Width * 0.5f, drawPlayer.legFrame.Height * 0.5f);
                Vector2 pos = new Vector2((int)(Position.X - Main.screenPosition.X - drawPlayer.bodyFrame.Width / 2 + drawPlayer.width / 2), (int)(Position.Y - Main.screenPosition.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f)) + drawPlayer.bodyPosition + new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2);

                Rectangle useFrame = drawPlayer.bodyFrame;
                int frameCount = OnHeadDraw.HeadDictionary[drawPlayer.head].cycleFrameCount;
                if (frameCount > 1)
                {
                    int f = (int)(Main.GlobalTimeWrappedHourly / 10 % (frameCount + (frameCount - 2)));
                    if (f >= frameCount)
                        f = frameCount - 1 - (f - frameCount);

                    useFrame.X = 40 * f;
                }

                DrawData data = new(texture, pos, useFrame, color12, 0f, origin, 1f, drawInfo.playerEffect, 0);
                if (useShader == -1)
                    data.shader = drawInfo.cHead;
                else
                    data.shader = drawPlayer.dye[useShader].dye;

                drawInfo.DrawDataCache.Add(data);
            }
        }
    }
    public class OnHeadDraw
    {
        public static Dictionary<int, OnHeadDraw> HeadDictionary = new();
        public Texture2D texture;
        public bool glowmask = true;
        public int useShader = -1;
        public int cycleFrameCount = 1;
        public OnHeadDraw(Texture2D texture, bool glowmask = true, int useShader = -1, int cycleFrameCount = 1)
        {
            this.texture = texture;
            this.glowmask = glowmask;
            this.useShader = useShader;
            this.cycleFrameCount = cycleFrameCount;
        }
        public static void RegisterHeads()
        {
            var immediate = AssetRequestMode.ImmediateLoad;
            Mod mod = FM.Instance;
            OnHeadDraw head = new(Request<Texture2D>("FM/Content/Items/Armor/PowerArmor/CyberRoninHelm_Head_Glow", immediate).Value);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "CyberRoninHelm", EquipType.Head), head);

            head = new(Request<Texture2D>("FM/Content/Items/Armor/PowerArmor/HolographicHelmet_Head_Glow", immediate).Value);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "HolographicHelmet", EquipType.Head), head);

            head = new(Request<Texture2D>("FM/Content/Items/Armor/PowerArmor/PowerVisor_Head_Glow", immediate).Value);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "PowerVisor", EquipType.Head), head);
            
            head = new(Request<Texture2D>("FM/Content/Items/Armor/PowerArmor/VisorOverlord_Head_Glow", immediate).Value);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "VisorOverlord", EquipType.Head), head);
        }
    }
}