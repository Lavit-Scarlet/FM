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

namespace FM.Content.Projectiles.Ranged.Knifes.RosariaProjs
{
    public class RosariaKillProj : ModProjectile
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
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            //Projectile.usesLocalNPCImmunity = true;
			//Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Poisoned, 120, false);
		}
		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 107, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = false;
			Main.dust[dust].scale = 0.4f;
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
			for (int k = 0; k < 3; k++)
			{
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 107, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f);
				Main.dust[dust].noGravity = false;
    			Main.dust[dust].scale = 1f;
			}
		}
    }
}