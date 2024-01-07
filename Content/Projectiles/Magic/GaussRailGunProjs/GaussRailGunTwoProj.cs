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
using FM.Content.Projectiles.Magic.GaussRailGunProjs;
using FM.Globals;
using FM.Base;
using FM.Content.Buffs.Debuff;
using ParticleLibrary;
using FM.Particles;

namespace FM.Content.Projectiles.Magic.GaussRailGunProjs
{
    public class GaussRailGunTwoProj : ModProjectile
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<Electric>(), 120);
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
			float initScaleY = 30;

			for (int i = 0; i < 3; i++)
			{
				Texture2D texture = (i > 1) ? Mod.Assets.Request<Texture2D>("Assets/Textures/Trails/Trail_1").Value : Mod.Assets.Request<Texture2D>("Assets/Textures/Trails/GlowTrail").Value;
				Vector2 scale = new Vector2(shotLength, MathHelper.Lerp(initScaleY, 5, quoteant)) / texture.Size();

				Color color = i switch
				{
					0 => Color.LightBlue * .5f,
					1 => Color.Violet * .8f,
					_ => Color.White * .6f
				};
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
				Color color = Color.Lerp(Color.Blue, Color.White, 0.5f);
				ParticleManager.NewParticle<HollowCircle_Small2>(origin, Projectile.velocity, color, 0.28f, 0, 0);
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

			for (int i = 0; i < 12; i++)
			{
				Dust dust = Dust.NewDustPerfect(origin + (dirUnit * shotLength), 226, Vector2.Zero, 0, Color.White, Main.rand.NextFloat(1.0f, 1.5f));
				dust.velocity = -(Projectile.velocity * Main.rand.NextFloat(0.2f, 0.5f)).RotatedByRandom(0.8f);
				dust.noGravity = true;
                dust.scale = 1f;
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