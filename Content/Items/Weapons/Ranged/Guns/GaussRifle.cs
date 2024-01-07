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
using FM.Content.Projectiles.Ranged.Guns;
using Terraria.Localization;

namespace FM.Content.Items.Weapons.Ranged.Guns
{
	public class GaussRifle : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.shoot = 1;
			Item.shootSpeed = 5;
			Item.useAmmo = AmmoID.Bullet;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 2;
			Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/GaussRifle") 
			{
				Volume = 0.2f,
				PitchVariance = 0.0f,
				MaxInstances = 1,
			};
			Item.autoReuse = true;
			if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("FM/Content/Items/Weapons/Ranged/Guns/GaussRifle_Glow").Value;
            }
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 2);
		}
		public override bool AltFunctionUse(Player player) => true;
        public int AttackMode;
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
            {
                Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/CooldownOver");
            }
            else
            {
                switch (AttackMode)
                {
                    case 0:
			    		Item.shootSpeed = 5;
						Item.useTime = 14;
            			Item.useAnimation = 14;
						Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/GaussRifle") 
						{
							Volume = 0.3f,
							PitchVariance = 0.0f,
							MaxInstances = 1,
						};
                        break;

                    case 1:
			    		Item.shootSpeed = 5;
						Item.useTime = 44;
            			Item.useAnimation = 44;
						Item.UseSound = new SoundStyle($"{nameof(FM)}/Assets/Sounds/Items/Guns/GaussRifle") 
						{
							Volume = 0.38f,
							PitchVariance = 0.0f,
							MaxInstances = 1,
						};
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
                        CombatText.NewText(player.getRect(), Color.LightBlue, Language.GetTextValue("Mods.FM.Items.GaussRifle.Mode1"), true, false);
                        break;
                    case 1:
                        CombatText.NewText(player.getRect(), Color.LightBlue, Language.GetTextValue("Mods.FM.Items.GaussRifle.Mode2"), true, false);
                        break;
                }
            }
			else
			{
				switch (AttackMode)
				{
					case 0:
            			for (int i = 0; i < 1; i++)
						{
							Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(2));

							Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileType<IkvRifle>(), damage, knockback, player.whoAmI);
						}
					break;

					case 1:
            			for (int i = 0; i < 4; i++)
						{
							Vector2 perturbedSpeed2 = velocity.RotatedByRandom(MathHelper.ToRadians(6));
							perturbedSpeed2 *= 1f - Main.rand.NextFloat(0.5f);
							Projectile.NewProjectile(source, position, perturbedSpeed2, ProjectileType<IkvRifle>(), damage, knockback, player.whoAmI);
						}
					break;
				}
			}
			return false;
        }
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 55f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string shotType = "";
            switch (AttackMode)
            {
                case 0:
                    shotType = Language.GetTextValue("Mods.FM.Items.GaussRifle.Mode1");
                    break;
                case 1:
                    shotType = Language.GetTextValue("Mods.FM.Items.GaussRifle.Mode2");
                    break;
            }
            TooltipLine line = new(Mod, "ShotName", shotType)
            {
                OverrideColor = Color.LightBlue,
            };
            tooltips.Add(line);
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wire, 40);
			recipe.AddRecipeGroup("FM:IronBar", 5);
			recipe.AddRecipeGroup("FM:CopperBar", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}