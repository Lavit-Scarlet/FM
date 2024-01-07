using System.IO;
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
using FM.Base;
using FM.Content.Buffs.Debuff;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Melee.Spears.HeavenlySpearProjs
{
    public class HeavenlySpearLaser : ModProjectile
    {
        private int Counter
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private readonly int counterMax = 10;
        private int shotLength = 1200;
		private Vector2 origin;
		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(shotLength);
			writer.WriteVector2(origin);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			shotLength = reader.Read();
			origin = reader.ReadVector2();
		}

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 position = player.Center + Projectile.velocity - Main.screenPosition;
			SpriteEffects effects = Projectile.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			float rotation = Projectile.velocity.ToRotation() + ((effects == SpriteEffects.FlipHorizontally) ? MathHelper.Pi : 0);

            if (Counter > 0)
				DrawSecondaryBeam();
            return false;
        }
        private void DrawSecondaryBeam()
		{
			float quoteant = (float)Counter / counterMax;
			float initScaleY = 60;

			for (int i = 0; i < 3; i++)
			{
				Texture2D texture = Mod.Assets.Request<Texture2D>("Assets/Textures/Trails/GlowTrail").Value;
				Vector2 scale = new Vector2(shotLength, MathHelper.Lerp(initScaleY, 5, quoteant)) / texture.Size();

				Color color = Color.White;

				color = (color with { A = 0 }) * (float)(1f - quoteant);
				scale.Y -= 0.03f * i;

				Main.EntitySpriteDraw(texture, origin - Main.screenPosition, null, color, Projectile.velocity.ToRotation(), new Vector2(0, texture.Height / 2), scale, SpriteEffects.None, 0);
			}
		}
        private void CheckCollision()
        {
			Vector2 dirUnit = Vector2.Normalize(Projectile.velocity);
			for (int i = 0; i < 3; i++)
			{
				ParticleManager.NewParticle<HollowCircle_Small>(origin, Vector2.Zero, Color.White, 0.3f, 0, 0);
				ParticleManager.NewParticle<BloomCircle_Perfect>(origin, Vector2.Zero, Color.White, 2f, 0, 0);
			}
            float rotParticle = MathHelper.ToRadians(-60);
			float numberParticles = Main.rand.Next(15, 30);
			for (int i = 0; i < numberParticles; i++)
			{
				ParticleManager.NewParticle<BloomCircle>(origin, Projectile.velocity.RotatedByRandom(MathHelper.Lerp(-rotParticle, rotParticle, i / (numberParticles))) * Main.rand.NextFloat(0.1f, 0.8f), Color.White, Main.rand.NextFloat(0.5f, 1f), 0, 0);
			}
			//FMDraw.SpawnCirclePulse(origin, Color.White, 0.04f);
			Player player = Main.player[Projectile.owner];
            

			//Test tile collision
			float[] samples = new float[3];
			Collision.LaserScan(origin, dirUnit, 1, shotLength, samples);

			shotLength = 0;
			foreach (float sample in samples)
				shotLength += (int)(sample / samples.Length);

            NPC target = null;
			//Test NPC collision
			foreach (NPC npc in Main.npc)
			{
				float collisionPoint = shotLength;
				if (Collision.CheckAABBvLineCollision(npc.Hitbox.TopLeft(), npc.Hitbox.Size(), origin, origin + (dirUnit * shotLength), 1, ref collisionPoint) && npc.active && !npc.friendly)
				{
					if (collisionPoint < shotLength)
					{
						shotLength = (int)collisionPoint;
						target = npc;
					}
				}
			}
            if (target != null)
			{
				int hitDirection = target.RightOfDir(Projectile);
                BaseAI.DamageNPC(target, Projectile.damage, Projectile.knockBack, hitDirection, Projectile, crit: Projectile.HeldItemCrit());
				//target.StrikeNPC(target);
			}
			for (int i = 0; i < 2; i++)
			{
				ParticleManager.NewParticle<BloomCircle_Perfect>(origin + (dirUnit * shotLength), Vector2.Zero, Color.White, 3.4f, 0, 0);
				ParticleManager.NewParticle<HollowCircle_Small>(origin + (dirUnit * shotLength), Vector2.Zero, Color.White, 0.34f, 0, 0);
			}
        }
		//private static int RandomizeDamage(float damage) => (int)(damage * Main.rand.NextFloat(0.8f, 1.2f));
		public override void OnSpawn(IEntitySource source) => origin = Projectile.Center;

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Projectile.Center = player.Center;
			player.heldProj = Projectile.whoAmI;

			if (Counter == 0)
				CheckCollision();

			if (Counter < counterMax)
				Counter++;
			if (player.itemAnimation < 2)
				Projectile.Kill();
			
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
                Projectile.localAI[1] -= 1f / 10f;
            }
            if (Projectile.timeLeft > 10)
            {
                Projectile.localAI[1] += 1f / 10f;
            }
		}
    }
}