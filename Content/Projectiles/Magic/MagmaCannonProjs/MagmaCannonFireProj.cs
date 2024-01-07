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

namespace FM.Content.Projectiles.Magic.MagmaCannonProjs
{
    public class MagmaCannonFireProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = 12;
            Projectile.friendly = true;
            Projectile.timeLeft = Main.rand.Next(120, 240);
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = false;
        }
		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.67f, 0.1f));
		}
        public override void AI()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
            }
            if (Projectile.ai[1] == 0f && Projectile.type >= 326 && Projectile.type <= 328)
            {
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
            }
            int num199 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 174, 0f, 0f, 100, default(Color), 1f);
            Dust expr_8946_cp_0 = Main.dust[num199];
            expr_8946_cp_0.position.X = expr_8946_cp_0.position.X - 2f;
            Dust expr_8964_cp_0 = Main.dust[num199];
            expr_8964_cp_0.position.Y = expr_8964_cp_0.position.Y + 2f;
            Main.dust[num199].scale += (float)Main.rand.Next(50) * 0.01f;
            Main.dust[num199].noGravity = true;
            Dust expr_89B7_cp_0 = Main.dust[num199];
            expr_89B7_cp_0.velocity.Y = expr_89B7_cp_0.velocity.Y - 2f;
            if (Main.rand.Next(2) == 0)
            {
                int num200 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 174, 0f, 0f, 100, default(Color), 1f);
                Dust expr_8A1E_cp_0 = Main.dust[num200];
                expr_8A1E_cp_0.position.X = expr_8A1E_cp_0.position.X - 2f;
                Dust expr_8A3C_cp_0 = Main.dust[num200];
                expr_8A3C_cp_0.position.Y = expr_8A3C_cp_0.position.Y + 2f;
                Main.dust[num200].scale += 0.3f + (float)Main.rand.Next(50) * 0.01f;
                Main.dust[num200].noGravity = true;
                Main.dust[num200].velocity *= 0.1f;
            }
            if ((double)Projectile.velocity.Y < 0.25 && (double)Projectile.velocity.Y > 0.15)
            {
                Projectile.velocity.X = Projectile.velocity.X * 0.8f;
            }
            Projectile.rotation = -Projectile.velocity.X * 0.05f;

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 5f)
            {
                Projectile.ai[0] = 5f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.97f;
                    if ((double)Projectile.velocity.X > -0.01 && (double)Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
                return;
            }
            if (Projectile.wet && !Projectile.lavaWet)
			{
				Projectile.Kill();
			}
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X *= .8f;
            return false;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.OnFire, 120, false);
			}
		}

    }
}