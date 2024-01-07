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
using FM.Content.Projectiles.Magic.Books.BookofSilesiaProjs;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class BookofSilesia : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 140;
			Item.shoot = ModContent.ProjectileType<BookofSilesiaProj>();
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.channel = true;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.rare = 10;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/HeavenlyShoot2") 
			{
				Volume = 0.5f,
				Pitch = 0.1f
			};
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 25, 26, 0);
			Item.mana = 10;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/BookofSilesia_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 6;
        }
	}
}