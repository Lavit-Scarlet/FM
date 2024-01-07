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
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Ranged.Bows
{
    public class ElectroBowProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 2;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.extraUpdates = 4;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<Electric>(), 120);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 2)
            {
                Color ColorDV = new Color(216, 246, 253); ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(Projectile.scale * 0.4f, Projectile.scale * 1.6f);//длина, ширина
                Scale *= Mult;
                Mult *= 1f - Projectile.alpha / 255f;
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                color *= Mult; 
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
            }
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
            /*Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 2f)
            {
				Player player = Main.player[Projectile.owner];
				Projectile.ai[0] -= 1.1f;
				if (Projectile.localAI[1] == 0f)
				{
					Projectile.localAI[1] = 1f;
                    for (float j = 0.25f; j <= 0.75f; j += 0.25f)
                    {
                        for (int i = 0; i < 360; i += 10)
                        {
                            Vector2 circular = new Vector2(2 + 4 * j, 0).RotatedBy(MathHelper.ToRadians(i));
                            circular.X *= 0.55f;
                            circular = circular.RotatedBy(Projectile.velocity.ToRotation());
                            Vector2 fromCenter = Projectile.velocity.SafeNormalize(Vector2.Zero) * 32 * j;
                            Dust dust = Dust.NewDustDirect(Projectile.Center + Projectile.velocity * j + circular - new Vector2(5) + fromCenter, 0, 0, 206);
                            dust.velocity = 0.4f * circular + fromCenter;
                            dust.noGravity = true;
                            dust.fadeIn = 0.1f;
                            dust.scale *= 1f;
                        }
                    }
				}
			}	*/			
		}
		public override void Kill(int timeLeft)
		{
			FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/electricHit0"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .4f, .20f);
			for (int i = 0; i < 30; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 226, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
		}
    }
}