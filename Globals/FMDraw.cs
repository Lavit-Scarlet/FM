using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Assets.Textures;
using FM.Content.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FM.Globals
{
    public static class FMDraw
    {
        public static void SpawnRing(Vector2 center, Color color, float flatScale = 0.13f, float multiScale = 0.9f, float glowScale = 2)
        {
            Dust dust = Dust.NewDustPerfect(center, ModContent.DustType<GlowDust>(), Vector2.Zero, Scale: glowScale);
            dust.noGravity = true;
            Color dustColor = new(color.R, color.G, color.B) { A = 0 };
            dust.color = dustColor;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int p = Projectile.NewProjectile(null, center, Vector2.Zero, ModContent.ProjectileType<Ring_Visual>(), 0, 0,
                    Main.myPlayer, flatScale, multiScale);
                (Main.projectile[p].ModProjectile as Ring_Visual).color = color;
            }
        }

        public static void DrawTreasureBagEffect(SpriteBatch spriteBatch, Texture2D tex, ref float drawTimer, Vector2 position, Rectangle? rect, Color color, float rot, Vector2 origin, Vector2 scale, SpriteEffects effects = 0)
        {
            float time = Main.GlobalTimeWrappedHourly;
            float timer = drawTimer / 240f + time * 0.04f;
            time %= 4f;
            time /= 2f;
            if (time >= 1f)
                time = 2f - time;
            time = time * 0.5f + 0.5f;
            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(tex, position + new Vector2(0f, 8f).RotatedBy(radians) * time, rect, new Color(color.R, color.G, color.B, 50), rot, origin, scale, effects, 0);
            }
            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(tex, position + new Vector2(0f, 4f).RotatedBy(radians) * time, rect, new Color(color.R, color.G, color.B, 77), rot, origin, scale, effects, 0);
            }
        }
        public static void SpawnCirclePulse(Vector2 center, Color color, float scale = 1, Entity target = null)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int p = Projectile.NewProjectile(null, center, Vector2.Zero, ModContent.ProjectileType<CirclePulse_Visual>(), 0, 0,
                    Main.myPlayer, scale);
                (Main.projectile[p].ModProjectile as CirclePulse_Visual).color = color;
                (Main.projectile[p].ModProjectile as CirclePulse_Visual).entityTarget = target;
            }
        }
        public static void SpawnExplosion(Vector2 center, Color color, int dustID = DustID.Torch, float shakeAmount = 7, int dustAmount = 30, float dustScale = 2, float scale = 4f, bool noDust = false, Texture2D tex = null, float rot = 0)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int p = Projectile.NewProjectile(null, center, Vector2.Zero, ModContent.ProjectileType<Explosion_Visual>(), 0, 0,
                    Main.myPlayer, shakeAmount, dustAmount);
                Main.projectile[p].rotation = rot;
                if (Main.projectile[p].ModProjectile is Explosion_Visual explode)
                {
                    explode.color = color;
                    explode.dustID = dustID;
                    explode.dustScale = dustScale;
                    explode.scale = scale;
                    explode.noDust = noDust;
                    explode.texture = tex;
                }
            }
        }
    }

}