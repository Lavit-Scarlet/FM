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
using Terraria.Audio;

namespace FM.Content.NPCs.Bosses.Void.VoidProjs
{
    public class VoidLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 4800;
        }
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            Projectile.rotation = Projectile.velocity.ToRotation();
            return true;
        }
        int timer = 0;
        public override void PostDraw(Color lightColor)
        {
            if (timer < 50)
            {
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("FM/Assets/Textures/MagicPixel").Value,
                        Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 4800, 4), Color.DarkRed * (float)Math.Sin(timer * Math.PI / 50) * 0.66f, Projectile.rotation, new Vector2(0, 2), 1f, SpriteEffects.None, 0);
            }
            else if (timer >= 50 && timer < 60)
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

                float totalscale = (float)Projectile.timeLeft / 8;
                Color drawLaserColor = Color.Lerp(new Color(242, 89, 163), new Color(23, 12, 64), 0.48f);
                for (int i = 0; i < 3; i++)
                {
                    Main.EntitySpriteDraw(texture2, startPos - Main.screenPosition, null, drawLaserColor, 0, drawOrigin2, Projectile.scale * 0.10f * totalscale, SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(texture2, endPos - Main.screenPosition, null, drawLaserColor, 0, drawOrigin2, Projectile.scale * 0.14f * totalscale, SpriteEffects.None, 0);


                    Main.EntitySpriteDraw(texture3, startPos - Main.screenPosition, new Rectangle(0,0, (int)Distance - 16, texture3.Height), drawLaserColor * 1f * (Projectile.timeLeft > 12 ? 1f : (float)Projectile.timeLeft / 12), unit.ToRotation(), drawOrigin3, new Vector2(1, totalscale * 0.3f), SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(texture3, startPos - Main.screenPosition, new Rectangle(0, 0, (int)Distance - 16, texture3.Height), drawLaserColor * 1f * (Projectile.timeLeft > 12 ? 1f : (float)Projectile.timeLeft / 12), unit.ToRotation(), drawOrigin3, new Vector2(1, totalscale * 0.2f), SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(texture3, startPos - Main.screenPosition, new Rectangle(0,0, (int)Distance - 16, texture3.Height), drawLaserColor * 1f * (float)Math.Sin((float)Projectile.timeLeft * Math.PI / 30), unit.ToRotation(), drawOrigin3, new Vector2(1, totalscale * 0.5f), SpriteEffects.None, 0);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (timer < 50 || timer >= 60) return false;
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Vector2.Normalize(Projectile.velocity) * 4800f, 40, ref point);

			Vector2 unit = Vector2.Normalize(Projectile.velocity);
			float Distance = Projectile.ai[0];

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + unit * Distance, 22, ref point);
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            timer++;
            if (timer == 50)
            {
                SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            }

            Projectile.localAI[0]++;

            Player player = Main.player[Projectile.owner];

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);
            //Projectile.Center = player.MountedCenter;
	    	if (Projectile.ai[1] < 5)
            {
                for (Projectile.ai[0] = 16f; Projectile.ai[0] <= 4800f; Projectile.ai[0] += 8f)
                {
                    Projectile.ai[0] += 8f;
                }
                Projectile.ai[1] = 10;
            }
        }
    }
}
