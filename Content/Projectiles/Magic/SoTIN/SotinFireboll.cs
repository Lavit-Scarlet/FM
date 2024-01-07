using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Effects.PrimitiveTrails;
using System.IO;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using FM.Effects;
using System.Collections.Generic;
using ParticleLibrary;
using FM.Particles;
using FM.Content.Projectiles.Magic.SoTIN;

namespace FM.Content.Projectiles.Magic.SoTIN
{
    public class SotinFireboll : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
		}

        public override void AI()
        {
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 5)
				{
					Projectile.frame = 0;
				}
            }
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 174, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1f;
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 16;
            return true;
        }
		public override void Kill(int timeLeft)
		{
            Player player = Main.player[Projectile.owner];
			FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
			modPlayer.Shake += 8;
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Kaboom"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .80f);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 25; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 174, 0f, 0f, 0);
				Main.dust[num].velocity *= 6f;
                Main.dust[num].scale = 2f;
				Main.dust[num].noGravity = true;
			}
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SotinExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
			}
		}
    }
}