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

namespace FM.Content.NPCs.Bosses.Armagem.ArmagemProjs
{
    public class ArmagemLaserProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 2800;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.scale = 1f;
            Projectile.timeLeft = 240;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            //Projectile.extraUpdates = 2;
            Projectile.friendly = false;
			Projectile.hostile = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[0] < 36)
            {
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("FM/Assets/Textures/MagicPixel").Value,
                Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 2800, 20), Color.Red * (float)Math.Sin(Projectile.ai[0] * Math.PI / 40) * 0.2f, Projectile.rotation, new Vector2(0, 3), 0.6f, SpriteEffects.None, 0);
            }

            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 2)
            {
                Color ColorDV = Color.Red; ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(Projectile.scale * 1.54f, Projectile.scale * 0.8f);
                Scale *= Mult;
                Mult *= 1f - Projectile.alpha / 255f;
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                color *= Mult; 
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            if (Projectile.ai[0] < 40) return false;
            return base.ShouldUpdatePosition();
        }
        public override bool? CanDamage()
        {
            if (Projectile.ai[0] < 40) return false;
            return base.CanDamage();
        }
        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.ai[0] == 40)
                FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Beam2"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .30f);

            if (Projectile.ai[0] >= 40)
            {
                Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
                Projectile.extraUpdates = 2;
                Projectile.alpha = 0;
            }
            Projectile.alpha = (int)(255 - 255f * Terraria.Utils.Clamp(Projectile.ai[0] / 20, 0, 1f));
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 235, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 3f;
                Main.dust[num].scale = 1f;
				Main.dust[num].noGravity = true;
			}
		}
    }
}