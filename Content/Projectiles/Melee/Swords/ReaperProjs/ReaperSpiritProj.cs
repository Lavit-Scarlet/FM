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


namespace FM.Content.Projectiles.Melee.Swords.ReaperProjs
{
	public class ReaperSpiritProj : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
        }
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new GradientTrail(new Color(196, 247, 255), new Color(6, 106, 255)), new NoCap(), new DefaultTrailPosition(), 20f, 100f, new ImageShader(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_2", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 2f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(196, 247, 255) * .25f, new Color(6, 106, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 30f, 200f, new DefaultShader());
        }
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = Main.rand.Next(180, 360);
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
		public override Color? GetAlpha(Color lightColor) 
		{
			return new Color(196, 247, 255, 0) * Projectile.Opacity;
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

			if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
            }
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 30f)
			{
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
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath39, Projectile.position);
			for (int num623 = 0; num623 < 20; num623++)
			{
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 111, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 3f;
			}
		}
	}
}
