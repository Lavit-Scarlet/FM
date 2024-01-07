using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Dynamic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Magic.StaffofPrismProjs
{
    public class StaffofPrismLaser : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true; 
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
        }
        public override bool? CanCutTiles()
        {
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            return true;
        }
        public override void PostDraw(Color lightColor)
        {
            Vector2 unit = Vector2.Normalize(Projectile.velocity);

            float Distance = Projectile.ai[0];

            Texture2D texture2 = ModContent.Request<Texture2D>("FM/Assets/Textures/Extras/flare").Value;
            Vector2 drawOrigin2 = new Vector2(texture2.Width / 2, texture2.Height / 2);

            Texture2D texture3 = ModContent.Request<Texture2D>("FM/Assets/Textures/LightBeam").Value;
            Vector2 drawOrigin3 = new Vector2(0, 32f);

            Vector2 startPos = Projectile.Center + 16f * unit;
            Vector2 endPos = Projectile.Center + Distance * unit;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            float totalscale = (float)Projectile.timeLeft / 24;
            Color drawLaserColor = Main.hslToRgb(Projectile.localAI[0] / 18, 0.8f, 0.66f);


            Main.EntitySpriteDraw(texture2, startPos - Main.screenPosition, null, Color.White, 0, drawOrigin2, Projectile.scale * 0.10f * totalscale, SpriteEffects.None, 0);


            Main.EntitySpriteDraw(texture2, endPos - Main.screenPosition, null, Color.White, 0, drawOrigin2, Projectile.scale * 0.14f * totalscale, SpriteEffects.None, 0);



            Main.EntitySpriteDraw(texture3, startPos - Main.screenPosition, new Rectangle(0,0, (int)Distance - 16, texture3.Height), drawLaserColor * 0.8f * (Projectile.timeLeft > 12 ? 1f : (float)Projectile.timeLeft / 12), unit.ToRotation(), drawOrigin3, new Vector2(1, totalscale * 0.3f), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture3, startPos - Main.screenPosition, new Rectangle(0, 0, (int)Distance - 16, texture3.Height), Color.White * 0.9f * (Projectile.timeLeft > 12 ? 1f : (float)Projectile.timeLeft / 12), unit.ToRotation(), drawOrigin3, new Vector2(1, totalscale * 0.2f), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture3, startPos - Main.screenPosition, new Rectangle(0,0, (int)Distance - 16, texture3.Height), drawLaserColor * 0.63f * (float)Math.Sin((float)Projectile.timeLeft * Math.PI / 30), unit.ToRotation(), drawOrigin3, new Vector2(1, totalscale * 0.5f), SpriteEffects.None, 0);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool ShouldUpdatePosition() => false;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
        {
			Vector2 unit = Vector2.Normalize(Projectile.velocity);
			float point = 0f;
			float Distance = Projectile.ai[0];

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + unit * Distance, 22, ref point);
		}

        public override void AI()
        {
            Projectile.localAI[0]++;

            Player player = Main.player[Projectile.owner];

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);
            //Projectile.Center = player.MountedCenter;
	    	if (Projectile.ai[1] < 5)
            {
                for (Projectile.ai[0] = 16f; Projectile.ai[0] <= 2000f; Projectile.ai[0] += 8f)
                {
                    var start = Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];
                    if (!Collision.CanHitLine(player.Center - new Vector2(1,1), 2, 2, start- new Vector2(1,1), 2, 2))
                    {
                        Projectile.ai[0] += 8f;
                        break;
                    }
                }
                Projectile.ai[1] = 10;
            }

            //float castlight = (float)Projectile.timeLeft / 30;
            //DelegateMethods.v3_1 = new Vector3(castlight, castlight, castlight);
            //Terraria.Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Projectile.ai[0] - 16), 26, DelegateMethods.CastLight);
        }
    }
}
