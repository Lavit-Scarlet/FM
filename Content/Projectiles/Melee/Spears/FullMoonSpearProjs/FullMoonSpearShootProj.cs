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
using Terraria.GameContent.Drawing;

namespace FM.Content.Projectiles.Melee.Spears.FullMoonSpearProjs
{
    public class FullMoonSpearShootProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void SetDefaults()
        {
			Projectile.tileCollide = false;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
			Projectile.localNPCHitCooldown = 10;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) }, Projectile.owner);
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 2)
            {
                Color ColorDV = Color.DarkViolet; ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(1.1f, Projectile.scale) * 1f;
                Scale *= Mult;
                Mult *= 1f - Projectile.alpha / 255f;
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Color CW = Color.DarkViolet; CW.A = 0;
                CW *= 0.25f;
                color *= Mult; CW *= Mult;
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), CW, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
            }
            return false;
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            Color color = Color.DarkViolet;
            color.A = 0;
            color *= 1.25f;
            color *= 1f - Projectile.alpha / 255f;
            for (int j = 0; j < Projectile.oldPos.Length; j++)
            {
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Color color3 = color * ((Projectile.oldPos.Length - j) / (float)Projectile.oldPos.Length);
                if (j != 0) color3 *= 0.5f;
                Main.spriteBatch.Draw(texture2D, vector,
                    new Rectangle?(rectangle), color3,
                    Projectile.rotation, Utils.Size(rectangle) / 2f,
                    new Vector2(1f), 0, 0f);
            }
            return false;
        }*/
        public override void AI()
        {
			Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;

			Projectile.alpha += 10;
            if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}   
        }
    }
}