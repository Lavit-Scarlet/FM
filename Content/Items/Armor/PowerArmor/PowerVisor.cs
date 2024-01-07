using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using FM.Content.Items.Armor.PowerArmor;
using FM.Content.Items.Weapons.Ranged.Bows;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using FM.Globals;
using FM.Content.Items.Materials;

namespace FM.Content.Items.Armor.PowerArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class PowerVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Power Visor");
            //Tooltip.SetDefault("Increases ranged damage by 14%\n" +
            //"Increases ranged critical strike by 10");

            ArmorIDs.Head.Sets.DrawHead[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = false;

            //SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += .14f;
            player.GetCritChance(DamageClass.Ranged) += 10;
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
                player.setBonus = "Увеличивает урон Электро лука на 16% и увеличивает скорость атаки на 40%\n" +
                "Дает иммунитет к наэлектризованный";
            }
            else
            {
                player.setBonus = "Increases Electro Bow damage by 16% and increases attack speed by 40%\n" +
                "Gives immunity to electrified";
            }
            player.buffImmune[BuffID.Electrified] = true;

            if(player.HeldItem.type == ModContent.ItemType<ElectroBow>())
            {
                player.GetDamage(DamageClass.Ranged) += .16f;
                player.GetAttackSpeed(DamageClass.Ranged) += .40f;
            }
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<ElectroBar>(), 6);
            recipe.AddRecipeGroup("FM:TitaniumBar", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}