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
using FM.Content.NPCs.Bosses.Armagem.ArmagemProjs;

namespace FM.Content.NPCs.Bosses.Armagem.ArmagemProjs
{
	public class ArmagemOrbProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 180;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 2;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);


            Texture2D texture = ModContent.Request<Texture2D>("FM/Assets/Textures/SoftGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(232, 106, 106), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}
		public override void Kill(int timeLeft)
		{
			FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/ATTACK3"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .5f);
            int y = 8;
            int z = Main.rand.Next(360);
            for (int x = 0; x < y; x++)
            {
                Vector2 Vec = MathHelper.ToRadians((float)360 / y * x + z).ToRotationVector2();
                SoundEngine.PlaySound(SoundID.Item71, Projectile.position);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vec * 4f, ModContent.ProjectileType<ArmagemBigLaserProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            Projectile.netUpdate = true;
		}

	}
}
