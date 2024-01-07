using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using FM.Content.Projectiles.Magic.Books;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class TheBookofWeakSouls : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.shoot = ProjectileType<TheBookofWeakSoulsProj>();
			Item.shootSpeed = 12f;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 5;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 2, 28, 0);
			Item.mana = 20;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/TheBookofWeakSouls_Glow").Value;
            }
		}
	}
}