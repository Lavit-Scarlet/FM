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
using ReLogic.Content;
using FM.Effects.PrimitiveTrails;
using System.IO;

namespace FM.Content.Projectiles.Ranged.Knifes.SpectralKnifeProjs
{
    public class SpectralKnifeTestProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
        }
        public override void SetDefaults()
        {
			Projectile.tileCollide = true;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 2;
			Projectile.localNPCHitCooldown = 60;
            Projectile.timeLeft = 600;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = ModContent.Request<Texture2D>("FM/Content/Projectiles/Ranged/Knifes/SpectralKnifeProjs/SpectralKnifeTestGlowProj", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 1)
            {
                Color ColorDV = new Color(0, 0, 255); ColorDV.A = 0;
                Color ColorDV2 = new Color(255, 255, 255); ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                Color color2 = ColorDV2 * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(Projectile.scale * 1.1f, Projectile.scale * 2.2f);//длина, ширина
                Vector2 Scale2 = new Vector2(Projectile.scale * 0.6f, Projectile.scale * 1.8f);//длина, ширина
                Scale *= Mult;
                Scale2 *= Mult;
                Mult *= 1f - Projectile.alpha / 255f;
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                color *= Mult; 
                color2 *= Mult; 
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color2, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale2, 0, 0f);
            }

            Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num123 = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num123 * Projectile.frame, value.Width, num123), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num123 / 2f), 1f, (SpriteEffects)0, 0);
            return false;
        }
        public override void AI()
        {
			Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0)
            {
				Vector2 velocity = new Vector2(0.1f * Math.Sign(Projectile.velocity.X), -5f);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, velocity, ProjectileType<SpectralKnifeDeadNpc>(), Projectile.damage, 0, Main.player[Projectile.owner].whoAmI);
            }
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 226, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
		}
    }
}