using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using FM.Effects;
using FM.Core;
using static FM.Globals.FMNet;
using FM.Helpers;
using FM.Globals;
using FM.Effects.PrimitiveTrails;
using FM.Globals.ArmorGlowExtra;
using FM.CrossMod;
using FM.Content.NPCs.Bosses.Armagem;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Terraria.ModLoader.Config;
using Terraria.GameContent.Dyes;
using FM.Assets.Backgrounds.Skies;
using FM.Content.NPCs.Bosses.Void;

namespace FM
{
	public class FM : Mod
	{
		public const string Abbreviation = "MoR";
		public static FM Instance { get; private set; }

		public static BasicEffect basicEffect;
        //public static RenderTargetManager Targets;
        public static Effect GlowTrailShader;
		public static TrailManager TrailManager;

        public override void PostSetupContent()
        {
            WeakReferences.PerformModSupport();
            if (!Main.dedServ)
            {
                Main.QueueMainThreadAction(() =>
                {
                    OnHeadDraw.RegisterHeads();
                    OnLegDraw.RegisterLegs();
                    OnBodyDraw.RegisterBodies();
                });
            }
        }

		private List<ILoadable> _loadCache;

		public static Asset<Texture2D> GlowTrail;

		public FM()
        {
            Instance = this;
        }
		public override void Load()
		{
			LoadCache();
			if (!Main.dedServ)
			{
				TrailManager = new TrailManager(this);
                AdditiveCallManager.Load();
				GlowTrail = ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/GlowTrail", AssetRequestMode.ImmediateLoad);

                Filters.Scene["FM:ArmagemSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.44f, 0f, 0f).UseOpacity(0.34f), EffectPriority.VeryHigh);
                SkyManager.Instance["FM:ArmagemSky"] = new ArmagemSky();

                Filters.Scene["FM:Void"] = new Filter(new VoidScreenShaderData("FilterMiniTower").UseColor(.1f, .06f, .08f).UseOpacity(0.72f), EffectPriority.VeryHigh);
                SkyManager.Instance["FM:Void"] = new VoidSky();
			}

		}
		public override void Unload()
		{
			GlowTrail = null;
			TrailManager = null;
            AdditiveCallManager.Unload();
		}
        private void LoadCache()
        {
            _loadCache = new List<ILoadable>();

            foreach (Type type in Code.GetTypes())
            {
                if (!type.IsAbstract && type.GetInterfaces().Contains(typeof(ILoadable)))
                {
                    _loadCache.Add(Activator.CreateInstance(type) as ILoadable);
                }
            }

            _loadCache.Sort((x, y) => x.Priority > y.Priority ? 1 : -1);

            for (int i = 0; i < _loadCache.Count; ++i)
            {
                if (Main.dedServ && !_loadCache[i].LoadOnDedServer)
                {
                    continue;
                }

                _loadCache[i].Load();
            }
        }
		public ModPacket GetPacket(ModMessageType type, int capacity)
        {
            ModPacket packet = GetPacket(capacity + 1);
            packet.Write((byte)type);
            return packet;
        }
		public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
        {
            packet.Write(msg);

            for (int m = 0; m < param.Length; m++)
            {
                object obj = param[m];
                if (obj is bool boolean) packet.Write(boolean);
                else
                if (obj is byte @byte) packet.Write(@byte);
                else
                if (obj is int @int) packet.Write(@int);
                else
                if (obj is float single) packet.Write(single);
                else
                if (obj is Vector2 vector) { packet.Write(vector.X); packet.Write(vector.Y); }
            }
            return packet;
        }
		public override void HandlePacket(BinaryReader bb, int whoAmI)
		{
			ModMessageType msgType = (ModMessageType)bb.ReadByte();
            //byte player;
            switch (msgType)
			{
                case ModMessageType.SpawnTrail:
                    int projindex = bb.ReadInt32();

                    if (Main.netMode == NetmodeID.Server)
                    {
                        //If received by the server, send to all clients instead
                        WriteToPacket(Instance.GetPacket(), (byte)ModMessageType.SpawnTrail, projindex).Send();
                        break;
                    }

                    if (Main.projectile[projindex].ModProjectile is IManualTrailProjectile trailProj)
                        trailProj.DoTrailCreation(TrailManager);
                    break;
			}
		}
	}
	public class FMSystem : ModSystem
	{
        public static FMSystem Instance { get; private set; }
        public FMSystem()
        {
            Instance = this;
        }

        public static bool Silence;

        public override void PostUpdatePlayers()
        {
            Silence = false;
        }
		public override void Load()
		{
			FMDetours.Initialize();
			if (!Main.dedServ)
			{

			}
		}
		public override void PreUpdateItems()
        {
            if (Main.netMode != NetmodeID.Server)
                FM.TrailManager.UpdateTrails();
        }

	}
}