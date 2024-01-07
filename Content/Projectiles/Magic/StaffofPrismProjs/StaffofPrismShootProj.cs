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
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;
using FM.Content.Projectiles.Magic.StaffofPrismProjs;
using Terraria.GameContent.Drawing;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Magic.StaffofPrismProjs
{
	public class StaffofPrismShootProj : ModProjectile, ITrailProjectile
	{
		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.scale = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 12;
            Projectile.light = 0.25f;
			//Projectile.extraUpdates = 1;
		}
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new RainbowTrail(5f, 0.002f, 1f, .75f), new RoundCap(), new DefaultTrailPosition(), 12f, 300f, new DefaultShader());
            tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 255) * .24f, new Color(255, 255, 255) * .24f), new RoundCap(), new DefaultTrailPosition(), 20f, 400f, new DefaultShader());
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            
			Projectile.ai[0] -= 1.1f;
			if (Projectile.localAI[1] == 0f)
			{
                /*int DustType = Main.rand.Next(7);
    			switch(DustType)
    			{
    				case 0:
    					DustType = 272;
    					break;
     				case 1:
    					DustType = 264;
    					break;
    				case 2:
    					DustType = 223;
     					break;
    				case 3:
    					DustType = 222;
    					break;
    				case 4:
    					DustType = 221;
    					break;
    				case 5:
    					DustType = 220;
    					break;
    				case 6:
    					DustType = 219;
    					break;
    				default:
        				break;
    			}
				
				float count = 40.0f;
				for (int k = 0; (double)k < (double)count; k++)
				{
					Vector2 vector2 = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double)k * (6.22 / (double)count), new Vector2()) * new Vector2(2.0f, 14.0f)).RotatedBy((double)Projectile.velocity.ToRotation(), new Vector2());
					int dust = Dust.NewDust(Projectile.Center - new Vector2(0.0f, 4.0f), 0, 0, DustID.RainbowMk2, 0, 0, 0, Main.DiscoColor);
					Main.dust[dust].scale = 1f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position = Projectile.Center + vector2;
					Main.dust[dust].velocity = Projectile.velocity * 0.0f + vector2.SafeNormalize(Vector2.UnitY) * 1.0f;
				}*/
				Projectile.localAI[1] = 1f;
				for (int i = 0; i < 3; i++)
				{
					ParticleManager.NewParticle<HollowCircle_Small2>(Projectile.Center, Projectile.velocity, Main.DiscoColor, 0.35f, 0, 0);
				}

			}
            Vector2 move = Vector2.Zero;
            float distance = 600f;
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
		//public override bool? CanHitNPC(NPC target) => !target.friendly && Projectile.timeLeft <= 100 ? null : false;
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Main.DiscoColor * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale * 1.5f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			
			
			float rotParticle = MathHelper.ToRadians(-45);
			
			float numberParticles = Main.rand.Next(10, 14);
			for (int i = 0; i < numberParticles; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.4f), Main.DiscoColor, Main.rand.NextFloat(0.5f, 1f), 0, 0);
			}
		}
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
			for (int i = 0; i < 15; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(Projectile.Center, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), Main.DiscoColor, Main.rand.NextFloat(0.5f, 1f), 0, 0);
			}
            /*int KProj = Main.rand.Next(1, 5);
            if (Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < KProj; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, FMHelper.PolarVector(4, Main.rand.NextFloat(0, MathHelper.TwoPi)), ModContent.ProjectileType<StaffofThePrismaticOrbKillProj>(), Projectile.damage, 0, player.whoAmI);
                }
            }
            int DustType = Main.rand.Next(7);
			switch(DustType)
			{
				case 0:
					DustType = 272;
					break;
				case 1:
					DustType = 264;
					break;
				case 2:
					DustType = 223;
					break;
				case 3:
					DustType = 222;
					break;
				case 4:
					DustType = 221;
					break;
				case 5:
					DustType = 220;
					break;
				case 6:
					DustType = 219;
					break;
				default:
     				break;
			}
            
			for (int i = 0; i < 20; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.RainbowMk2, 0, 0, 0, Main.DiscoColor, 2.3f);
				Main.dust[num].velocity *= 4f;
				Main.dust[num].noGravity = true;
			}*/
            ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.RainbowRodHit, new ParticleOrchestraSettings { PositionInWorld = Projectile.Center });
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
	}
}
