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
using FM.Content.Projectiles.Ranged.Bows.RubyBowProjs;

namespace FM.Content.Projectiles.Ranged.Bows.RubyBowProjs
{
    public class RubyBowProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), lightColor, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
		}
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.position);
            Vector2 vector9 = Projectile.position;
            Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
            vector9 += value19 * 16f;
            for (int num257 = 0; num257 < 15; num257++)
            {
                int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, 90, 0f, 0f, 0, default(Color), 1f);
                Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
                Main.dust[newDust].velocity -= value19 * 12f;
                Main.dust[newDust].velocity *= 0.7f;
                Main.dust[newDust].noGravity = true;
                Main.dust[newDust].noLight = true;
                vector9 -= value19 * 2f;
            }
            for (int r = 0; r < 10; r++)
			{
                int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 90, 0f, 0f, 0, new Color(), 1f);
				Main.dust[num2].velocity *= 4f;
				Main.dust[num2].noGravity = true;
                Main.dust[num2].noLight = true;
			}
			for (int p = 0; p < 3; ++p)
			{
				Vector2 targetDir = ((((float)Main.rand.Next(-180, 180)))).ToRotationVector2();
				targetDir.Normalize();
				targetDir *= 5;
				Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, targetDir, ProjectileType<RubyBowProj2>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
			}
        }
    }
}