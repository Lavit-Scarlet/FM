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
using FM.Content.Projectiles.Ranged.Ammo;
using FM.Globals;

namespace FM.Content.Projectiles.Ranged.Ammo
{
    public class RTGrenadeProj : ModProjectile
    {
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 1;
			Projectile.aiStyle = 1;
		}
        public override void AI()
		{
			int num = 5;
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 4f)
				for (int k = 0; k < 5; k++)
				{
					int index2 = Dust.NewDust(Projectile.position, 4, 4, 31, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = 1f;
					Main.dust[index2].velocity *= 0.2f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
		}
        public override void Kill(int timeLeft)
        {
			Player player = Main.player[Projectile.owner];
			FMPlayer modPlayer = player.GetModPlayer<FMPlayer>();
			modPlayer.Shake += 4;
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 30; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[num].velocity *= 1.7f;
			}
			for (int j = 0; j < 20; j++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0, default(Color), 3.5f);
				Main.dust[num2].noGravity = true;
				Main.dust[num2].velocity *= 10f;
				num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[num2].velocity *= 6f;
			}
        }
    }
}