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

namespace FM.Content.Projectiles.Magic
{
	public class StaffofFateProj : ModProjectile, ITrailProjectile
	{
		public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 252, 148), new Color(61, 187, 177)), new RoundCap(), new DefaultTrailPosition(), 8f, 280f, new ImageShader(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Trail_2", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 1.6f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 255) * .25f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 26f, 380f, new DefaultShader());
        }
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(cen.X);
			writer.Write(cen.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			cen.X = reader.ReadSingle();
			cen.Y = reader.ReadSingle();
		}
        public float start = 0;
		Vector2 cen = Vector2.Zero;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			target.immune[player.whoAmI] = 0;
			if (Projectile.penetrate <= 2)
			{
				Projectile.ai[1] = 25;
				Projectile.netUpdate = true; //make sure to sync the AI change
			}
		}
        public override void AI()
		{
			start = MathHelper.Lerp(start, 10, 0.05f);
			Projectile.ai[0]++;
			if (Projectile.ai[0] == 20)
			{
				if(Projectile.owner == Main.myPlayer)
				{
					cen = Projectile.DirectionTo(Main.MouseWorld) * 0.5f;
					Projectile.netUpdate = true;
				}
			}
			if (Projectile.ai[0] > 20 && Projectile.ai[0] < 50)
            {
				if(Projectile.ai[0] == 28)
				SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
				Projectile.velocity += cen * 1.5f;
            }
            else if (Projectile.ai[0] < 20)
            {
				Projectile.velocity *= 0.95f;
			}
			else if (Projectile.ai[0] > 50)
            {
				Projectile.velocity *= 0.97f;
            }
			Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 1.57f);
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("FM/Assets/Textures/SoftGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 252, 148), Projectile.rotation, drawOrigin, Projectile.scale / 1.4f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
            Color ColorDV = Color.White;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), ColorDV, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
        }
		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 20; k++)
			{
				SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
				int index = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 163, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f);
				Main.dust[index].position = Projectile.Center;
				Main.dust[index].noGravity = true;
				Main.dust[index].scale = 0.8f;
			}
		}
	}
}