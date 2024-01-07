﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Globals;
using FM.Content.Projectiles.Melee.Spears.HeavenlySpearProjs;
using System.IO;
using Terraria.DataStructures;
using System;

namespace FM.Content.Projectiles.Melee.Spears.HeavenlySpearProjs
{
    public class HeavenlySpear_Slash : HeavenlySpearSlash
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 112;
            Projectile.height = 112;
            Projectile.scale = 1f;
            AttackAI[0] = 20f;
        }
        public override Vector2 PosA(Vector2 Pos, Vector2 Vec)
        {
            float Dis = (Projectile.width + Projectile.height + AttackAI[0]) * 0.45f;
            Vector2 A = Pos + Vec * Dis * Projectile.scale * 0.9f;
            return A;
        }
        public override Vector2 PosB(Vector2 Pos, Vector2 Vec)
        {
            float Dis = (Projectile.width + Projectile.height + AttackAI[0]) * 0.45f;
            Vector2 B = Pos + Vec * Dis * Projectile.scale * 0.4f;
            return B;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (AttackAI[1] == 1f)
            {
                Color[] LColor = new Color[] { Color.White };
                AttackEffects(LColor);
                AttackEffects(lightColor);
            }
            return false;
        }
        public override void AI()
        {
            if (AttackAI[1] == 1f)
            {

            }
            Attack();
            Player player = Main.player[Projectile.owner];
            if (player.itemAnimation < player.itemAnimationMax * 0.4f)
            {
                if (Projectile.ai[0] == 4f)
                {
                    Projectile.ai[0] = 5f;
                    ShootPro(1.5f);
                }
            }
            else if (player.itemAnimation < player.itemAnimationMax * 0.45f)
            {
                if (Projectile.ai[0] == 3f)
                {
                    Projectile.ai[0] = 4f;
                    ShootPro(0.75f);
                }
            }
            else if (player.itemAnimation < player.itemAnimationMax * 0.5f)
            {
                if (Projectile.ai[0] == 2f)
                {
                    Projectile.ai[0] = 3f;
                    ShootPro(0f);
                }
            }
            else if (player.itemAnimation < player.itemAnimationMax * 0.55f)
            {
                if (Projectile.ai[0] == 1f)
                {
                    Projectile.ai[0] = 2f;
                    ShootPro(-0.75f);
                }
            }
            else if (player.itemAnimation < player.itemAnimationMax * 0.6f)
            {
                if (Projectile.ai[0] == 0f)
                {
                    Projectile.ai[0] = 1f;
                    ShootPro(-1.5f);
                }
            }
        }
        private void ShootPro(float Rot)
        {
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/HeavenlyBlast"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .80f, 0.6f);
            Vector2 vec = Vector2.Normalize(DefVec) * 10f;
            vec = vec.RotatedBy(MathHelper.Pi * 0.15f * -Rot * Projectile.ai[1]);
            Player player = Main.player[Projectile.owner];
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center + vec, vec, ModContent.ProjectileType<HeavenlySpearShootProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
        public override void AttackPro()
        {
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/BigSlash"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .80f, 0.3f);
            base.AttackPro();
        }
    }
    public abstract class HeavenlySpearSlash : ModProjectile
    {
        public float[] AttackAI = new float[4];
        public Vector2 DefVec;
        public int Standstill = 0;
        public Player Player
        {
            get
            {
                return Main.player[Projectile.owner];
            }
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.scale = 1f;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 0;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Standstill = 0;
            AttackAI[0] = 0f;
            AttackAI[1] = 0f;
            AttackAI[2] = 0f;
            AttackAI[3] = 0f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool? falg = false;
            int Max = 10;
            for (int i = 0; i < Max; i++)
            {
                Vector2 Pos = Player.Center + new Vector2(0f, Player.gfxOffY);
                Vector2 Rot = Projectile.oldRot[0].ToRotationVector2();
                Vector2 Rot2 = Projectile.oldRot[1].ToRotationVector2();
                float VRot = Vector2.Lerp(Rot, Rot2, i / (float)Max).ToRotation() - 0.785f;
                Vector2 Vec = VRot.ToRotationVector2() * Projectile.velocity.Length();
                float num = 0f;
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox),
                    PosA(Pos, Vec), PosB(Pos, Vec), (Projectile.width + Projectile.height) / 2f, ref num))
                    falg = true;
            }
            return falg;
        }
        public void AttackEffects(Color lightColor, Texture2D Texture = null)
        {
            if (AttackAI[1] != 0f)
            {
                if (Texture == null) Texture = TextureAssets.Projectile[Projectile.type].Value;
                Rectangle rectangle = new(0, 0, Texture.Width, Texture.Height);
                Vector2 vector = Projectile.position + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                SpriteEffects spriteEffects = Projectile.ai[1] == 1f ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Main.spriteBatch.Draw(Texture, vector, new Rectangle?(rectangle), lightColor, Projectile.rotation + (spriteEffects == SpriteEffects.FlipHorizontally ? MathHelper.Pi / 2f : 0f), rectangle.Size() / 2f, Projectile.scale, spriteEffects, 0f);
            }
        }
        public void AttackEffects(Color[] LerpColor)
        {
            if (AttackAI[1] != 0f)
            {
                List<VertexInfo2> Vertex = new() { };
                int Max = Math.Min(Projectile.oldPos.Length, Projectile.timeLeft);
                List<float> LRot = new() { };
                for (int i = 0; i < Max - 1; i++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        Vector2 Vec = Projectile.oldRot[i].ToRotationVector2();
                        Vector2 Vec2 = Projectile.oldRot[i + 1].ToRotationVector2();
                        float Rot = Vector2.Lerp(Vec, Vec2, k / 10f).ToRotation();
                        LRot.Add(Rot);
                    }
                }
                Max = LRot.Count;
                for (int i = 0; i < Max; i++)
                {
                    Vector2 Vec = (LRot[i] - MathHelper.PiOver4).ToRotationVector2() * Projectile.velocity.Length();
                    Vector2 Pos = Player.Center + new Vector2(0f, Player.gfxOffY) - Main.screenPosition;
                    Color color = LerpColor[(int)MathHelper.Clamp(i, 0f, LerpColor.Length - 1)];
                    color *= 1f - i / (float)Max;
                    Vertex.Add(new VertexInfo2(PosA(Pos, Vec), new Vector3(i / (float)Max, 0f, 1f), color));
                    Vertex.Add(new VertexInfo2(PosB(Pos, Vec), new Vector3(i / (float)Max, 1f, 1f), color));
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("FM/Assets/Textures/Blade").Value;
                if (Vertex.Count >= 3)
                {
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertex.ToArray(), 0, Vertex.Count - 2);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        public virtual Vector2 PosA(Vector2 Pos, Vector2 Vec)
        {
            Vector2 A = Pos + Vec * AttackAI[0] * Projectile.scale * 2.1f;
            return A;
        }
        public virtual Vector2 PosB(Vector2 Pos, Vector2 Vec)
        {
            Vector2 B = Pos;
            return B;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.netUpdate = true;
            base.OnSpawn(source);
        }
        public virtual void Update()
        {
            Player.heldProj = Projectile.whoAmI;
            Player.itemTime = Player.itemAnimation;
            Projectile.Center = Player.Center;
            Projectile.spriteDirection = Player.direction;
            Projectile.rotation += AttackAI[3];
            Projectile.velocity = (Projectile.rotation - 0.785f).ToRotationVector2();
            Projectile.rotation = Projectile.velocity.ToRotation() + 0.785f;
            Projectile.position += Projectile.velocity * AttackAI[0] * Projectile.scale;
            float r = Projectile.velocity.ToRotation();
            if (Player.direction == -1) r += MathHelper.Pi;
            Player.itemRotation = r;
        }
        public void Attack()
        {
            if (AttackAI[1] == 0f)
            {
                Projectile.netUpdate = true;
                DefVec = Projectile.velocity;
                AttackAI[1] = 1f;
                AttackAI[2] = Projectile.damage;
                Projectile.rotation = Projectile.velocity.ToRotation() + 0.795f;
                Projectile.rotation += MathHelper.Pi * 0.65f * Projectile.ai[1];
                Projectile.netUpdate = true;
            }
            Update();
            float rot = MathHelper.TwoPi * 1.25f / Player.itemAnimationMax * -Projectile.ai[1];
            if (Player.itemAnimation < Player.itemAnimationMax / 2)
            {
                Projectile.scale -= 0f;
            }
            else
            {
                Projectile.scale += 0f;
            }
            if (Animation(0.25f))
            {
                float use = Player.itemAnimation / (float)Player.itemAnimationMax * 0.25f;
                AttackAI[3] = rot * use * 0.5f;
            }
            else if (Animation(0.5f))
            {
                float use = Player.itemAnimation / (float)Player.itemAnimationMax * 0.5f;
                AttackAI[3] = rot * use * 1.5f;
            }
            else if (Animation(0.75f))
            {
                if (Projectile.localAI[0] == 0f)
                {
                    Projectile.localAI[0] = 1f;
                    AttackPro();
                    Projectile.netUpdate = true;
                }
                float use = 1f - (Player.itemAnimation - Player.itemAnimationMax * 0.5f) / (float)Player.itemAnimationMax * 0.5f;
                AttackAI[3] = rot * use * 1.5f;
            }
            else
            {
                float use = 1f - (Player.itemAnimation - Player.itemAnimationMax * 0.75f) / (float)Player.itemAnimationMax * 0.25f;
                AttackAI[3] = rot * use * 0.5f;
            }
            if (Player.itemAnimation <= 1 || Player.itemTime <= 1)
            {
                Projectile.netUpdate = true;
                Projectile.Kill();
            }
            if (Projectile.localAI[1] == 0f)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Projectile.oldPos[i] = Projectile.position;
                    Projectile.oldRot[i] = Projectile.rotation;
                }
                Projectile.localAI[1] = 1f;
                Projectile.netUpdate = true;
            }
        }
        private bool Animation(float Mult)
        {
            return Player.itemAnimation < Player.itemAnimationMax * Mult;
        }
        public virtual void AttackPro()
        {
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(Projectile.velocity);
            writer.Write(Projectile.rotation);
            writer.Write(Projectile.ai[1]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.velocity = reader.ReadVector2();
            Projectile.rotation = reader.ReadSingle();
            Projectile.ai[1] = reader.ReadSingle();
        }
    }
}
