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
using static Terraria.ModLoader.ModContent;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Ranged.Bows.ElegantBowProjs
{
    public class ElegantBowYellowProj : ModProjectile, ITrailProjectile
    {
		public void DoTrailCreation(TrailManager tManager)
        {
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 0), new Color(255, 255, 0)), new RoundCap(), new DefaultTrailPosition(), 6f, 200f, new DefaultShader());
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 0) * .20f, new Color(255, 255, 0) * .20f), new RoundCap(), new DefaultTrailPosition(), 12f, 400f, new DefaultShader());
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            //Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
        }
		public override Color? GetAlpha(Color lightColor) 
		{
			return new Color(255, 255, 0);
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
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.netUpdate = true;
			NPC pre = null;
			if (FMHelper.ClosestNPC(ref pre, 400, Projectile.Center, false))
			{
				float direction1 = Projectile.velocity.ToRotation();
				direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(1));
				Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
			}
        }
		public override void Kill(int timeLeft)
		{
            for (int i = 0; i < 15; i++)
			{
				ParticleManager.NewParticle<LineStreak_ShortTime>(Projectile.Center, new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f)), new Color(255, 255, 0), 3f, 0, 0);
			}
            ParticleManager.NewParticle<HollowCircle_Small>(Projectile.Center, Vector2.Zero, new Color(255, 255, 0), 0.5f, 0, 0);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
    }
}