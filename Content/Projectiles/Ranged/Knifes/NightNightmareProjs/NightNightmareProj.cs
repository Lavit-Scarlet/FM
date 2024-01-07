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
using FM.Content.Projectiles.Ranged.Knifes.NightNightmareProjs;

namespace FM.Content.Projectiles.Ranged.Knifes.NightNightmareProjs
{
    public class NightNightmareProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
			//AIType = ProjectileID.ThrowingKnife;
            AIType = ProjectileID.Bullet;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            //Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
        }
		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0.0f, 0.0f, 0, new Color(), 1f);
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
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			Projectile.Kill();
			return false;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 15; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27, 0f, 0f, 0, default(Color), 2f);
				Main.dust[num].velocity *= 4f;
				Main.dust[num].noGravity = true;
			}
			Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X + Main.rand.Next(-5, 5), Projectile.velocity.Y + Main.rand.Next(-5, 5), ProjectileType<NightNightmareKillProj>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X + Main.rand.Next(-5, 5), Projectile.velocity.Y + Main.rand.Next(-5, 5), ProjectileType<NightNightmareKillProj>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, 0f, 0f);
		}
    }
}