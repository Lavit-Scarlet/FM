using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace FM.Content.Projectiles.Melee.Swords.BloodSwordProjs
{
    public class BloodSwordShootHitEffectProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.light = 1f;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.tileCollide = false;
        }
        public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPvp(Player target)
		{
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return false;
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
        public override bool PreDraw(ref Color lightColor)
        {
            float opacity = Projectile.Opacity;
            if (Projectile.localAI[0] == 0f) return false;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = new(0, 0, texture.Width, texture.Height);
            Color color = new Color(255, 75, 75, 0) * opacity;
            color.A = 0;

            Color color2 = new Color(255, 255, 255);
            color2.A = 0;

            Vector2 Scale = new(Projectile.scale * .34f, Projectile.scale * .64f);
            Vector2 Scale2 = new(Projectile.scale * .14f, Projectile.scale * .44f);
            Vector2 vector = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
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
