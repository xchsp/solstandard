﻿using System;
using System.Collections.Generic;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Entity.Unit.Actions.Archer;
using SolStandard.Entity.Unit.Actions.Champion;
using SolStandard.Entity.Unit.Actions.Creeps;
using SolStandard.Entity.Unit.Actions.Mage;
using SolStandard.Entity.Unit.Actions.Monarch;
using SolStandard.Utility.Monogame;

namespace SolStandard.Entity.Unit
{
    public static class UnitGenerator
    {
        private static List<ITexture2D> _largePortraits;
        private static List<ITexture2D> _mediumPortraits;
        private static List<ITexture2D> _smallPortraits;

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
            {"Monarch", Role.Monarch},
            {"Slime", Role.Slime},
            {"Troll", Role.Troll},
            {"Orc", Role.Orc}
        };


        public static List<GameUnit> GenerateUnitsFromMap(IEnumerable<UnitEntity> units,
            List<ITexture2D> largePortraits, List<ITexture2D> mediumPortraits, List<ITexture2D> smallPortraits)
        {
            _largePortraits = largePortraits;
            _mediumPortraits = mediumPortraits;
            _smallPortraits = smallPortraits;

            List<GameUnit> unitsFromMap = new List<GameUnit>();

            foreach (UnitEntity unit in units)
            {
                if (unit == null) continue;

                Team unitTeam = TeamDictionary[unit.TiledProperties["Team"]];
                Role role = RoleDictionary[unit.TiledProperties["Class"]];

                GameUnit unitToBuild = BuildUnitFromProperties(unit.Name, unitTeam, role, unit);
                unitsFromMap.Add(unitToBuild);
            }

            return unitsFromMap;
        }

        private static GameUnit BuildUnitFromProperties(string id, Team unitTeam, Role unitJobClass,
            UnitEntity mapEntity)
        {
            ITexture2D smallPortrait = FindSmallPortrait(unitTeam.ToString(), unitJobClass.ToString());
            ITexture2D mediumPortrait = FindMediumPortrait(unitTeam.ToString(), unitJobClass.ToString());
            ITexture2D largePortrait = FindLargePortrait(unitTeam.ToString(), unitJobClass.ToString());

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
                case Role.Monarch:
                    unitStats = SelectMonarchStats();
                    unitSkills = SelectMonarchSkills();
                    break;
                case Role.Slime:
                    unitStats = SelectSlimeStats();
                    unitSkills = SelectSlimeSkills();
                    break;
                case Role.Troll:
                    unitStats = SelectTrollStats();
                    unitSkills = SelectTrollSkills();
                    break;
                case Role.Orc:
                    unitStats = SelectOrcStats();
                    unitSkills = SelectOrcSkills();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("unitJobClass", unitJobClass, null);
            }

            GameUnit generatedUnit = new GameUnit(id, unitTeam, unitJobClass, mapEntity, unitStats, largePortrait,
                mediumPortrait, smallPortrait, unitSkills);

            switch (generatedUnit.Role)
            {
                case Role.Champion:
                    break;
                case Role.Archer:
                    break;
                case Role.Mage:
                    break;
                case Role.Monarch:
                    break;
                case Role.Slime:
                    generatedUnit.CurrentGold += 5 + GameDriver.Random.Next(0, 5);
                    break;
                case Role.Troll:
                    generatedUnit.CurrentGold += 20 + GameDriver.Random.Next(0, 5);
                    break;
                case Role.Orc:
                    generatedUnit.CurrentGold += 15 + GameDriver.Random.Next(0, 5);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return generatedUnit;
        }

        private static UnitStatistics SelectArcherStats()
        {
            return new UnitStatistics(6, 4, 4, 3, 2, 5, new[] {2});
        }

        private static UnitStatistics SelectChampionStats()
        {
            return new UnitStatistics(7, 9, 5, 5, 1, 6, new[] {1});
        }

        private static UnitStatistics SelectMageStats()
        {
            return new UnitStatistics(5, 3, 6, 3, 2, 5, new[] {1, 2});
        }

        private static UnitStatistics SelectMonarchStats()
        {
            return new UnitStatistics(20, 0, 4, 3, 1, 5, new[] {1});
        }

        private static UnitStatistics SelectSlimeStats()
        {
            return new UnitStatistics(5, 0, 3, 3, 0, 3, new[] {1});
        }

        private static UnitStatistics SelectTrollStats()
        {
            return new UnitStatistics(10, 5, 4, 2, 2, 4, new[] {1});
        }

        private static UnitStatistics SelectOrcStats()
        {
            return new UnitStatistics(10, 0, 5, 3, 0, 4, new[] {1});
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
                new Wait()
            };
        }

        private static List<UnitAction> SelectChampionSkills()
        {
            return new List<UnitAction>
            {
                new Bloodthirst(2),
                new BasicAttack(),
                new Tackle(),
                new Shove(),
                new Atrophy(2, 2),
                new Guard(3),
                new Wait()
            };
        }

        private static List<UnitAction> SelectMageSkills()
        {
            return new List<UnitAction>
            {
                new BasicAttack(),
                new Ignite(2, 3),
                new Inferno(2, 3),
                new Replace(),
                new Guard(3),
                new Wait()
            };
        }

        private static List<UnitAction> SelectMonarchSkills()
        {
            return new List<UnitAction>
            {
                new BasicAttack(),
                new DoubleTime(2, 1),
                new Inspire(2, 1),
                new Bulwark(2, 2),
                new Wait()
            };
        }

        private static List<UnitAction> SelectSlimeSkills()
        {
            return new List<UnitAction>
            {
                new RoamingRoutine(),
                new BasicAttack()
            };
        }

        private static List<UnitAction> SelectTrollSkills()
        {
            return new List<UnitAction>
            {
                new RoamingRoutine(true),
                new BasicAttack()
            };
        }
        private static List<UnitAction> SelectOrcSkills()
        {
            return new List<UnitAction>
            {
                new RoamingRoutine(true),
                new BasicAttack()
            };
        }

        private static ITexture2D FindLargePortrait(string unitTeam, string unitJobClass)
        {
            return _largePortraits.Find(texture =>
                texture.MonoGameTexture.Name.Contains(unitTeam) && texture.MonoGameTexture.Name.Contains(unitJobClass));
        }

        private static ITexture2D FindMediumPortrait(string unitTeam, string unitJobClass)
        {
            return _mediumPortraits.Find(texture =>
                texture.MonoGameTexture.Name.Contains(unitTeam) && texture.MonoGameTexture.Name.Contains(unitJobClass));
        }

        private static ITexture2D FindSmallPortrait(string unitTeam, string unitJobClass)
        {
            return _smallPortraits.Find(texture =>
                texture.MonoGameTexture.Name.Contains(unitTeam) && texture.MonoGameTexture.Name.Contains(unitJobClass));
        }
    }
}