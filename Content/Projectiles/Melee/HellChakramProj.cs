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
using FM.Content.Projectiles.Melee;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;


namespace FM.Content.Projectiles.Melee
{
	public class HellChakramProj : ModProjectile, ITrailProjectile
	{
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 215, 70), new Color(200, 18, 34)), new RoundCap(), new DefaultTrailPosition(), 62f, 200f, new ImageShader(ModContent.Request<Texture2D>("FM/Assets/Textures/Noise/FireNoise", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 2f));
        }
		int timer = 0;
		public override void SetDefaults()
		{
            Projectile.tileCollide = true;
			Projectile.width = 62;
			Projectile.height = 62;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Melee; 
			Projectile.extraUpdates = 1;
			//aiType = 3;
			Projectile.aiStyle = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 12;
		}
		public override void AI()
		{
			Projectile.rotation += (float)Projectile.direction * 0.2f;
			Projectile.penetrate = -1;

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1.5f;

			int size = 62;
            Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.OnFire, 180, false);
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 20;
            height = 20;
            return true;
        }
	}
}
