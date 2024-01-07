using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using FM.Content.Items.Armor.PowerArmor;
using FM.Content.Items.Weapons.Summon;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using FM.Globals;
using FM.Content.Items.Materials;

namespace FM.Content.Items.Armor.PowerArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class VisorOverlord : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Visor Overlord");
            //Tooltip.SetDefault("Increases minion damage by 12%\n" +
            //"Increases minion slot by 2");

            ArmorIDs.Head.Sets.DrawHead[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = true;
            ArmorIDs.Head.Sets.DrawFullHair[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = false;
            ArmorIDs.Head.Sets.DrawHatHair[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = true;

            //SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += .12f;
            player.maxMinions += 2;
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
                player.setBonus = "Увеличивает урон режущего посоха на 25%\n" +
                "Увеличивает миньон слот на 1\n" +
                "Дает иммунитет к наэлектризованный";
            }
            else
            {
                player.setBonus = "Increases the damage of the Cutting Staff by 25%\n" +
                "Increases minion slot by 1\n" +
                "Gives immunity to electrified";
            }

            player.buffImmune[BuffID.Electrified] = true;
            player.maxMinions += 1;

            if(player.HeldItem.type == ModContent.ItemType<CutterStaff>())
            {
                player.GetDamage(DamageClass.Summon) += .25f;
            }
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<ElectroBar>(), 8);
            recipe.AddRecipeGroup("FM:TitaniumBar", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}