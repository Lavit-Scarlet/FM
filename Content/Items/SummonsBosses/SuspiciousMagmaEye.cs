using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using FM.Content.Projectiles;
using FM.Content.NPCs.Bosses.HellGuardianBoss;

namespace FM.Content.Items.SummonsBosses
{
	public class SuspiciousMagmaEye : ModItem
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
			Item.value = 0;
			Item.rare = 4;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
		}
        public override bool CanUseItem(Player player)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<HellGuardian>()) && player.ZoneUnderworldHeight)
				return true;
			return false;
        }

        public override bool? UseItem(Player player)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<HellGuardian>());

			else if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 spawnPos = player.Center;
				int tries = 0;
				int maxtries = 300;
				while ((Vector2.Distance(spawnPos, player.Center) <= 200 || WorldGen.SolidTile((int)spawnPos.X / 16, (int)spawnPos.Y / 16) || WorldGen.SolidTile2((int)spawnPos.X / 16, (int)spawnPos.Y / 16) || WorldGen.SolidTile3((int)spawnPos.X / 16, (int)spawnPos.Y / 16)) && tries <= maxtries)
				{
					spawnPos = player.Center + Main.rand.NextVector2Circular(800, 800);
					tries++;
				}

				if (tries >= maxtries)
					return false;

			}
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return true;
		}
	}
}
