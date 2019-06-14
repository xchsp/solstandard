using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers;
using SolStandard.Containers.Contexts;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions.Pugilist
{
    public class Suplex : UnitAction
    {
        //TODO Add unique icon for skill
        public Suplex() : base(
            icon: SkillIconProvider.GetSkillIcon(SkillIcon.BasicAttack, new Vector2(GameDriver.CellSize)),
            name: "Suplex",
            description: "Flip an enemy behind you if the opposite space is unoccupied, then attack.",
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Attack),
            range: new[] {1},
            freeAction: false
        )
        {
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            GameUnit actingUnit = GameContext.ActiveUnit;
            GameUnit targetUnit = UnitSelector.SelectUnit(targetSlice.UnitEntity);

            if (TargetIsAnEnemyInRange(targetSlice, targetUnit))
            {
                Vector2 oppositeCoordinates = DetermineOppositeTileOfUnit(targetUnit.UnitEntity.MapCoordinates,
                    actingUnit.UnitEntity.MapCoordinates);
                MapSlice oppositeSlice = MapContainer.GetMapSliceAtCoordinates(oppositeCoordinates);

                if (OppositeTileIsMovable(oppositeSlice))
                {
                    MapContainer.ClearDynamicAndPreviewGrids();

                    Queue<IEvent> eventQueue = new Queue<IEvent>();
                    eventQueue.Enqueue(new MoveEntityToCoordinatesEvent(targetUnit.UnitEntity, oppositeCoordinates));
                    eventQueue.Enqueue(new WaitFramesEvent(10));
                    eventQueue.Enqueue(new StartCombatEvent(targetUnit));
                    GlobalEventQueue.QueueEvents(eventQueue);
                }
                else
                {
                    GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Opposite tile is obstructed!", 50);
                    AssetManager.WarningSFX.Play();
                }
            }
            else
            {
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Not an enemy in range!", 50);
                AssetManager.WarningSFX.Play();
            }
        }

        private static bool OppositeTileIsMovable(MapSlice targetSlice)
        {
            return UnitMovingContext.CanEndMoveAtCoordinates(targetSlice.MapCoordinates);
        }
    }
}