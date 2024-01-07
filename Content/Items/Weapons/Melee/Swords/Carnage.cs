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
using FM.Content.Projectiles.Melee.Swords;
using System;
using ReLogic.Content;

namespace FM.Content.Items.Weapons.Melee.Swords
{
	public class Carnage : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 1, 10, 0);
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox) 
		{
	    	int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5);
			Main.dust[dust].noGravity = true;
		}
		public int randHeal;
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			float theta = (float)Main.rand.NextDouble() * 3.14f * 2;
			float mag = 80;

			if(Main.rand.Next(4) == 0)
			{
    			theta = (float)Main.rand.NextDouble() * 3.14f * 2;
    			mag = 80;
    			Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X + (int)(mag * Math.Cos(theta)), target.Center.Y + (int)(mag * Math.Sin(theta)), -8 * (float)Math.Cos(theta), -8 * (float)Math.Sin(theta), ProjectileType<CarnageProj>(), 12, 0, Main.myPlayer);
			}
			randHeal = Main.rand.Next(1, 6);
			if (target.life <= 0)
			{
				player.statLife += randHeal;
	    		player.HealEffect(randHeal);
			}
		}
	}
}
