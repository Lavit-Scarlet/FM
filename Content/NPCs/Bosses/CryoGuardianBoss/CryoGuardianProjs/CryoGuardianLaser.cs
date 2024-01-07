using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Globals;
using Terraria.Audio;
using Terraria.GameContent;


namespace FM.Content.NPCs.Bosses.CryoGuardianBoss.CryoGuardianProjs
{
    public class CryoGuardianLaser : LaserProjectile
    {
        private new const float FirstSegmentDrawDist = 14;
        public override bool ShouldUpdatePosition() => false;
        public override void SetSafeDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 140;
            LaserScale = 1;
            LaserSegmentLength = 28;
            LaserWidth = 26;
            LaserEndSegmentLength = 28;
            MaxLaserLength = 2000;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            #region Beginning And End Effects
            Projectile ls = Main.projectile[(int)Projectile.ai[0]];
            if (AITimer == 0)
            {
                if (ls.type == ModContent.ProjectileType<CryoGuardianLaserSpawn>())
                    Projectile.timeLeft = 80;
                LaserScale = 0.1f;
            }

            if (AITimer == 20)
                FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Beam"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .50f, .40f);

            if (ls.active && ls.type == ModContent.ProjectileType<CryoGuardianLaserSpawn>())
            {
                Projectile.Center = ls.Center;
                Projectile.velocity = FMHelper.PolarVector(10, ls.rotation + (float)-Math.PI / 2);
            }

            if (AITimer > 20 && AITimer <= 30)
                LaserScale += 0.09f;
            else if (Projectile.timeLeft < 10 || !ls.active)
            {
                if (Projectile.timeLeft > 10)
                    Projectile.timeLeft = 10;

                LaserScale -= 0.1f;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            #endregion

            LaserLength = MaxLaserLength;
            ++AITimer;
        }
        public override bool CanHitPlayer(Player target)
        {
            return AITimer > 20;
        }
        #region Drawcode
        public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default, int transDist = 1)
        {
            float r = unit.ToRotation() + rotation;
            // Draws the Laser 'body'
            for (float i = transDist; i <= (maxDist * (1 / LaserScale)); i += LaserSegmentLength)
            {
                var origin = start + i * unit;
                Main.EntitySpriteDraw(texture, origin - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                    new Rectangle((int)(LaserWidth * Frame), LaserEndSegmentLength, LaserWidth, LaserSegmentLength), color, r,
                    new Vector2(LaserWidth / 2, LaserSegmentLength / 2), scale, 0, 0);
            }
            // Draws the Laser 'base'
            Main.EntitySpriteDraw(texture, start + unit * (transDist - LaserEndSegmentLength) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                new Rectangle((int)(LaserWidth * Frame), 0, LaserWidth, LaserEndSegmentLength), color, r, new Vector2(LaserWidth / 2, LaserSegmentLength / 2), scale, 0, 0);
            // Draws the Laser 'end'
            Main.EntitySpriteDraw(texture, start + maxDist * (1 / scale) * unit - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                new Rectangle((int)(LaserWidth * Frame), LaserSegmentLength + LaserEndSegmentLength, LaserWidth, LaserEndSegmentLength), color, r, new Vector2(LaserWidth / 2, LaserSegmentLength / 2), scale, 0, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            DrawLaser(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center + (new Vector2(Projectile.width, 0).RotatedBy(Projectile.rotation) * LaserScale), new Vector2(1f, 0).RotatedBy(Projectile.rotation) * LaserScale, -1.57f, LaserScale, LaserLength, Projectile.GetAlpha(Color.White), (int)FirstSegmentDrawDist);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        #endregion
    }
}