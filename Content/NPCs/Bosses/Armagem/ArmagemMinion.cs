using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using Terraria.UI;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.UI;
using System;
using FM.Content.NPCs.FlanricMobs;
using static Terraria.ModLoader.ModContent;
using FM.Content.NPCs.Bosses.Armagem.ArmagemProjs;

namespace FM.Content.NPCs.Bosses.Armagem
{
    public class ArmagemMinion : ModNPC
    {
		public override void SetDefaults()
		{
            NPC.CloneDefaults(NPCID.GiantCursedSkull);
            NPC.lifeMax = 640;  
            NPC.damage = 50; 
            NPC.defense = 14; 
            NPC.width = 30;
            NPC.height = 30;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
		}
        int delay;
        public override void AI()
        {
            delay++;
            Player player = Main.player[NPC.target];
            if (delay == 180)
            {
                SoundEngine.PlaySound(SoundID.Item33, NPC.position);
                Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
                direction.Normalize();
                direction.X *= 10f;
                direction.Y *= 10f;
                
                float A = (float)Main.rand.Next(-50, 50) * 0.01f;
                float B = (float)Main.rand.Next(-50, 50) * 0.01f;
                int damage = Main.expertMode ? 15 : 20;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileType<ArmagemMiniLaserProj>(), damage, 1, Main.myPlayer, 0, 0);
            }
            if (delay >= 180 && Main.netMode != NetmodeID.MultiplayerClient)
                delay = Main.rand.Next(-180, 180);
            
            if (!player.active || player.dead || Main.dayTime)
                NPC.active = false;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 20; i++)
                {
                    int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                    Main.dust[num].velocity *= 4f;
				    Main.dust[num].noGravity = true;
                    Main.dust[num].scale = 1.2f;
                }
            }
            for (int i = 0; i < 6; i++)
            {
                int num = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, 219);
                Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
            }
        }
    }
}