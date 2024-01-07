using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Effects.PrimitiveTrails;
using FM.Globals;
using FM.Content.Items.Weapons.Melee;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;
using FM.Effects;
using System.Collections.Generic;

namespace FM.Content.Projectiles.Melee
{
	public class AncientHammerProj : ModProjectile
	{

		private double UseCounter
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = (float)value;
		}

		private double Radians
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = (float)value;
		}
		private const int Size = 68;
		private float distance = Size;

		private bool ReverseSwing => Projectile.velocity.X < 0;

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = Projectile.height = 68;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Main.player[Projectile.owner].Center, Projectile.Center) ? true : base.Colliding(projHitbox, targetHitbox);

		//public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => hitDirection = Math.Sign(target.Center.X - Main.player[Projectile.owner].Center.X);
        private readonly int NUMPOINTS = 10;
        public Color baseColor = new(255, 255, 100);
        public Color endColor = new(255, 100, 0);
        public Color EdgeColor = new(255, 255, 0);
        private List<Vector2> cache;
        private List<Vector2> cache2;
        private DanTrail trail;
        private DanTrail trail2;
        private readonly float thickness = 3f;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			player.heldProj = Projectile.whoAmI;

			int degrees = (int)((double)(UseCounter / player.itemAnimationMax * -180) + 55) * player.direction * (int)player.gravDir;
			if (ReverseSwing)
				degrees = (int)((float)Math.PI - (int)((double)(UseCounter / player.itemAnimationMax * -180) + 55) * player.direction * (int)player.gravDir);
			if (player.direction == 1)
				degrees += 180;

			Radians = degrees * (Math.PI / 180);
			float amount = 0.2f * (player.HeldItem.useTime / player.itemTimeMax);
			UseCounter = MathHelper.Lerp((float)UseCounter, player.itemAnimationMax, amount);

			Projectile.position.X = player.Center.X - (int)(Math.Cos(Radians * 0.96) * Size) - (Projectile.width / 2);
			Projectile.position.Y = player.Center.Y - (int)(Math.Sin(Radians * 0.96) * Size) - (Projectile.height / 2);

			if (player.itemTime < 2)
				Projectile.active = false;
			player.itemRotation = MathHelper.WrapAngle((float)Radians + ((player.direction < 0) ? 0 : MathHelper.Pi));

			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)Radians + 1.57f);
			player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (float)Radians + 1.57f);

			if (UseCounter < (player.itemTimeMax - 1)) //Do fancy dusts
			{
				Vector2 dustPos = player.Center + (player.DirectionTo(Projectile.Center) * (distance + 8));
				Dust dust = Dust.NewDustPerfect(dustPos, 269, Vector2.Zero);
				dust.velocity = Vector2.Zero;
				dust.noGravity = false;
			}

			if (distance < Size)
				distance++;

			int maxDist = Size - 4;
			bool collided = false;
			//Allow the projectile to be pushed back by solid tiles
			for (int i = 0; i < Size; i++)
			{
				Vector2 position = player.Center + (player.DirectionTo(Projectile.Center) * (distance - 8));
				if (WorldGen.SolidOrSlopedTile(Main.tile[(position / 16).ToPoint()]) && distance > maxDist)
				{
					collided = true;
					distance--;
				}
				else
				{
					if (collided)
					break;
				}
			}
			if (Main.netMode != NetmodeID.Server)
            {
                TrailHelper.ManageBasicCaches(ref cache, ref cache2, NUMPOINTS, Projectile.Center);
                TrailHelper.ManageBasicTrail(ref cache, ref cache2, ref trail, ref trail2, NUMPOINTS, Projectile.Center, baseColor, endColor, EdgeColor, thickness);
            }
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];

			SpriteEffects effects = ((player.direction * (int)player.gravDir * Projectile.velocity.X) < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			float rotation = (float)Radians + 3.9f + ((effects == SpriteEffects.FlipHorizontally) ? MathHelper.PiOver2 : 0);

			float quoteant = (float)(distance / Size);

			Vector2 origin = new Vector2(Size * (float)(1f - quoteant), Size * quoteant);
			if (effects == SpriteEffects.FlipHorizontally)
				origin = new Vector2(Size, Size) * quoteant;

			Texture2D trail4 = ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/Slash").Value;

			quoteant = (float)((float)UseCounter / player.itemAnimationMax);
			Color color = Color.Lerp(Color.DarkRed, Color.DarkRed, quoteant) with { A = 0 };
			Vector2 scale = new Vector2((1f - quoteant) * 0f, 0f) * Projectile.scale;
			Vector2 trailPos = player.Center + (player.DirectionTo(Projectile.Center) * (distance - 14));

			//Draw the trail
			Main.EntitySpriteDraw(trail4, trailPos - Main.screenPosition, null, color, (float)Radians - 4.71f, new Vector2(trail4.Width * ((effects == SpriteEffects.FlipHorizontally) ? 0 : 1), trail4.Height / 2), scale, effects, 0);
			//Draw the projectile
			Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, player.Center - Main.screenPosition, new Rectangle(0, 0, Size, Size), Projectile.GetAlpha(Color.White), rotation, origin, Projectile.scale, effects, 0);

			return false;
		}
	}
}