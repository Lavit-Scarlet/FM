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
using FM.Content.Projectiles.Melee.Swords.PandoraProjs;
using Terraria.GameContent.Drawing;

namespace FM.Content.Projectiles.Melee.Swords.PandoraProjs
{
	public class PandoraProj : ModProjectile
	{
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 90;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            Color color = Color.Lerp(Color.White, Color.White, 0.75f);
            color.A = 0;
            color *= 1.25f;
            color *= 1f - Projectile.alpha / 255f;
            for (int j = 0; j < Projectile.oldPos.Length; j++)
            {
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Color color3 = color * ((Projectile.oldPos.Length - j) / (float)Projectile.oldPos.Length);
                if (j != 0) color3 *= 0.5f;
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color3, Projectile.rotation, Utils.Size(rectangle) / 2f, new Vector2(1.25f, Projectile.scale * 1.25f), 0, 0f);
            }
            return false;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) }, Projectile.owner);
			if (target.life <= 0 && Main.rand.Next(2) == 0)
			{
				SoundEngine.PlaySound(SoundID.Item68, Projectile.position);
				int n = 8;
                int deviation = Main.rand.Next(-120, 120);
                for (int i = 0; i < n; i++)
                {
                    float rotation = MathHelper.ToRadians(360 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= 5f;
                    perturbedSpeed.Y *= 5f;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, perturbedSpeed, ProjectileType<PandoraKillNpcsProj>(), Projectile.damage * 2, 0, Projectile.owner);
                }
			}
		}
		public override void AI()
		{
            Projectile.velocity *= .98f;
			Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
			Projectile.alpha += 5;
            if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}  
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(48 / 2f, 90 / 2f), Projectile.width - 48, Projectile.height - 90);
            if (collding)
            {
                Projectile.velocity *= 0.7f;
            }
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num = 0f;
            return new bool?(Collision.CheckAABBvLineCollision(
                Utils.TopLeft(targetHitbox),
                Utils.Size(targetHitbox),
                Projectile.Center - Vector2.Normalize(Projectile.velocity) * (Projectile.width / 2 * Projectile.scale),
                Projectile.Center + Vector2.Normalize(Projectile.velocity) * (Projectile.width / 2 * Projectile.scale),
                Projectile.height * Projectile.scale,
                ref num));
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 20;
            height = 20;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, -oldVelocity.X, 0.75f);
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, -oldVelocity.Y, 0.75f);
            return false;
        }
	}
}
