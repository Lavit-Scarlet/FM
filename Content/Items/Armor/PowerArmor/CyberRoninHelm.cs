using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using FM.Content.Items.Armor.PowerArmor;
using FM.Content.Items.Weapons.Melee.Swords;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using FM.Globals;
using FM.Content.Items.Materials;

namespace FM.Content.Items.Armor.PowerArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class CyberRoninHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cyber-Ronin Helm");
            //Tooltip.SetDefault("Increases melee damage and critical strike by 12%");

            ArmorIDs.Head.Sets.DrawHead[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = true;

            //SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += .12f;
            player.GetCritChance(DamageClass.Melee) += 12;
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
                player.setBonus = "Увеличивает урон экзо-клинка на 20%, а также скорость атаки на на 12% и критический удар на 4\n" +
                "Увеличивает скорость ближнего боя на 10%, а также скорость передвижения\n" +
                "Дает иммунитет к наэлектризованный";
            }
            else
            {
                player.setBonus = "Increases Exo Blade damage by 20%, attack speed by 12%, and critical strike by 4\n" +
                "Increases melee speed by 10% and movement speed\n" +
                "Gives immunity to electrified";
            }
            player.GetAttackSpeed(DamageClass.Melee) += .10f;
            player.moveSpeed += .10f;
            if(player.HeldItem.type == ModContent.ItemType<ExoBlade>())
            {
                player.GetCritChance(DamageClass.Melee) += 4;
                player.GetDamage(DamageClass.Melee) += .20f;
                player.GetAttackSpeed(DamageClass.Melee) += .12f;
            }
            player.buffImmune[BuffID.Electrified] = true;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<ElectroBar>(), 10);
            recipe.AddRecipeGroup("FM:TitaniumBar", 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}