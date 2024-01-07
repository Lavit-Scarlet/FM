using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using FM.Content.Items.Armor.DarkArmor;
using FM.Content.Items.Materials;
using static Terraria.ModLoader.ModContent;

namespace FM.Content.Items.Armor.DarkArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class AcolytesHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acolyte's Hood");
            //Tooltip.SetDefault("Increases magic damage by 5%");

            ArmorIDs.Head.Sets.DrawHead[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = true;

            //SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.sellPrice(silver: 40);
            Item.rare = 4;
            Item.defense = 4;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += .05f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AcolytesMantle>() && legs.type == ModContent.ItemType<AcolytesKilt>();
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
                player.setBonus = "Увеличивает скорость магической атаки на 2% и снижает затраты маны на 10%\n" +
                "Увеличивает магический урон на 5%";
            }
            else
            {
                player.setBonus = "Increases magic attack speed by 2% and reduces mana cost by 10%\n" +
                "Increases magic damage by 5%";
            }

            //player.setBonus = Language.GetTextValue("Mods.FM.GenericTooltips.ArmorSetBonus.Acolytes.Bonus");

            player.GetAttackSpeed(DamageClass.Magic) += .02f;
            player.GetDamage(DamageClass.Magic) += .05f;
            player.manaCost -= 0.10f;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<DarkBar>(), 2);
			recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(ItemID.Bone, 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}