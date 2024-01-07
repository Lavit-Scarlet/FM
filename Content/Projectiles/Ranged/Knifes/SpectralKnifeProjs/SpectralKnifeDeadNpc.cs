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
    public class SpectralKnifeDeadNpc : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }
        public override void SetDefaults()
        {
			Projectile.tileCollide = false;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 100;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 1)
            {
                Color ColorDV = Color.LightBlue; ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(Projectile.scale);
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
			Player player = Main.player[Projectile.owner]; 
            Vector2 move = Vector2.Zero;
            float distance = 800f;
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

            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha >= 255)
                    Projectile.Kill();
            }
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
    }
}