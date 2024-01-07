using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using FM.Content.Items.Armor.PowerArmor;
using FM.Content.Items.Weapons.Magic;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using FM.Common;
using FM.Content.Items.Materials;

namespace FM.Content.Items.Armor.PowerArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class HolographicHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holographic Helmet");
            //Tooltip.SetDefault("Increases magic damage by 12%\n" +
            //"Increases magic critical strike by 5\n" + 
            //"Increases mana by 40");

            ArmorIDs.Head.Sets.DrawHead[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = true;
            ArmorIDs.Head.Sets.DrawFullHair[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = false;
            ArmorIDs.Head.Sets.DrawHatHair[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = false;

            //SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.sellPrice(silver: 75);
            Item.rare = 6;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += .12f;
            player.GetCritChance(DamageClass.Magic) += 5;
            player.statManaMax2 += 40;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PowerBreastplate>() && legs.type == ModContent.ItemType<PowerBoots>();
        }
        private string GetLang()
		{ 
            var culture = Language.ActiveCulture.Name;
            return culture;
		}

        public override void UpdateArmorSet(Player player)
        {
            if (GetLang() == "ru-RU")
            {
                player.setBonus = "Увеличивает урон Рейлгана Гаусса на 25% и увеличивает скорость атаки на 20%\n" +
                "Рейлган Гаусса не потребляет ману\n" +
                "Дает иммунитет к наэлектризованный";
            }
            else
            {
                player.setBonus = "Increases Railgun Gauss damage by 25% and increases attack speed by 20%\n" +
                "Railgun Gauss does not consume mana\n" +
                "Gives immunity to electrified";
            }
            player.buffImmune[BuffID.Electrified] = true;

            if(player.HeldItem.type == ModContent.ItemType<GaussRailGun>())
            {
                player.GetDamage(DamageClass.Magic) += .25f;
                player.manaCost -= 1f;
                player.GetAttackSpeed(DamageClass.Magic) += .20f;
            }
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<ElectroBar>(), 4);
            recipe.AddRecipeGroup("FM:TitaniumBar", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}