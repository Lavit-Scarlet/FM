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


namespace FM.Content.Projectiles.Melee.Swords.SpectraliburProjs
{
	public class SpectraliburShootSpiritProj : ModProjectile
	{
		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
        }
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}
		public override Color? GetAlpha(Color lightColor) 
		{
			return new Color(196, 247, 255, 0) * Projectile.Opacity;
		}
		public override bool PreDraw(ref Color lightColor) 
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) 
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.alpha += 2;
            if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
			if (Projectile.owner == Main.myPlayer)
			{
				if (Projectile.velocity.Length() != 0f)
				{
					Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				}
				Projectile.ai[1] = Main.rand.Next(-4, 4);
				Projectile.netUpdate = true;
			}
			if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
            }
			Projectile.netUpdate = true;
			NPC pre = null;
			if (FMHelper.ClosestNPC(ref pre, 600, Projectile.Center, true))
			{
				float direction1 = Projectile.velocity.ToRotation();
				direction1.SlowRotation((pre.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(2));
				Projectile.velocity = FMHelper.PolarVector(Projectile.velocity.Length(), direction1);
			
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath39, Projectile.position);
			for (int num623 = 0; num623 < 20; num623++)
			{
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 3f;
			}
		}
	}
}
