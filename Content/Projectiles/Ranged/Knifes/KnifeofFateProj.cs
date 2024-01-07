using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Effects.PrimitiveTrails;
using System.IO;
using Terraria.Audio;

namespace FM.Content.Projectiles.Ranged.Knifes
{
    public class KnifeofFateProj : ModProjectile, ITrailProjectile
    {
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 252, 148), new Color(61, 187, 177)), new RoundCap(), new DefaultTrailPosition(), 6f, 200f, new ImageShader(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_2", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 1f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 255) * .25f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 24f, 300f, new DefaultShader());
        }
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 113;
			AIType = ProjectileID.ThrowingKnife;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
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
			for (int k = 0; k < 6; k++)
			{
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 163, Projectile.oldVelocity.X * -0.2f, Projectile.oldVelocity.Y * -0.2f);
				int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 155, Projectile.oldVelocity.X * -0.2f, Projectile.oldVelocity.Y * -0.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust2].noGravity = true;
			}
		}
    }
}