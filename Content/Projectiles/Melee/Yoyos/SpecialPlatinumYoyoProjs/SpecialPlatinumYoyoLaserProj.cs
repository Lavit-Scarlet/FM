using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.GameContent;

namespace FM.Content.Projectiles.Melee.Yoyos.SpecialPlatinumYoyoProjs
{
    public class SpecialPlatinumYoyoLaserProj : ModProjectile
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

        //should be set to about half of the end length
        private const float FirstSegmentDrawDist = 14;

        public int MaxLaserLength = 2000;
        public int maxLaserFrames = 1;
        public int LaserFrameDelay = 5;
        public bool StopsOnTiles = true;
        // >

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }
        public override bool ShouldUpdatePosition() => false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation();
            #region Beginning And End Effects
            if (AITimer == 0)
                LaserScale = 0.1f;
            else
                Projectile.Center = Projectile.Center + Vector2.Normalize(Projectile.velocity) * 0f;

            if (AITimer <= 10)
            {
                LaserScale += 0.08f;
            }
            else if (Projectile.timeLeft < 58 || !player.active)
            {
                if (Projectile.timeLeft > 10)
                {
                    Projectile.timeLeft = 10;
                }
                LaserScale -= 0.1f;
            }
            #endregion

            #region Length Setting
            if (StopsOnTiles)
            {
                EndpointTileCollision();
            }
            else
            {
                LaserLength = MaxLaserLength;
            }
            #endregion

            #region Frame and Timer Updates
            /*++Projectile.frameCounter;
            if (Projectile.frameCounter >= LaserFrameDelay)
            {
                Projectile.frameCounter = 0;
                Frame++;
                if (Frame >= maxLaserFrames)
                {
                    Frame = 0;
                }
            }*/
            ++AITimer;
            #endregion

            #region misc
            //CutTiles();
            //CastLights();
            #endregion
        }

        #region Laser AI Submethods
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
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = new Vector2(1.5f, 0).RotatedBy(Projectile.rotation);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * LaserLength, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
        private void CastLights()
        {
            // Cast a light along the line of the Laser
            DelegateMethods.v3_1 = new Vector3(1f, 1f, 1f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + new Vector2(1f, 0).RotatedBy(Projectile.rotation) * LaserLength, 26, DelegateMethods.CastLight);
        }
        #endregion

        #region Drawcode
        // The core function of drawing a Laser, you shouldn't need to touch this
        public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default, int transDist = 1)
        {
            float r = unit.ToRotation() + rotation;
            // Draws the Laser 'body'
            for (float i = transDist; i <= (maxDist * (1 / LaserScale)); i += LaserSegmentLength)
            {
                Color col = Color.White;


                var origin = start + i * unit;
                Main.EntitySpriteDraw(texture, origin - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                    new Rectangle((int)(LaserWidth * Frame), LaserEndSegmentLength, LaserWidth, LaserSegmentLength), col, r,
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

            DrawLaser(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center + (new Vector2(Projectile.width, 0).RotatedBy(Projectile.rotation) * LaserScale), new Vector2(1f, 0).RotatedBy(Projectile.rotation) * LaserScale, -1.57f, LaserScale, LaserLength, new Color(255, 204, 181), (int)FirstSegmentDrawDist);

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
                Projectile.Center + unit * LaserLength, 2 * LaserScale, ref point))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region MP Sync
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(LaserLength);
            writer.Write(LaserScale);
            writer.Write(LaserSegmentLength);
            writer.Write(LaserEndSegmentLength);
            writer.Write(LaserWidth);
            writer.Write(MaxLaserLength);
            //writer.Write(maxLaserFrames);
            //writer.Write(LaserFrameDelay);
            writer.Write(StopsOnTiles);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            LaserLength = reader.ReadSingle();
            LaserScale = reader.ReadSingle();
            LaserSegmentLength = reader.ReadInt32();
            LaserEndSegmentLength = reader.ReadInt32();
            LaserWidth = reader.ReadInt32();
            MaxLaserLength = reader.ReadInt32();
            StopsOnTiles = reader.ReadBoolean();
            //maxLaserFrames = reader.
        }
        #endregion
    }
}