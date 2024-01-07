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
using FM.Content.Projectiles.Melee.Swords.ExoBladeProjs;

namespace FM.Content.Projectiles.Melee.Swords.ExoBladeProjs
{
    public class ExoBladeCuttingProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 20;
            Projectile.scale = 1f;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num = 0f;
            return new bool?(Collision.CheckAABBvLineCollision(
                Utils.TopLeft(targetHitbox),
                Utils.Size(targetHitbox),
                Projectile.Center - Projectile.rotation.ToRotationVector2() * (Projectile.width / 2 * Projectile.scale),
                Projectile.Center + Projectile.rotation.ToRotationVector2() * (Projectile.width / 2 * Projectile.scale),
                Projectile.height * Projectile.scale,
                ref num));
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Slashing"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .24f, .14f);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<ExoBladeCuttingProj3>(),
            Projectile.damage, 0, Projectile.owner, Main.rand.NextFloat(MathHelper.TwoPi), 1f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0] == 0f) return false;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = new(0, 0, texture.Width, texture.Height);
            Color color = Color.Blue * 600;
            color.A = 0;
            Color color2 = Color.White * 600;
            color2.A = 0;

            Vector2 Scale = new(Projectile.scale * 10f, Projectile.scale * .76f);//длина, ширина
            Vector2 Scale2 = new(Projectile.scale * 8f, Projectile.scale * .52f);
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, vector, new Rectangle?(rectangle), color, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
            Main.spriteBatch.Draw(texture, vector, new Rectangle?(rectangle), color2, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale2, 0, 0f);
            return false;
        }
        public override void AI()
        {
            Projectile.scale = Projectile.localAI[1];
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.rotation = Projectile.ai[0];
                Projectile.netUpdate = true;
                return;
            }
            if (Projectile.timeLeft < 10)
            {
                Projectile.localAI[1] += 1f / 10f;
            }
            if (Projectile.timeLeft > 10)
            {
                Projectile.localAI[1] -= 1f / 10f;
            }
        }
    }
}
