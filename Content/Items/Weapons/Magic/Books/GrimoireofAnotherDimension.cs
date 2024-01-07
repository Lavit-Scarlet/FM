using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using FM.Globals;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FM.Content.Projectiles.Magic.Books.GrimoireofAnotherDimensionProjs;
using Terraria.Localization;

namespace FM.Content.Items.Weapons.Magic.Books
{
	public class GrimoireofAnotherDimension : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults()
		{
			Item.damage = 135;
			Item.shoot = 1;
			Item.shootSpeed = 10;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 42;
			Item.height = 36;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.rare = 10;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(1, 0, 0, 0);
			Item.mana = 10;
			//Item.UseSound = SoundID.Item105;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Magic/Books/GrimoireofAnotherDimension_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(1, 0);
		}
		public override bool AltFunctionUse(Player player) => true;
        public int AttackMode;
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
            {
                //Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/CooldownOver");
            }
            else
            {
                switch (AttackMode)
                {
                    case 0:
			    		Item.damage = 135;
			    		Item.shootSpeed = 10;
						Item.useTime = 12;
            			Item.useAnimation = 12;
                        break;

                    case 1:
				    	Item.damage = 235;
			    		Item.shootSpeed = 10;
						Item.useTime = 12;
            			Item.useAnimation = 12;
                        break;
                }
            }
			return true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{	
			if (player.altFunctionUse == 2)
            {
                player.itemAnimationMax = 5;
                player.itemTime = 5;
                player.itemAnimation = 5;

                AttackMode++;
                if (AttackMode >= 2)
                    AttackMode = 0;

                switch (AttackMode)
                {
                    case 0:
                        CombatText.NewText(player.getRect(), Color.Red, Language.GetTextValue("Mods.FM.Items.GrimoireofAnotherDimension.Mode1"), true, false);
                        break;
                    case 1:
                        CombatText.NewText(player.getRect(), Color.Red, Language.GetTextValue("Mods.FM.Items.GrimoireofAnotherDimension.Mode2"), true, false);
                        break;
                }
            }
			else
			{
				switch (AttackMode)
				{
					case 0:
						for (int I = 0; I < 3; I++) 
						{
							float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
							Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 300f;
							position += spawnPlace;
							Vector2 velocity1 = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
							Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<GrimoireofAnotherDimensionProj>(), damage, knockback, player.whoAmI);
						}
					break;
					case 1:
						for (int I = 0; I < 1; I++) 
						{
							float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
							Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 300f;
							position += spawnPlace;
							Vector2 velocity1 = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
							Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<GrimoireofAnotherDimensionLaserProj>(), damage, knockback, player.whoAmI);
						}
					break;
				}
			}
			return false;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string shotType = "";
            switch (AttackMode)
            {
                case 0:
                    shotType = Language.GetTextValue("Mods.FM.Items.GrimoireofAnotherDimension.Mode1");
                    break;
                case 1:
                    shotType = Language.GetTextValue("Mods.FM.Items.GrimoireofAnotherDimension.Mode2");
                    break;
            }
            TooltipLine line = new(Mod, "ShotName", shotType)
            {
                OverrideColor = Color.Red,
            };
            tooltips.Add(line);
        }
	}
}