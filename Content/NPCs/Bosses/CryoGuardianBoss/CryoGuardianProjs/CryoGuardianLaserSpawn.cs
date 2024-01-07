using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using FM.Content.NPCs.Bosses.CryoGuardianBoss.CryoGuardianProjs;

namespace FM.Content.NPCs.Bosses.CryoGuardianBoss.CryoGuardianProjs
{
    public class CryoGuardianLaserSpawn : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.timeLeft = 350;
            Projectile.hide = true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity *= .96f;

            Player player = Main.player[Projectile.owner];
            if (Projectile.localAI[0] == 0)
            {
                Projectile.netUpdate = true;
                Projectile.alpha -= 4;
                if (Projectile.alpha <= 0)
                {
                    if (Main.myPlayer == Projectile.owner)
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<CryoGuardianLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Projectile.whoAmI);
                    Projectile.localAI[0] = 1;
                }
            }
            else
            {
                Projectile.localAI[0]++;
                if (Projectile.localAI[0] >= 60)
                {
                    Projectile.alpha += 12;
                    if (Projectile.alpha >= 255)
                        Projectile.Kill();
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }
    }
}