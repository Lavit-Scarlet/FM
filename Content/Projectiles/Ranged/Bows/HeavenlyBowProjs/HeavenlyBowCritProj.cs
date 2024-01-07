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
using FM.Content.Projectiles.Ranged.Bows.HeavenlyBowProjs;
using System.Collections.Generic;

namespace FM.Content.Projectiles.Ranged.Bows.HeavenlyBowProjs
{
	public class HeavenlyBowCritProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
			Projectile.width = 128;
			Projectile.height = 128;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 600;
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
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
		public override void AI()
		{
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Player player = Main.player[Projectile.owner];
            if (Projectile.localAI[0] == 0)
            {
                Projectile.netUpdate = true;
                Projectile.alpha -= 12;
                if (Projectile.alpha <= 0)
                {
					FMHelper.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/HeavenlyBlast"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 2f, 0.6f);
                    if (Main.myPlayer == Projectile.owner)
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<HeavenlyBowCritShootProj>(), Projectile.damage * 5, Projectile.knockBack, Main.myPlayer, Projectile.whoAmI);
                    Projectile.localAI[0] = 1;
                }
            }
            else
            {
                Projectile.localAI[0]++;
                if (Projectile.localAI[0] >= 30)
                {
                    Projectile.alpha += 20;
                    if (Projectile.alpha >= 255)
                        Projectile.Kill();
                }
            }
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }
	}
}
