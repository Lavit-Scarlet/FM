using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.Localization;
using FM.Content.NPCs.Bosses.Armagem;
using FM.Content.NPCs.Bosses.CryoGuardianBoss;
using FM.Content.NPCs.Bosses.HellGuardianBoss;
using FM.Content.NPCs.Bosses.ForestGuardian;
using FM.Content.NPCs.Bosses.PrimordialWorm;
using FM.Globals;
using FM.Content.Items.SummonsBosses;
using FM.Content.Items.Weapons.Magic;
using FM.Content.Items.Weapons.Melee.Spears;
using FM.Content.Items.Materials;
using FM.Content.Items.Weapons.Magic.Books;
using FM.Content.Items.Weapons.Melee.Swords;
using FM.Content.Items.Weapons.Melee;
using FM.Content.Items.Weapons.Ranged.Bows;
using FM.Content.Items.Weapons.Ranged.Guns;

namespace FM.CrossMod
{
    internal class WeakReferences
    {
        public static void PerformModSupport()
        {
            PerformBossChecklistSupport();
        }
        private static void PerformBossChecklistSupport()
        {
            FM mod = FM.Instance;
            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
            {
                #region Cryo Guardian
                bossChecklist.Call("LogBoss", mod, nameof(CryoGuardian), 2.5f, () => FMBossDowned.downedCryoGuardian, ModContent.NPCType<CryoGuardian>(), new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<CryoCrystal>(),
                    ["collectibles"] = new List<int>
                        {
                            ModContent.ItemType<IceFlower>(),
                            ModContent.ItemType<SpearofCryoGuardian>(),
                            ModContent.ItemType<Severus>(),
                            ModContent.ItemType<FrostyCrossbow>(),
                            ModContent.ItemType<FrostPistol>()
                        },
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("FM/CrossMod/BossChecklist/CryoGuardian").Value;
                        Vector2 centered = new(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });
                #endregion

                #region Armagem
                bossChecklist.Call("LogBoss", mod, nameof(ArmagemHead), 19f, () => FMBossDowned.downedArmagem, ModContent.NPCType<ArmagemHead>(), new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<SoulofForce>(),
                    ["collectibles"] = new List<int>
                        {
                            ModContent.ItemType<PureSoulofPower>(),
                            ModContent.ItemType<ScarletFragment>(),
                            ModContent.ItemType<FlanricCrystal>(),
                            ModContent.ItemType<VeryOldWeapon>()
                        },
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("FM/CrossMod/BossChecklist/Armagem").Value;
                        Vector2 centered = new(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });
                #endregion

                #region Forest Guardian
                bossChecklist.Call("LogBoss", mod, nameof(ForestGuardian), 0.5f, () => FMBossDowned.downedForestGuardian, ModContent.NPCType<ForestGuardian>(), new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<WoodenFigurine>(),
                    ["collectibles"] = new List<int>
                        {
                            ModContent.ItemType<ForestBook>()
                        },
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("FM/CrossMod/BossChecklist/ForestGuardian").Value;
                        Vector2 centered = new(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });
                #endregion

                #region Primordial Worm
                bossChecklist.Call("LogBoss", mod, nameof(PrimordialWormHead), 3.5f, () => FMBossDowned.downedPrimordialWorm, ModContent.NPCType<PrimordialWormHead>(), new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<PrimordialController>(),
                    ["collectibles"] = new List<int>
                        {
                            ModContent.ItemType<PrimordialThrowingAx>(),
                            ModContent.ItemType<PrimordialSword>(),
                            ModContent.ItemType<PrimordialBow>(),
                            ModContent.ItemType<PrimordialLance>(),
                            ModContent.ItemType<PrimordialStaff>()
                        },
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("FM/CrossMod/BossChecklist/PrimordialWorm").Value;
                        Vector2 centered = new(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });
                #endregion

                #region Hell Guardian
                bossChecklist.Call("LogBoss", mod, nameof(HellGuardian), 6.5f, () => FMBossDowned.downedHellGuardian, ModContent.NPCType<HellGuardian>(), new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<SuspiciousMagmaEye>(),
                    ["collectibles"] = new List<int>
                        {
                            ModContent.ItemType<StaffofTheInfernalNecromancer>(),
                            ModContent.ItemType<HellstoneBow>()
                        },
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("FM/CrossMod/BossChecklist/HellGuardian").Value;
                        Vector2 centered = new(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });
                #endregion

                /*KingSlime = 1f;
                EyeOfCthulhu = 2f;
                EaterOfWorlds = 3f; // and Brain of Cthulhu
                QueenBee = 4f;
                Skeletron = 5f;
                DeerClops = 6f;
                WallOfFlesh = 7f;
                QueenSlime = 8f;
                TheTwins = 9f;
                TheDestroyer = 10f;
                SkeletronPrime = 11f;
                Plantera = 12f;
                Golem = 13f;
                Betsy = 14f;
                EmpressOfLight = 15f;
                DukeFishron = 16f;
                LunaticCultist = 17f;
                Moonlord = 18f;*/
            }
        }
    }
}