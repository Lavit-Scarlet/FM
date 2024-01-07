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
using FM.Content.Projectiles.Melee.Swords.LastProjs;
using Terraria.GameContent.Drawing;

namespace FM.Content.Projectiles.Melee.Swords.LastProjs
{
	public class LastProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 224;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
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
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.alpha += 5;
            if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			} 
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(90 / 2f, 224 / 2f), Projectile.width - 90, Projectile.height - 224);
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
            width = 40;
            height = 40;
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			for (int i = 0; i < 20; i++)
			{
				int num = Dust.NewDust(target.Center, target.width, target.height, 6, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 6.2f;
				Main.dust[num].scale = 2;
				Main.dust[num].noGravity = true;
			}
			for (int p = 0; p < 3; p++)
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) }, Projectile.owner);
				Dust.NewDust(Main.rand.NextVector2FromRectangle(target.Hitbox), 0, 0, 6);
			}
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<LastExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
        }
	}
}
