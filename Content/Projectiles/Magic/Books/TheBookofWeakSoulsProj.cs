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
using System.IO;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;

namespace FM.Content.Projectiles.Magic.Books
{
	public class TheBookofWeakSoulsProj : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
        }
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new GradientTrail(new Color(196, 247, 255), new Color(6, 106, 255)), new NoCap(), new DefaultTrailPosition(), 20f, 100f, new ImageShader(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_2", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 2f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(196, 247, 255) * .25f, new Color(6, 106, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 32f, 200f, new DefaultShader());
        }
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.penetrate = 1; 
			Projectile.timeLeft = Main.rand.Next(40, 100);
			Projectile.DamageType = DamageClass.Magic;
		}
		public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);
		public override void AI()
		{
			if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
            }
            Vector2 move = Vector2.Zero;
            float distance = 800f;
            bool targetted = false;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (!target.CanBeChasedBy())
                    continue;

                Vector2 newMove = target.Center - Projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                if (distanceTo < distance)
                {
                    move = target.Center;
                    distance = distanceTo;
                    targetted = true;
                }
            }
            if (targetted)
                Projectile.Move(move, Projectile.timeLeft > 50 ? 30 : 50, 50);

			if (Projectile.owner == Main.myPlayer)
			{
				if (Projectile.velocity.Length() != 0f)
				{
					Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				}
				Projectile.ai[1] = Main.rand.Next(-4, 4);
				Projectile.netUpdate = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath39, Projectile.position);
			for (int num623 = 0; num623 < 20; num623++)
			{
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 2f;
			}
		}
	}
}
