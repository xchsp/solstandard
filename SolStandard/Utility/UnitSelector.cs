﻿using System.Linq;
using Microsoft.Xna.Framework;
using SolStandard.Containers.Components.Global;
using SolStandard.Entity.Unit;
using SolStandard.Map.Elements;

namespace SolStandard.Utility
{
    public static class UnitSelector
    {
        public static GameUnit SelectUnit(MapEntity unit)
        {
            return unit == null ? null : GlobalContext.Units.First(gameUnit => gameUnit.UnitEntity == unit);
        }

        public static UnitEntity FindOtherUnitEntityAtCoordinates(Vector2 coordinates, MapEntity excludedEntity)
        {
            foreach (GameUnit unit in GlobalContext.Units)
            {
                if (unit.UnitEntity != null && unit.UnitEntity != excludedEntity)
                {
                    if (unit.UnitEntity.MapCoordinates == coordinates) return unit.UnitEntity;
                }
            }

            return null;
        }
    }
}