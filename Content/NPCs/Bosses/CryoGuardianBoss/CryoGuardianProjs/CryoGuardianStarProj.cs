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
using FM.Content.NPCs.Bosses.CryoGuardianBoss.CryoGuardianProjs;
using Terraria.Audio;

namespace FM.Content.NPCs.Bosses.CryoGuardianBoss.CryoGuardianProjs
{
    public class CryoGuardianStarProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;

			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;

            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
        }
        public override void Kill(int timeLeft)
        {
            Projectile.netUpdate = true;
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 20; k++)
            {
                int D = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 80, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            for (int i = 0; i < 8; ++i)
            {
                Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
                targetDir.Normalize();
                targetDir *= 8;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, targetDir.X, targetDir.Y, ProjectileType<CryoGuardianStarKillProj>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
            }
        }
        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * 0.2f;
            Projectile.velocity *= 0.98f;
        }
    }
}