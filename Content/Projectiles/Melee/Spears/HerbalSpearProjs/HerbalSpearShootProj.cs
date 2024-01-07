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

namespace FM.Content.Projectiles.Melee.Spears.HerbalSpearProjs
{
    public class HerbalSpearShootProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
			Projectile.extraUpdates = 2;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.Poisoned, 120, false);
			}
		}
		public override void AI()
		{
			Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 40, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1f;
		}
		public override bool PreDraw(ref Color lightColor) 
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) 
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Grass, Projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 40, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}
		}
    }
}