using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using ReLogic.Content;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using ParticleLibrary;
using FM.Particles;
using FM.Globals;
using FM.Content.NPCs.Bosses.Void;
using Terraria.Graphics.Shaders;

namespace FM.Content.NPCs.Bosses.Void
{
	public class VoidPortal : ModProjectile
	{
		public override string Texture => "FM/Content/NPCs/Bosses/Void/VoidPortalTex";

		public int maxTime = 300;
						
		float blackHoleRadius = 1080 / 2;
		
		public override void SetDefaults()
		{
			Projectile.width = (int)blackHoleRadius * 2;
			Projectile.height = (int)blackHoleRadius * 2;

			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;

			Projectile.timeLeft = maxTime;
			Projectile.alpha = 50;
		}
		
		public override void OnSpawn(IEntitySource source)
		{
			ParticleManager.NewParticle<HollowCircle_Misty>(Projectile.Center, Vector2.Zero, Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), 0.68f, 0, 0);
			Projectile.rotation += MathHelper.ToRadians(Main.rand.NextFloat(1, 3600));
		}

		SlotId SoundSlot;
		
		bool channelSound;
		
		float pullStrength;
		
		float swirlOpacity = 0;
		
		float spaceOpacity = 1;
		int timer = 0;
		int timerSpawn = 0;
		public override void AI()
		{
			Player player = Main.LocalPlayer;
			Projectile.ai[0]++;
			timer++;
			timerSpawn++;
			if (timer == 2)
			{
				FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
				modPlayer.Shake += 2;
				timer = 0;
			}
			if (timerSpawn == 120)
			{
				int V = 44;
				FMHelper.SpawnNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y + V, ModContent.NPCType<Void>());
			}

			spaceOpacity *= 0.93f;
			Projectile.rotation -= MathHelper.ToRadians(1.25f);
			Projectile.velocity *= 0;
			
			if (!player.active || player.dead) {
				Projectile.Kill();
			}
				
			pullStrength += 0.04f;
			
			if (Projectile.timeLeft > maxTime - 10)
				swirlOpacity += 0.1f;
			else if (Projectile.timeLeft < maxTime - (maxTime + 10))
				swirlOpacity -= 0.1f;

			float num627 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float num628 = Main.rand.NextFloat();
			Vector2 position7 = Projectile.Center + num627.ToRotationVector2() * (250f + 540 * num628);
			Vector2 vector154 = (num627 + (float)Math.PI).ToRotationVector2() * (14f + 0f * Main.rand.NextFloat() + 8f * num628);
			ParticleManager.NewParticle<BloomCircle_FadingIn>(position7, vector154.RotatedBy(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(0.25f, 0.75f), Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Main.rand.NextFloat(0.75f, 1.5f), 0, 0);
        }
		
		public override void Kill(int timeLeft)
		{
			ParticleManager.NewParticle<LensFlare>(Projectile.Center, Vector2.Zero, Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), 2f, 0, 0);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);
            int shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.NegativeDye);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            GameShaders.Armor.ApplySecondary(shader, Main.player[Main.myPlayer], null);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), -Projectile.rotation, drawOrigin, Projectile.scale / 3, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f), Projectile.rotation, drawOrigin, Projectile.scale / 2, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D extra = ModContent.Request<Texture2D>("FM/Content/NPCs/Bosses/Void/VoidPortalTex_Center").Value;
			Main.EntitySpriteDraw(extra, Projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(130, 57, 125, 255), new Color(23, 12, 64, 255), 0.56f) * Projectile.Opacity, -Projectile.rotation * 1.5f, new Vector2(extra.Width / 2, extra.Height / 2), Projectile.scale, 0, 0);

			return false;
		}
	}
}
