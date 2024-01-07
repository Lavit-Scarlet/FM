using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Effects.PrimitiveTrails;
using System.IO;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using FM.Effects;
using System.Collections.Generic;
using FM.Content.Projectiles.Melee.Swords.HeavenlySwordProjs;

namespace FM.Content.Projectiles.Melee.Swords.HeavenlySwordProjs
{
    public class HeavenlySwordProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.extraUpdates = 1;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
            Color ColorDV = Color.White; ColorDV.A = 0;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), ColorDV, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);

            Main.spriteBatch.End();
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["MoR:GlowTrailShader"]?.GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/BeamTrail").Value);
            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.05f);
            effect.Parameters["repeats"].SetValue(2f);

            trail?.Render(effect);
            trail2?.Render(effect);

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}
        private readonly int NUMPOINTS = 26;
        public Color baseColor = new(216, 246, 253);
        public Color endColor = new(72, 135, 205);
        public Color EdgeColor = new(93, 203, 243);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 1.9f;
        public override void AI()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center + Projectile.velocity);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center + Projectile.velocity, baseColor, endColor, EdgeColor, thickness);
            }
            Projectile.velocity *= .97f;
            Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Slashing"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .4f, -0.10f);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity / 6, ModContent.ProjectileType<HeavenlySwordCuttingProj>(), Projectile.damage, 0, Projectile.owner);
        }
    }
}