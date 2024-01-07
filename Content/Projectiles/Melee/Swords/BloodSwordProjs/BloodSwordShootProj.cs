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
using ReLogic.Content;
using FM.Content.Projectiles.Melee.Swords.BloodSwordProjs;
using System.IO;
using Terraria.GameContent.Drawing;
using FM.Content.Buffs.Debuff;

namespace FM.Content.Projectiles.Melee.Swords.BloodSwordProjs
{
    public class BloodSwordShootProj : ModProjectile
    {
        public override string Texture => "FM/Assets/Textures/EmptyPixel";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 2;
			ProjectileID.Sets.TrailingMode[Type] = 6;
		}
        public override void SetDefaults()
		{
			Projectile.width = 170;
			Projectile.height = 170;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
		}
        public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 40, 40, 0);
		}
        public override void AI()
		{
			/*var target = Projectile.FindTargetWithinRange(400f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 10f, 0.05f);
			}*/

			if ((int)Projectile.ai[0] == 0)
			{
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				Projectile.ai[0]++;
			}

			if (Projectile.alpha > 40)
			{
				if (Projectile.extraUpdates > 0)
				{
					Projectile.extraUpdates = 0;
				}
				if (Projectile.scale > 1f)
				{
					Projectile.scale -= 0.02f;
					if (Projectile.scale < 1f)
					{
						Projectile.scale = 1f;
					}
				}
			}
			Projectile.velocity *= 0.99f;
			Projectile.rotation += Projectile.velocity.Length() * 0.03f * Projectile.direction;
			int size = 140;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
			if (collding)
			{
				Projectile.alpha += 10;
				Projectile.velocity *= 0.8f;
			}
			Projectile.alpha += 5;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
            for (int i = 0; i < 1; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 219);
                Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
			}
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<BloodSwordShootHitEffectProj>(), Projectile.damage, 0, Projectile.owner, Main.rand.NextFloat(MathHelper.TwoPi), 1f);
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<FlanricDisease>(), 120);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			var texture = TextureAssets.Projectile[Type].Value;
			var drawPosition = Projectile.Center;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			lightColor = Projectile.GetAlpha(lightColor);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			float opacity = Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, new Color(255, 0, 0) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale, SpriteEffects.None, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, Projectile.GetAlpha(lightColor) * opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

			var swish = FM.Instance.Assets.Request<Texture2D>("Content/Projectiles/Melee/Swords/BloodSwordProjs/BloodSwordShootSpinProj", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = new Color(255, 75, 75, 0) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;

			Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
			var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
			var flareOrigin = flare.Size() / 2f;
			float flareOffset = (swish.Width / 2f - 4f);
			var flareDirectionNormal = Vector2.Normalize(Projectile.velocity) * flareOffset;
			float flareDirectionDistance = 200f;
			for (int i = 0; i < 2; i++)
			{
				float swishRotation = Projectile.rotation + MathHelper.Pi * i;
				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation,
					swishOrigin,
					swishScale, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation,
					swishOrigin,
					swishScale, SpriteEffects.None, 0);

				for (int j = 0; j < 3; j++)
				{
					var flarePosition = (swishRotation + 0.6f * (j - 1)).ToRotationVector2() * flareOffset;
					float flareIntensity = Math.Max(flareDirectionDistance - Vector2.Distance(flareDirectionNormal, flarePosition), 0f) / flareDirectionDistance;
					Main.EntitySpriteDraw(
						flare,
						swishPosition + flarePosition,
						null,
						swishColor * flareIntensity * 3f * 0.4f,
						0f,
						flareOrigin,
						new Vector2(swishScale * 0.7f, swishScale * 2f) * flareIntensity, SpriteEffects.None, 0);

					Main.EntitySpriteDraw(
						flare,
						swishPosition + flarePosition,
						null,
						swishColor * flareIntensity * 3f * 0.4f,
						MathHelper.PiOver2,
						flareOrigin,
						new Vector2(swishScale * 0.8f, swishScale * 2.5f) * flareIntensity, SpriteEffects.None, 0);
				}
			}
			return false;
		}
    }
}