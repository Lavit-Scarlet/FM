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
using FM.Content.Projectiles.Melee.Spears.TerraSpearProjs;

namespace FM.Content.Projectiles.Melee.Spears.TerraSpearProjs
{
    public class TerraSpearShootProj : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 2;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int num2 = num * Projectile.frame;
            Rectangle rectangle = new(0, num2, texture2D.Width, num);
            for (int j = 0; j < Projectile.oldPos.Length; j += 2)
            {
                Color ColorDV = new Color(44, 217, 0); ColorDV.A = 0;

                Color color = ColorDV * (600f / Projectile.oldPos.Length * j);
                float Mult = 1f - j / (float)Projectile.oldPos.Length;
                Vector2 Scale = new Vector2(Projectile.scale * 1f, Projectile.scale * 1.8f);//длина, ширина
                Scale *= Mult;
                Mult *= 1f - Projectile.alpha / 255f;
                Vector2 vector = Projectile.oldPos[j] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                color *= Mult; 
                Main.spriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), color, Projectile.rotation, Utils.Size(rectangle) / 2f, Scale, 0, 0f);
            }
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = (float)Projectile.velocity.ToRotation() + 1.57f;
            for (int k = 0; k < 4; k++)
            {
                int index = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 107, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f);
                Main.dust[index].position = Projectile.Center;
                Main.dust[index].scale = 1.2f;
                Main.dust[index].noGravity = true;
            }				
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TerraSpearHitEffect>(), Projectile.damage, 0, Projectile.owner, Main.rand.NextFloat(MathHelper.TwoPi), 1f);
            SpawnProj();
        }

        private void SpawnProj()
		{
			for (int I = 0; I < 3; I++) 
			{
				Player player = Main.player[Projectile.owner];
		    	{
	     			float positionX = Main.rand.Next(-80, 80);
	     			float positionY = Main.rand.Next(-60, 0);
		    		int a = Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.Center.X + positionX, player.Center.Y + positionY, 0f, 0f, ProjectileType<TerraSpearHitNpcProj>(), Projectile.damage / 2, 1f, player.whoAmI);
		    		Vector2 vector2_2 = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.projectile[a].Center) * 10;
		    		Main.projectile[a].velocity = vector2_2;
	    		}
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 107, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}
		}
    }
}