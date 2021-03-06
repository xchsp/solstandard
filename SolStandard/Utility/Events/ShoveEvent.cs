﻿using Microsoft.Xna.Framework;
using SolStandard.Containers.Components.Global;
using SolStandard.Entity.Unit;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Utility.Assets;

namespace SolStandard.Utility.Events
{
    public class ShoveEvent : IEvent
    {
        private readonly GameUnit target;

        public ShoveEvent(GameUnit target)
        {
            this.target = target;
        }

        public bool Complete { get; private set; }

        public void Continue()
        {
            Vector2 actorCoordinates = GlobalContext.ActiveUnit.UnitEntity.MapCoordinates;
            Vector2 targetCoordinates = target.UnitEntity.MapCoordinates;
            Vector2 oppositeCoordinates = UnitAction.DetermineOppositeTileOfUnit(actorCoordinates, targetCoordinates);
            target.UnitEntity.SlideToCoordinates(oppositeCoordinates);
            AssetManager.CombatBlockSFX.Play();
            Complete = true;
        }
    }
}