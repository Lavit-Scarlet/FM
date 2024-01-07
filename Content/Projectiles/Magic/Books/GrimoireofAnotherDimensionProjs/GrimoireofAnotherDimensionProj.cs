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
using FM.Content.Projectiles.Magic.Books.GrimoireofAnotherDimensionProjs;

namespace FM.Content.Projectiles.Magic.Books.GrimoireofAnotherDimensionProjs
{
	public class GrimoireofAnotherDimensionProj : ModProjectile
	{
		public const int State_Opening = 0;

		private const int State_Open = 1;

		private const int State_Closing = 2;

		private int time;

		private int State
		{
			get
			{
				return (int)Projectile.ai[0];
			}
			set
			{
				Projectile.ai[0] = value;
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.width = 98;
			Projectile.height = 98;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			int num = value.Height / Main.projFrames[Projectile.type];
			Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, (Rectangle?)new Rectangle(0, num * Projectile.frame, value.Width, num), Color.White, Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), 1f, (SpriteEffects)0, 0);
			return false;
		}
		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPvp(Player target)
		{
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return false;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
		{
			if (Projectile.localAI[1] == 0f)
			{
				Projectile.localAI[1] = 1f;
				FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/BeamSwing"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .30f, .30f);
			}
			Projectile.rotation = base.Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
			if (State == 0)
			{
				if (++Projectile.frameCounter >= 4)
				{
					Projectile.frameCounter = 0;
					if (++Projectile.frame >= Main.projFrames[Projectile.type])
					{
						Projectile.frame = Main.projFrames[Projectile.type] - 1;
						State = 1;
					}
				}
			}
			else if (State == 1)
			{
				time++;
				if (time >= 4)
				{
					FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/Beam2"), (int)Projectile.Center.X, (int)Projectile.Center.Y, .30f);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<GrimoireofAnotherDimensionShootProj>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Projectile.whoAmI);

					State = 2;
					Projectile.netUpdate = true;
				}
			}
			else if (++Projectile.frameCounter >= 4)
			{
				Projectile.frameCounter = 0;
				if (--Projectile.frame < 0)
				{
					Projectile.Kill();
				}
			}
		}
	}
}
