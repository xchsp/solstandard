using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers;
using SolStandard.Containers.Contexts;
using SolStandard.Map;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions.Marauder
{
    public class Grapple : UnitAction
    {
        private enum ActionPhase
        {
            SelectTarget,
            SelectPlacementSpace
        }

        private ActionPhase currentPhase = ActionPhase.SelectTarget;
        private UnitEntity selectedUnitEntity;

        public Grapple() : base(
            icon: SkillIconProvider.GetSkillIcon(SkillIcon.Guillotine, new Vector2(GameDriver.CellSize)),
            name: "Grapple",
            description: "Select a unit within range, then move it to an unoccupied space in range.",
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Action),
            range: new[] {1},
            freeAction: true
        )
        {
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            switch (currentPhase)
            {
                case ActionPhase.SelectTarget:
                    if (SelectTarget(targetSlice)) currentPhase = ActionPhase.SelectPlacementSpace;
                    break;
                case ActionPhase.SelectPlacementSpace:
                    if (SelectPlacementSpace(targetSlice)) currentPhase = ActionPhase.SelectTarget;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void CancelAction()
        {
            selectedUnitEntity = null;
            currentPhase = ActionPhase.SelectTarget;
            base.CancelAction();
        }

        private bool SelectTarget(MapSlice targetSlice)
        {
            GameUnit targetUnit = UnitSelector.SelectUnit(targetSlice.UnitEntity);

            if (TargetIsUnitInRange(targetSlice, targetUnit))
            {
                MapContainer.ClearDynamicAndPreviewGrids();
                selectedUnitEntity = targetUnit.UnitEntity;
                AssetManager.MenuConfirmSFX.Play();
                GeneratePlacemementTiles(GameContext.ActiveUnit.UnitEntity.MapCoordinates);
                return true;
            }

            GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Must target unit in range!", 50);
            AssetManager.WarningSFX.Play();
            return false;
        }

        private bool SelectPlacementSpace(MapSlice targetSlice)
        {
            if (TargetTileCanPlaceUnit(targetSlice))
            {
                MapContainer.ClearDynamicAndPreviewGrids();

                Queue<IEvent> eventQueue = new Queue<IEvent>();
                eventQueue.Enqueue(new WaitFramesEvent(10));
                eventQueue.Enqueue(new MoveEntityToCoordinatesEvent(selectedUnitEntity, targetSlice.MapCoordinates));
                eventQueue.Enqueue(new PlaySoundEffectEvent(AssetManager.CombatDamageSFX));
                eventQueue.Enqueue(new WaitFramesEvent(10));
                eventQueue.Enqueue(new AdditionalActionEvent());
                GlobalEventQueue.QueueEvents(eventQueue);
                return true;
            }

            GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Must place unit in unoccupied space!", 50);
            AssetManager.WarningSFX.Play();
            return false;
        }

        private static bool TargetTileCanPlaceUnit(MapSlice targetSlice)
        {
            return UnitMovingContext.CanEndMoveAtCoordinates(targetSlice.MapCoordinates) &&
                   targetSlice.DynamicEntity != null;
        }

        private void GeneratePlacemementTiles(Vector2 origin)
        {
            base.GenerateActionGrid(origin);

            List<MapElement> targetTiles = MapContainer.GetMapElementsFromLayer(Layer.Dynamic);

            foreach (MapElement tile in targetTiles)
            {
                MapSlice tileSlice = MapContainer.GetMapSliceAtCoordinates(tile.MapCoordinates);
                if (!TargetTileCanPlaceUnit(tileSlice))
                {
                    MapContainer.GameGrid[(int) Layer.Dynamic][(int) tile.MapCoordinates.X, (int) tile.MapCoordinates.Y]
                        = null;
                }
            }
        }
    }
}