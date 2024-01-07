using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.Audio;

namespace FM.Content.Items.Weapons.Ranged.Guns.GoblinInvention
{
	public class RTSnaiper : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.shoot = 1;
			Item.shootSpeed = 30;
			Item.useAmmo = AmmoID.Bullet;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 1, 80, 0);
			Item.rare = 3;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/RTSniper") 
			{
				Volume = 0.55f,
				PitchVariance = 0.0f,
				MaxInstances = 1,
			};
			Item.autoReuse = true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
		public override void HoldItem(Player player)
		{
			player.scope = true;
		}
	}
}