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
using Terraria.GameContent.Drawing;

namespace FM.Content.Projectiles.Magic.Books.BlackGrimoireProjs
{
    public class BlackGrimoireShootProj : ModProjectile, ITrailProjectile
    {
		public void DoTrailCreation(TrailManager tManager)
        {
            //tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 0, 0) * .25f, new Color(255, 0, 0) * .25f), new RoundCap(), new ArrowGlowPosition(), 50f, 580f, new ImageShader(ModContent.Request<Texture2D>("FM/Textures/Trails/Trail_1", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 1f));

			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 45, 45), new Color(255, 120, 100)), new RoundCap(), new DefaultTrailPosition(), 6f, 160f, new DefaultShader());
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 45, 45) * .20f, new Color(255, 45, 45) * .20f), new RoundCap(), new DefaultTrailPosition(), 12f, 320f, new DefaultShader());
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = Main.rand.Next(120, 180);
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
        }
		public override Color? GetAlpha(Color lightColor) 
		{
			return new Color(255, 45, 45);
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
			if (FMHelper.ClosestNPC(ref pre, 400, Projectile.Center, true))
			{
				float direction1 = Projectile.velocity.ToRotation();
				direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(2));
				Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
			}
        }
    }
}