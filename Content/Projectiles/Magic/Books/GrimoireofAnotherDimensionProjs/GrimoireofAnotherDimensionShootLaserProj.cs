using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Globals;
using Terraria.Audio;
using Terraria.GameContent;
using FM.Content.Projectiles;
using System.IO;
using Terraria.Enums;

namespace FM.Content.Projectiles.Magic.Books.GrimoireofAnotherDimensionProjs
{
    public class GrimoireofAnotherDimensionShootLaserProj : ModProjectile
    {
        public float AITimer
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public float Frame
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }
        public float LaserLength = 0;
        public float LaserScale = 0;
        public int LaserSegmentLength = 28;
        public int LaserWidth = 26;
        public int LaserEndSegmentLength = 28;
        public bool StopsOnTiles = false;

        //should be set to about half of the end length
        private const float FirstSegmentDrawDist = 14;

        public int MaxLaserLength = 2000;
        // >
        public override bool ShouldUpdatePosition() => false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            #region Beginning And End Effects
            Player player = Main.player[Projectile.owner];
            if (AITimer == 0)
            {
                LaserScale = 0.1f;
            }
            else
            {
                Projectile.Center = Projectile.Center + Vector2.Normalize(Projectile.velocity) * 0f;
            }

            if (AITimer == 30)
            {
                Projectile.friendly = true;
                FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Beam"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .30f);
                {
                    Projectile.Center = Projectile.Center + Vector2.Normalize(Projectile.velocity) * 0f;
                }
            }

            if (AITimer > 30 && AITimer <= 50)
            {
                LaserScale += 0.05f;
            }
            else if (Projectile.timeLeft < 10 || !player.active)
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

            if (StopsOnTiles)
            {
                EndpointTileCollision();
            }
            else
            {
                LaserLength = MaxLaserLength;
            }
        }

        private void EndpointTileCollision()
        {
            for (LaserLength = FirstSegmentDrawDist; LaserLength < MaxLaserLength; LaserLength += LaserSegmentLength)
            {
                Vector2 start = Projectile.Center + Vector2.UnitX.RotatedBy(Projectile.rotation) * LaserLength;
                if (!Collision.CanHitLine(Projectile.Center, 1, 1, start, 1, 1))
                {
                    LaserLength -= LaserSegmentLength;
                    break;
                }
            }
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

        #region Collisions
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 unit = new Vector2(1.5f, 0).RotatedBy(Projectile.rotation);
            float point = 0f;
            // Run an AABB versus Line check to look for collisions
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + unit * LaserLength, Projectile.width * LaserScale, ref point))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}