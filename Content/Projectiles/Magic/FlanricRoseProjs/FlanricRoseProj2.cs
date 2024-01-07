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
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;
using FM.Content.Projectiles.Magic;
using Terraria.GameContent.Drawing;
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Magic.FlanricRoseProjs
{
	public class FlanricRoseProj2 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 120;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.scale = 1f;
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
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			if (Projectile.timeLeft > 100)
                Projectile.velocity *= 0.94f;
            else
            {
                Vector2 move = Vector2.Zero;
                float distance = 1000f;
                bool targetted = false;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC target = Main.npc[i];
                    if (!target.CanBeChasedBy())
                        continue;

                    Vector2 newMove = target.Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = target.Center;
                        distance = distanceTo;
                        targetted = true;
                    }
                }
                if (targetted)
                    Projectile.Move(move, Projectile.timeLeft > 50 ? 30 : 50, 50);
                else
                    Projectile.velocity *= 0.94f;
            }
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.position);
			for (int i = 0; i < 2; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 219);
				Main.dust[num].velocity *= 2f;
				Main.dust[num].noGravity = true;
			}
        }
	}
}
