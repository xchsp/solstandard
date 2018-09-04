﻿using System;
using System.Collections.Generic;
using SolStandard.Utility.Monogame;

namespace SolStandard.Entity.Unit
{
    public class UnitClassBuilder
    {
        private readonly List<ITexture2D> largePortraits;
        private readonly List<ITexture2D> mediumPortraits;
        private readonly List<ITexture2D> smallPortraits;

        public UnitClassBuilder(List<ITexture2D> largePortraits, List<ITexture2D> mediumPortraits,
            List<ITexture2D> smallPortraits)
        {
            this.largePortraits = largePortraits;
            this.mediumPortraits = mediumPortraits;
            this.smallPortraits = smallPortraits;
        }

        public static List<GameUnit> GenerateUnitsFromMap(IEnumerable<UnitEntity> units,
            List<ITexture2D> largePortraitTextures,
            List<ITexture2D> mediumPortraitTextures, List<ITexture2D> smallPortraitTextures)
        {
            List<GameUnit> unitsFromMap = new List<GameUnit>();

            foreach (UnitEntity unit in units)
            {
                if (unit == null) continue;
                
                UnitClassBuilder unitBuilder = new UnitClassBuilder(largePortraitTextures, mediumPortraitTextures,
                    smallPortraitTextures);

                Team unitTeam;

                switch (unit.TiledProperties["Team"])
                {
                    case "Red":
                        unitTeam = Team.Red;
                        break;
                    case "Blue":
                        unitTeam = Team.Blue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("", unit.TiledProperties["Team"], null);
                }

                UnitClass unitClass;

                switch (unit.TiledProperties["Class"])
                {
                    case "Archer":
                        unitClass = UnitClass.Archer;
                        break;
                    case "Champion":
                        unitClass = UnitClass.Champion;
                        break;
                    case "Mage":
                        unitClass = UnitClass.Mage;
                        break;
                    case "Monarch":
                        unitClass = UnitClass.Monarch;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("", unit.TiledProperties["Class"], null);
                }

                GameUnit unitToBuild = unitBuilder.BuildUnitFromProperties(unit.Name, unitTeam, unitClass, unit, 0);
                unitsFromMap.Add(unitToBuild);
            }

            return unitsFromMap;
        }

        private GameUnit BuildUnitFromProperties(string id, Team unitTeam, UnitClass unitJobClass,
            UnitEntity mapEntity, int initiative)
        {
            string unitTeamAndClass = unitTeam.ToString() + "/" + unitJobClass.ToString();

            ITexture2D smallPortrait = FindSmallPortrait(unitTeamAndClass);
            ITexture2D mediumPortrait = FindMediumPortrait(unitTeamAndClass);
            ITexture2D largePortrait = FindLargePortrait(unitTeamAndClass);

            UnitStatistics unitStats;

            switch (unitJobClass)
            {
                case UnitClass.Archer:
                    unitStats = SelectArcherStats(initiative);
                    break;
                case UnitClass.Champion:
                    unitStats = SelectChampionStats(initiative);
                    break;
                case UnitClass.Mage:
                    unitStats = SelectMageStats(initiative);
                    break;
                case UnitClass.Monarch:
                    unitStats = SelectMonarchStats(initiative);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("unitJobClass", unitJobClass, null);
            }

            return new GameUnit(id, unitTeam, unitJobClass, mapEntity, unitStats, largePortrait, mediumPortrait,
                smallPortrait);
        }

        private static UnitStatistics SelectArcherStats(int initiative)
        {
            return new UnitStatistics(5, 4, 2, 1, 1, 3, new[] {2}, initiative);
        }

        private static UnitStatistics SelectChampionStats(int initiative)
        {
            return new UnitStatistics(7, 4, 3, 1, 1, 4, new[] {1}, initiative);
        }

        private static UnitStatistics SelectMageStats(int initiative)
        {
            return new UnitStatistics(3, 5, 1, 1, 1, 3, new[] {1, 2}, initiative);
        }

        private static UnitStatistics SelectMonarchStats(int initiative)
        {
            return new UnitStatistics(10, 2, 2, 1, 1, 2, new[] {1}, initiative);
        }

        private ITexture2D FindLargePortrait(string textureName)
        {
            return largePortraits.Find(texture => texture.MonoGameTexture.Name.Contains(textureName));
        }

        private ITexture2D FindMediumPortrait(string textureName)
        {
            return mediumPortraits.Find(texture => texture.MonoGameTexture.Name.Contains(textureName));
        }

        private ITexture2D FindSmallPortrait(string textureName)
        {
            return smallPortraits.Find(texture => texture.MonoGameTexture.Name.Contains(textureName));
        }
    }
}