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
using FM.Content.Projectiles.Ranged.Bows.FlanProjs;
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Ranged.Bows.FlanProjs
{
    public class Flan175Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            //Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
		}
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.position);
            Vector2 vector9 = Projectile.position;
            Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
            vector9 += value19 * 16f;
            for (int num257 = 0; num257 < 8; num257++)
            {
                int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, 219, 0f, 0f, 0, default(Color), 1f);
                Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
                Main.dust[newDust].velocity += value19 * 8f;
                Main.dust[newDust].velocity *= 0.5f;
                Main.dust[newDust].noGravity = true;
                Main.dust[newDust].noLight = true;
                vector9 -= value19 * 2f;
            }
        }
    }
}