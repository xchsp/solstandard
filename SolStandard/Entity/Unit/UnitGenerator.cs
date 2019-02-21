﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Entity.Unit.Actions.Archer;
using SolStandard.Entity.Unit.Actions.Bard;
using SolStandard.Entity.Unit.Actions.Champion;
using SolStandard.Entity.Unit.Actions.Creeps;
using SolStandard.Entity.Unit.Actions.Lancer;
using SolStandard.Entity.Unit.Actions.Mage;
using SolStandard.Entity.Unit.Actions.Pugilist;
using SolStandard.Map.Elements;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Monogame;

namespace SolStandard.Entity.Unit
{
    public static class UnitGenerator
    {
        private enum Routine
        {
            Roam,
            Wander
        }

        private static readonly Dictionary<string, Team> TeamDictionary = new Dictionary<string, Team>
        {
            {"Red", Team.Red},
            {"Blue", Team.Blue},
            {"Creep", Team.Creep}
        };

        private static readonly Dictionary<string, Role> RoleDictionary = new Dictionary<string, Role>
        {
            {"Archer", Role.Archer},
            {"Champion", Role.Champion},
            {"Mage", Role.Mage},
            {"Lancer", Role.Lancer},
            {"Bard", Role.Bard},
            {"Pugilist", Role.Pugilist},
            {"Slime", Role.Slime},
            {"Troll", Role.Troll},
            {"Orc", Role.Orc},
            {"Merchant", Role.Merchant}
        };

        private static readonly Dictionary<string, Routine> RoutineDictionary = new Dictionary<string, Routine>
        {
            {"Roam", Routine.Roam},
            {"Wander", Routine.Wander}
        };

        public const string TmxCommanderTag = "Commander";

        public static List<GameUnit> GenerateUnitsFromMap(IEnumerable<UnitEntity> units, List<IItem> loot,
            List<ITexture2D> portraits)
        {
            List<GameUnit> unitsFromMap = new List<GameUnit>();

            foreach (UnitEntity unit in units)
            {
                if (unit == null) continue;

                Team unitTeam = TeamDictionary[unit.TiledProperties["Team"]];
                Role role = RoleDictionary[unit.TiledProperties["Class"]];
                bool commander = Convert.ToBoolean(unit.TiledProperties[TmxCommanderTag]);

                GameUnit unitToBuild =
                    BuildUnitFromProperties(unit.Name, unitTeam, role, commander, unit, portraits, loot);
                unitsFromMap.Add(unitToBuild);
            }

            return unitsFromMap;
        }

        private static GameUnit BuildUnitFromProperties(string id, Team unitTeam, Role unitJobClass, bool isCommander,
            UnitEntity mapEntity, List<ITexture2D> portraits, List<IItem> loot)
        {
            ITexture2D portrait = FindSmallPortrait(unitTeam.ToString(), unitJobClass.ToString(), portraits);

            UnitStatistics unitStats;
            List<UnitAction> unitSkills;

            switch (unitJobClass)
            {
                case Role.Archer:
                    unitStats = SelectArcherStats();
                    unitSkills = SelectArcherSkills();
                    break;
                case Role.Champion:
                    unitStats = SelectChampionStats();
                    unitSkills = SelectChampionSkills();
                    break;
                case Role.Mage:
                    unitStats = SelectMageStats();
                    unitSkills = SelectMageSkills();
                    break;
                case Role.Lancer:
                    unitStats = SelectLancerStats();
                    unitSkills = SelectLancerSkills();
                    break;
                case Role.Bard:
                    unitStats = SelectBardStats();
                    unitSkills = SelectBardSkills();
                    break;
                case Role.Pugilist:
                    unitStats = SelectPugilistStats();
                    unitSkills = SelectPugilistSkills();
                    break;
                case Role.Slime:
                    unitStats = SelectSlimeStats();
                    unitSkills = SelectCreepRoutine(mapEntity.TiledProperties);
                    break;
                case Role.Troll:
                    unitStats = SelectTrollStats();
                    unitSkills = SelectCreepRoutine(mapEntity.TiledProperties);
                    break;
                case Role.Orc:
                    unitStats = SelectOrcStats();
                    unitSkills = SelectCreepRoutine(mapEntity.TiledProperties);
                    break;
                case Role.Merchant:
                    unitStats = SelectMerchantStats();
                    unitSkills = SelectCreepRoutine(mapEntity.TiledProperties);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("unitJobClass", unitJobClass, null);
            }

            GameUnit generatedUnit =
                new GameUnit(id, unitTeam, unitJobClass, mapEntity, unitStats, portrait, unitSkills, isCommander);

            if (generatedUnit.Team == Team.Creep)
            {
                PopulateUnitInventoryAndTradeActions(mapEntity, loot, generatedUnit);
            }

            switch (generatedUnit.Role)
            {
                case Role.Champion:
                    break;
                case Role.Archer:
                    break;
                case Role.Mage:
                    break;
                case Role.Lancer:
                    break;
                case Role.Bard:
                    break;
                case Role.Pugilist:
                    break;
                case Role.Slime:
                    generatedUnit.CurrentGold += 3 + GameDriver.Random.Next(5);
                    break;
                case Role.Troll:
                    generatedUnit.CurrentGold += 14 + GameDriver.Random.Next(5);
                    break;
                case Role.Orc:
                    generatedUnit.CurrentGold += 7 + GameDriver.Random.Next(8);
                    break;
                case Role.Merchant:
                    generatedUnit.CurrentGold += 5 + GameDriver.Random.Next(10);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return generatedUnit;
        }

        private static void PopulateUnitInventoryAndTradeActions(MapEntity mapEntity, List<IItem> loot,
            GameUnit generatedUnit)
        {
            string itemNameProp = mapEntity.TiledProperties["Item"];
            if (itemNameProp == string.Empty) return;
            string[] unitInventory = itemNameProp.Split('|');


            string itemPricesProp = mapEntity.TiledProperties["ItemPrice"];
            int[] itemPrices = (itemPricesProp != string.Empty)
                ? itemPricesProp.Split('|').Select(int.Parse).ToArray()
                : new int[0];

            for (int i = 0; i < unitInventory.Length; i++)
            {
                IItem unitItem = loot.Find(item => item.Name == unitInventory[i]);

                generatedUnit.AddItemToInventory(unitItem);

                if (UnitHasItemsToTrade(mapEntity, itemPrices))
                {
                    int itemPrice = itemPrices[i];

                    if (itemPrice > 0)
                    {
                        generatedUnit.AddContextualAction(new BuyItemAction(unitItem, itemPrices[i]));
                    }
                }
            }
        }

        private static bool UnitHasItemsToTrade(MapEntity mapEntity, IReadOnlyCollection<int> itemPrices)
        {
            return Convert.ToBoolean(mapEntity.TiledProperties["WillTrade"]) && itemPrices.Count > 0;
        }

        private static UnitStatistics SelectArcherStats()
        {
            return new UnitStatistics(hp: 7, armor: 5, atk: 6, ret: 4, luck: 1, mv: 5, atkRange: new[] {2});
        }

        private static UnitStatistics SelectChampionStats()
        {
            return new UnitStatistics(hp: 7, armor: 9, atk: 5, ret: 5, luck: 1, mv: 6, atkRange: new[] {1});
        }

        private static UnitStatistics SelectMageStats()
        {
            return new UnitStatistics(hp: 6, armor: 4, atk: 6, ret: 3, luck: 1, mv: 5, atkRange: new[] {1, 2});
        }

        private static UnitStatistics SelectLancerStats()
        {
            return new UnitStatistics(hp: 8, armor: 5, atk: 6, ret: 4, luck: 1, mv: 6, atkRange: new[] {1});
        }

        private static UnitStatistics SelectBardStats()
        {
            return new UnitStatistics(hp: 8, armor: 3, atk: 3, ret: 3, luck: 2, mv: 5, atkRange: new[] {1, 2});
        }

        private static UnitStatistics SelectPugilistStats()
        {
            return new UnitStatistics(hp: 9, armor: 4, atk: 7, ret: 4, luck: 0, mv: 6, atkRange: new[] {1});
        }

        private static UnitStatistics SelectSlimeStats()
        {
            return new UnitStatistics(hp: 7, armor: 0, atk: 3, ret: 3, luck: 0, mv: 3, atkRange: new[] {1});
        }

        private static UnitStatistics SelectTrollStats()
        {
            return new UnitStatistics(hp: 20, armor: 3, atk: 6, ret: 4, luck: 2, mv: 4, atkRange: new[] {1});
        }

        private static UnitStatistics SelectOrcStats()
        {
            return new UnitStatistics(hp: 15, armor: 0, atk: 5, ret: 4, luck: 0, mv: 4, atkRange: new[] {1});
        }

        private static UnitStatistics SelectMerchantStats()
        {
            return new UnitStatistics(hp: 20, armor: 15, atk: 5, ret: 8, luck: 3, mv: 6, atkRange: new[] {1, 2});
        }

        private static List<UnitAction> SelectArcherSkills()
        {
            return new List<UnitAction>
            {
                new BasicAttack(),
                new Draw(2, 1),
                new HuntingTrap(5, 1),
                new Harpoon(2),
                new Guard(3),
                new DropGiveGoldAction(),
                new Wait()
            };
        }

        private static List<UnitAction> SelectChampionSkills()
        {
            return new List<UnitAction>
            {
                new BasicAttack(),
                new Bloodthirst(2),
                new Tackle(),
                new Shove(),
                new Atrophy(2, 2),
                new Guard(3),
                new DropGiveGoldAction(),
                new Wait()
            };
        }

        private static List<UnitAction> SelectMageSkills()
        {
            return new List<UnitAction>
            {
                new BasicAttack(),
                new Ignite(3, 3),
                new Inferno(2, 3),
                new Replace(),
                new Guard(3),
                new DropGiveGoldAction(),
                new Wait()
            };
        }

        private static List<UnitAction> SelectLancerSkills()
        {
            return new List<UnitAction>
            {
                new BasicAttack(),
                new Execute(3),
                new PoisonTip(2, 4),
                new Charge(3),
                new Guard(3),
                new DropGiveGoldAction(),
                new Wait()
            };
        }

        private static List<UnitAction> SelectBardSkills()
        {
            return new List<UnitAction>
            {
                new BasicAttack(),
                new DoubleTime(2, 1),
                new Inspire(2, 1),
                new Bulwark(2, 2),
                new Guard(3),
                new DropGiveGoldAction(),
                new Wait()
            };
        }

        private static List<UnitAction> SelectPugilistSkills()
        {
            return new List<UnitAction>
            {
                new BasicAttack(),
                new Guard(3),
                new Meditate(),
                new DropGiveGoldAction(),
                new Wait()
            };
        }

        private static List<UnitAction> SelectCreepRoutine(IReadOnlyDictionary<string, string> tiledProperties)
        {
            List<UnitAction> actions = new List<UnitAction>();

            switch (RoutineDictionary[tiledProperties["Routine"]])
            {
                case Routine.Roam:
                    actions.Add(new RoamingRoutine(Convert.ToBoolean(tiledProperties["Independent"])));
                    break;
                case Routine.Wander:
                    actions.Add(new RoamingRoutine(Convert.ToBoolean(tiledProperties["Independent"]), false));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return actions;
        }

        private static ITexture2D FindSmallPortrait(string unitTeam, string unitJobClass, List<ITexture2D> portraits)
        {
            return portraits.Find(texture =>
                texture.MonoGameTexture.Name.Contains(unitTeam) && texture.MonoGameTexture.Name.Contains(unitJobClass));
        }

        public static ITexture2D GetUnitPortrait(Role role, Team team)
        {
            return AssetManager.SmallPortraitTextures
                .Find(texture =>
                    texture.MonoGameTexture.Name.Contains(team.ToString()) &&
                    texture.MonoGameTexture.Name.Contains(role.ToString())
                );
        }

        public static GameUnit GenerateDraftUnit(Role role, Team team, bool isCommander)
        {
            //FIXME Refactor so that this doesn't duplicate so much of GenerateUnitFromProperties()
            string generatedName = NameGenerator.GenerateUnitName(role);
            const string type = "Unit";

            UnitEntity entity = GenerateUnitEntity(generatedName, type, role, team, isCommander,
                AssetManager.UnitSprites, Vector2.Zero, new Dictionary<string, string>());

            ITexture2D portrait =
                FindSmallPortrait(team.ToString(), role.ToString(), AssetManager.SmallPortraitTextures);

            UnitStatistics unitStatistics;
            List<UnitAction> unitActions;

            switch (role)
            {
                case Role.Archer:
                    unitStatistics = SelectArcherStats();
                    unitActions = SelectArcherSkills();
                    break;
                case Role.Champion:
                    unitStatistics = SelectChampionStats();
                    unitActions = SelectChampionSkills();
                    break;
                case Role.Mage:
                    unitStatistics = SelectMageStats();
                    unitActions = SelectMageSkills();
                    break;
                case Role.Lancer:
                    unitStatistics = SelectLancerStats();
                    unitActions = SelectLancerSkills();
                    break;
                case Role.Bard:
                    unitStatistics = SelectBardStats();
                    unitActions = SelectBardSkills();
                    break;
                case Role.Pugilist:
                    unitStatistics = SelectPugilistStats();
                    unitActions = SelectPugilistSkills();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("role", role, null);
            }

            GameUnit unit =
                new GameUnit(generatedName, team, role, entity, unitStatistics, portrait, unitActions, isCommander);

            return unit;
        }


        public static UnitEntity GenerateUnitEntity(string name, string type, Role role, Team team, bool isCommander,
            List<ITexture2D> unitSprites, Vector2 mapCoordinates, Dictionary<string, string> unitProperties)
        {
            ITexture2D unitSprite = FetchUnitGraphic(team.ToString(), role.ToString(), unitSprites);

            Vector2 unitScale = new Vector2(unitSprite.Width) / 2.5f;
            const int unitAnimationFrames = 4;
            const int unitAnimationDelay = 12;

            UnitSpriteSheet animatedSpriteSheet = new UnitSpriteSheet(
                unitSprite,
                unitSprite.Width / unitAnimationFrames,
                unitScale,
                unitAnimationDelay,
                false,
                Color.White
            );

            UnitEntity unitEntity = new UnitEntity(name, type,
                animatedSpriteSheet, mapCoordinates, isCommander, unitProperties);

            return unitEntity;
        }

        public static ITexture2D FetchUnitGraphic(string unitTeam, string role, List<ITexture2D> unitSprites)
        {
            string unitTeamAndClass = unitTeam + role;
            return unitSprites.Find(texture => texture.Name.Contains(unitTeamAndClass));
        }
    }
}