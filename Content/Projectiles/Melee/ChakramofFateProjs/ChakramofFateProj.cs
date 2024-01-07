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
using FM.Content.Projectiles.Melee.ChakramofFateProjs;
using FM.Effects.PrimitiveTrails;
using ReLogic.Content;


namespace FM.Content.Projectiles.Melee.ChakramofFateProjs
{
	public class ChakramofFateProj : ModProjectile, ITrailProjectile
	{
        public void DoTrailCreation(TrailManager tManager)
        {
            tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 252, 148), new Color(61, 187, 177)), new RoundCap(), new DefaultTrailPosition(), 44f, 160f, new ImageShader(ModContent.Request<Texture2D>("FM/Assets/Textures/Trails/WavyNoise", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 1.4f));
        }
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.scale = 1f;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation += 0.22f * Projectile.direction;
            if (Projectile.ai[0] == 0f)
            {
                Projectile.velocity *= 0.98f;
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 40f)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            else if (Projectile.ai[0] == 1f)
            {
                Projectile.ai[0] = 2f;
                Projectile.ai[1] = 0f;
                int y = 6;
                int z = Main.rand.Next(360);
                for (int x = 0; x < y; x++)
                {
                    Vector2 Vec = MathHelper.ToRadians((float)360 / y * x + z).ToRotationVector2();
                    SoundEngine.PlaySound(SoundID.Item68, Projectile.position);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vec * 12f, ModContent.ProjectileType<ChakramofFateEndProj>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                }
                Projectile.netUpdate = true;
            }
            else if (Projectile.ai[0] == 2f)
            {
                Player player = Main.player[Projectile.owner];
                Projectile.tileCollide = false;
                Vector2 Vec = Vector2.Normalize(player.Center - Projectile.Center) * 8;
                Projectile.velocity = (Projectile.velocity * 10f + Vec) / 11f;
                if (Projectile.Hitbox.Intersects(player.Hitbox) && Main.myPlayer == Projectile.owner)
                {
                    Projectile.Kill();
                }
            }
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 4;
            height = 4;
            fallThrough = true;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Collision.HitTiles(Projectile.position + new Vector2(Projectile.width / 4f, Projectile.height / 4f), oldVelocity, Projectile.width / 2, Projectile.height / 2);
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
	}
}
