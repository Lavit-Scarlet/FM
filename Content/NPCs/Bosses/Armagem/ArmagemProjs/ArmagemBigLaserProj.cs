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
using FM.Content.NPCs.Bosses.Armagem.ArmagemProjs;

namespace FM.Content.NPCs.Bosses.Armagem.ArmagemProjs
{
	public class ArmagemBigLaserProj : ModProjectile
	{
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
        }
		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.timeLeft = 420;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 2;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);

            Texture2D texture2D = ModContent.Request<Texture2D>("FM/Content/NPCs/Bosses/Armagem/ArmagemProjs/ArmagemBigLaserProj_Trail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int num12 = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num12 * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num12);
            Color color = Color.Lerp(Color.White, Color.Red, 0.55f);
            color.A = 0;
            color *= 1.25f;
            color *= 1f - Projectile.alpha / 255f;
            for (int j = 0; j < Projectile.oldPos.Length; j++)
            {
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Color color3 = color * ((Projectile.oldPos.Length - j) / (float)Projectile.oldPos.Length);
                if (j != 0) color3 *= 0.5f;
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color3, Projectile.rotation, Utils.Size(rectangle) / 2f, new Vector2(1f, Projectile.scale * 1f), 0, 0f);
            }
			return false;
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 10; num257++)
			{
				int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, 235, 0f, 0f, 0, default(Color), 1.4f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].noLight = true;
				vector9 -= value19 * 2f;
			}
		}
	}
}
