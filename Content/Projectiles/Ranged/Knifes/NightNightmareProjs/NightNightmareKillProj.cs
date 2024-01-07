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

namespace FM.Content.Projectiles.Ranged.Knifes.NightNightmareProjs
{
    public class NightNightmareKillProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
			//AIType = ProjectileID.ThrowingKnife;
            AIType = ProjectileID.Bullet;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
			Projectile.tileCollide = true;
        }
		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1f;
			if (Projectile.owner == Main.myPlayer)
			{
				if (Projectile.velocity.Length() != 0f)
				{
					Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				}
				Projectile.ai[1] = Main.rand.Next(-2, 3);
				Projectile.netUpdate = true;
			}
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
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int i = 0; i < 12; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27, 0f, 0f, 0, default(Color), 2f);
				Main.dust[num].velocity *= 4f;
				Main.dust[num].noGravity = true;
			}
		}
    }
}