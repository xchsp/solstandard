using System;
using System.Collections.Generic;
using SolStandard.Containers;
using SolStandard.Containers.Contexts;
using SolStandard.Containers.Contexts.WinConditions;
using SolStandard.Entity.Unit.Actions.Lancer;
using SolStandard.Entity.Unit.Statuses;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions.Paladin
{
    public class CmdAngelicAssault : UnitAction
    {
        private readonly int cmdCost;

        private enum ActionPhase
        {
            SelectTarget,
            SelectLandingSpace
        }

        private ActionPhase currentPhase = ActionPhase.SelectTarget;
        private const MapDistanceTile.TileType ActionTileType = MapDistanceTile.TileType.Action;
        private GameUnit targetingUnit;

        public CmdAngelicAssault(int cmdCost) : base(
            icon: ObjectiveIconProvider.GetObjectiveIcon(VictoryConditions.Seize, GameDriver.CellSizeVector),
            name: $"[{cmdCost}{UnitStatistics.Abbreviation[Stats.CommandPoints]}] Angelic Assault",
            description: "Leap towards an enemy and stun them as a free action!" + Environment.NewLine +
                         "Select a target, then select a space to land on next to that target." + Environment.NewLine +
                         $"Costs {cmdCost} {UnitStatistics.Abbreviation[Stats.CommandPoints]}.",
            tileSprite: MapDistanceTile.GetTileSprite(ActionTileType),
            range: new[] {1, 2, 3},
            freeAction: true
        )
        {
            this.cmdCost = cmdCost;
        }

        public override void CancelAction()
        {
            currentPhase = ActionPhase.SelectTarget;
            base.CancelAction();
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            if (!CanAffordCommandCost(GameContext.ActiveUnit, cmdCost))
            {
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor(
                    $"This action requires {cmdCost} {UnitStatistics.Abbreviation[Stats.CommandPoints]}!", 50);
                AssetManager.WarningSFX.Play();
                return;
            }

            switch (currentPhase)
            {
                case ActionPhase.SelectTarget:
                    if (SelectTarget(targetSlice)) currentPhase = ActionPhase.SelectLandingSpace;
                    break;
                case ActionPhase.SelectLandingSpace:
                    if (SelectLandingSpace(targetSlice)) currentPhase = ActionPhase.SelectTarget;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool SelectTarget(MapSlice targetSlice)
        {
            GameUnit targetUnit = UnitSelector.SelectUnit(targetSlice.UnitEntity);

            if (TargetIsAnEnemyInRange(targetSlice, targetUnit))
            {
                if (!LeapStrike.SpaceAroundUnitIsEntirelyObstructed(targetUnit))
                {
                    MapContainer.ClearDynamicAndPreviewGrids();
                    targetingUnit = targetUnit;
                    LeapStrike.CreateLandingSpacesAroundTarget(ActionTileType, targetUnit.UnitEntity.MapCoordinates);
                    AssetManager.MenuConfirmSFX.Play();
                    return true;
                }

                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("No space to land!", 50);
                AssetManager.WarningSFX.Play();
                return false;
            }

            GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Not an enemy in range!", 50);
            AssetManager.WarningSFX.Play();
            return false;
        }

        private bool SelectLandingSpace(MapSlice targetSlice)
        {
            if (targetSlice.DynamicEntity != null && !LeapStrike.CoordinatesAreObstructed(targetSlice.MapCoordinates))
            {
                GameContext.ActiveUnit.RemoveCommandPoints(cmdCost);
                MapContainer.ClearDynamicAndPreviewGrids();

                Queue<IEvent> eventQueue = new Queue<IEvent>();
                eventQueue.Enqueue(new WaitFramesEvent(10));
                eventQueue.Enqueue(new MoveEntityToCoordinatesEvent(GameContext.ActiveUnit.UnitEntity,
                    targetSlice.MapCoordinates));
                eventQueue.Enqueue(new PlaySoundEffectEvent(AssetManager.CombatDamageSFX));
                eventQueue.Enqueue(new WaitFramesEvent(10));
                eventQueue.Enqueue(new CastStatusEffectEvent(targetingUnit, new ImmobilizedStatus(1)));
                eventQueue.Enqueue(new WaitFramesEvent(30));
                eventQueue.Enqueue(new AdditionalActionEvent());
                GlobalEventQueue.QueueEvents(eventQueue);
                return true;
            }

            GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Invalid landing space!", 50);
            AssetManager.WarningSFX.Play();
            return false;
        }
    }
}