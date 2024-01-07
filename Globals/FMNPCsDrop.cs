using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using FM.Content.Items.Weapons.Melee.Spears;
using FM.Content.Items.Materials;
using FM.Content.Items.Weapons.Melee.Swords;
using FM.Content.Items;
using FM.Content.Items.Weapons.Ranged.Knifes;
using FM.Content.Items.SummonsBosses;
using FM.Content.Items.Weapons.Ranged.Bows;
using FM.Content.Items.Accessories;
using FM.Content.Items.Weapons.Magic;
using FM.Content.Items.Weapons;
using FM.Content.Items.Weapons.Melee;
using FM.Content.Items.Weapons.Magic.Books;

namespace FM.Common
{
    public class FMNPCsDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) 
        {
            if (npc.type == 477)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunSpark>(), 6));
            }

            if (npc.type == 262)//Плантера
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Dionaea>(), 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Rosaria>(), 2));
            }

            if (npc.type == 113)//СтенаПлоти
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlushofFate>(), 1, 40, 80));
            }
            
            if (npc.type == 398)//мунлох
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeavenlyBow>(), 12));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BookofSilesia>(), 12));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeavenlyStaff>(), 12));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeavenlySpear>(), 12));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeavenlySword>(), 12));
            }

            if (npc.type == 439)//Култист
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ScarletSoul>(), 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StaffofColdBlood>(), 6));
            }

            if (npc.type == 243)//Ледяной голем
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceLaserRifle>(), 2));
            }

            if (npc.type == 474 || npc.type == 475 || npc.type == 473)//Мимик
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SpectralKnife>(), 5));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArosBow>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunaKnife>(), 10));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodthirstyClaymore>(), 12));
            }
            
            if (npc.type == 134 || npc.type == 126 || npc.type == 127 || npc.type == 125)//Жезтянки
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ScarletFragment>(), 1, 40, 80)); // 1-Шанс выподение (1 к 1 = 100%), 40 и 80 это рандомное количество от 40 до 80
            }

            if (npc.type == 150 || npc.type == 147 || npc.type == 184 || npc.type == 161 || npc.type == 431)//Ледяной кристал
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CryoCrystal>(), 90));
            }

            /*if (npc.type == 273 || npc.type == 274 || npc.type == 275 || npc.type == 276)//Синий скелет
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StaffofMigurudiya>(), 60));
            }*/

            if (npc.type == 395)//тарелка Инопришеленец
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WorldStaff>(), 15));
            }

            if (npc.type == 35)//Скелетрон
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TheBookofWeakSouls>(), 4));
            }

            if (npc.type == 6 || npc.type == -05 || npc.type == -12 || npc.type == 57 || npc.type == 47)//ПорчМобы
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HerosDemonicSword>(), 77));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Shining>(), 99));
            }

            if (npc.type == 181 || npc.type == 173 || npc.type == -22 || npc.type == -23 || npc.type == 464)//КримМобы
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Carnage>(), 77));
            }

            if(npc.type == 4)//GLAZIC
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Spitefang>(), 8));
            }

            if(npc.type == 32)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WaterStaff>(), 44));
            }
            if(npc.type == 245)//golem
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientHammer>(), 12));
            }
        }
    }
}