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
using FM.Content.Projectiles.Magic.Books.SpectralKillerProjs;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;

namespace FM.Content.Projectiles.Magic.Books.SpectralKillerProjs
{
	public class SpectralKillerPricelProj : ModProjectile
	{
        private int timer = 0;
        private int catchDamage = 0;
        private float maxHealth = 0;
        private int target = 0;
		public override void SetDefaults()
        {
			//Projectile.name = "Spectral Killer";
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.penetrate = -1;  
            Projectile.tileCollide = false;
			Projectile.timeLeft = 150;
		}
        public override void AI()
        {
            if (timer == 0)
            {
                catchDamage = Projectile.damage;
                Projectile.damage = 0;
            }
            if (timer < 40)
            {
                Vector2 speed = new Vector2(10f,10f).RotatedBy(timer * (6.28f / 40f));
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + speed * 4f, speed, ProjectileType<SpectralKillerProj>(), catchDamage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + speed.RotatedBy(3.14f) * 4f, speed.RotatedBy(3.14f), ProjectileType<SpectralKillerProj>(), catchDamage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
                Projectile.Center = Main.player[Projectile.owner].Center;
            }
            if (timer == 40)
            {
                for (int i = 0 ; i < Main.npc.Length ; i++)
                {
                    if (!Main.npc[i].friendly && Distance(Projectile.Center, Main.npc[i].Center) < 1200f)
                    {
                        if (Main.npc[i].life > maxHealth)
                        {
                            target = i;
                            maxHealth = Main.npc[i].life;
                        }
                    }
                }
            }
            if (timer > 40 && target != 0)
            {
                Projectile.Center = Main.npc[target].Center;
            }
            timer++;
        }
        private float Distance(Vector2 v1, Vector2 v2)
        {
            float dx = v1.X - v2.X;
            float dy = v1.Y - v2.Y;
            dx = (float)Math.Pow(dx,2);
            dy = (float)Math.Pow(dy,2);
            return (float)(Math.Sqrt(dx + dy));
        }
    }
}