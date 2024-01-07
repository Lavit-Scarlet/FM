using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ID;
using System.Linq;
using Terraria.Enums;

using Microsoft.Xna.Framework.Graphics;

using Terraria.Audio;


namespace FM.Globals
{
    internal class FMGlobalProjectile : GlobalProjectile
    {
        public float[] ModAI = new float[5];
        public bool ignoresArmor = false;
        public override bool InstancePerEntity => true;
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (ignoresArmor)
            {
                modifiers.ArmorPenetration += 10000;
            }
        }
        public static NPC Trace(Projectile projectile, float DistanceMax, Vector2 Center)
        {
            NPC target = null;
            float distanceMax = DistanceMax;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active
                    && !npc.friendly
                    && npc.type != NPCID.TargetDummy
                    && !npc.dontTakeDamage
                    && !NPCID.Sets.CountsAsCritter[npc.type])
                {
                    if (projectile.tileCollide && !Collision.CanHit(npc.position, npc.width, npc.height, projectile.position, projectile.width, projectile.height)) continue;
                    float currentDistance = Vector2.Distance(npc.Center, Center);
                    if (currentDistance < distanceMax)
                    {
                        distanceMax = currentDistance;
                        target = npc;
                    }
                }
            }
            return target;
        }
    }
    public abstract class TrueMeleeProjectile : ModProjectile
    {
        public float SetSwingSpeed(float speed)
        {
            Terraria.Player player = Main.player[Projectile.owner];
            return speed / player.GetAttackSpeed(DamageClass.Melee);
        }

        public virtual void SetSafeDefaults() { }

        public override void SetDefaults()
        {
            SetSafeDefaults();
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
        }
    }
    public abstract class LaserProjectile : ModProjectile
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
        public int LaserSegmentLength = 10;
        public int LaserWidth = 20;
        public int LaserEndSegmentLength = 22;

        public const float FirstSegmentDrawDist = 7;

        public int MaxLaserLength = 2000;
        public int maxLaserFrames = 1;
        public int LaserFrameDelay = 5;
        public bool StopsOnTiles = true;

        public virtual void SetSafeStaticDefaults() { }

        public override void SetStaticDefaults()
        {
            SetSafeStaticDefaults();
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }

        public virtual void SetSafeDefaults() { }

        public override void SetDefaults()
        {
            Projectile.width = LaserWidth;
            Projectile.height = LaserWidth;
            SetSafeDefaults();
        }
        public virtual void EndpointTileCollision()
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
        public virtual void CastLights(Vector3 color)
        {
            // Cast a light along the line of the Laser
            DelegateMethods.v3_1 = color;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + new Vector2(1f, 0).RotatedBy(Projectile.rotation) * LaserLength, 26, DelegateMethods.CastLight);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 unit = new Vector2(1.5f, 0).RotatedBy(Projectile.rotation);
            float point = 0f;
            // Run an AABB versus Line check to look for collisions
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + unit * LaserLength, Projectile.width * LaserScale, ref point))
                return true;
            return false;
        }
    }
}