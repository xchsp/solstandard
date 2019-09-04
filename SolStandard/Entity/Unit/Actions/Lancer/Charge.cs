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

namespace SolStandard.Entity.Unit.Actions.Lancer
{
    public class Charge : UnitAction
    {
        private readonly int chargeDistance;

        public Charge(IRenderable icon, string skillName, int chargeDistance) : base(
            icon: icon,
            name: skillName,
            description: "Dash towards a target and attack! Cannot move through obstacles or other units.",
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Attack),
            range: new[] {chargeDistance},
            freeAction: false
        )
        {
            this.chargeDistance = chargeDistance;
        }

        public Charge(int chargeDistance) : this(
            SkillIconProvider.GetSkillIcon(SkillIcon.Charge, GameDriver.CellSizeVector),
            "Charge",
            chargeDistance
        )
        {
        }

        public override void GenerateActionGrid(Vector2 origin, Layer mapLayer = Layer.Dynamic)
        {
            List<MapDistanceTile> attackTiles = new List<MapDistanceTile>();


            for (int i = chargeDistance; i > 1; i--)
            {
                (float originX, float originY) = origin;
                Vector2 northTile = new Vector2(originX, originY - i);
                Vector2 southTile = new Vector2(originX, originY + i);
                Vector2 eastTile = new Vector2(originX + i, originY);
                Vector2 westTile = new Vector2(originX - i, originY);
                AddTileWithinMapBounds(attackTiles, northTile, i, TileSprite);
                AddTileWithinMapBounds(attackTiles, southTile, i, TileSprite);
                AddTileWithinMapBounds(attackTiles, eastTile, i, TileSprite);
                AddTileWithinMapBounds(attackTiles, westTile, i, TileSprite);
            }

            AddAttackTilesToGameGrid(attackTiles, mapLayer);
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            GameUnit targetUnit = UnitSelector.SelectUnit(targetSlice.UnitEntity);

            if (TargetIsAnEnemyInRange(targetSlice, targetUnit))
            {
                if (!PathIsObstructed(targetSlice, targetUnit))
                {
                    Queue<IEvent> eventQueue = PathingUtil.MoveToCoordinates(
                        GameContext.ActiveUnit,
                        targetUnit.UnitEntity.MapCoordinates,
                        true,
                        false,
                        8
                    );
                    eventQueue.Enqueue(new WaitFramesEvent(10));
                    eventQueue.Enqueue(new StartCombatEvent(targetUnit));
                    GlobalEventQueue.QueueEvents(eventQueue);
                }
                else
                {
                    GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Target is obstructed!", 50);
                    AssetManager.WarningSFX.Play();
                }
            }
            else
            {
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Not an enemy in range!", 50);
                AssetManager.WarningSFX.Play();
            }
        }

        public static Queue<IEvent> MoveToTarget(Queue<IEvent> eventQueue, Vector2 origin, MapSlice targetSlice,
            int frameDelay)
        {
            if (TargetIsNorth(targetSlice))
            {
                int distanceToTarget = Convert.ToInt32(origin.Y - targetSlice.MapCoordinates.Y);

                for (int northDistance = 1; northDistance < distanceToTarget; northDistance++)
                {
                    Vector2 coordinatesToMove = new Vector2(origin.X, origin.Y - northDistance);

                    eventQueue.Enqueue(
                        new MoveEntityToCoordinatesEvent(GameContext.ActiveUnit.UnitEntity, coordinatesToMove)
                    );
                    eventQueue.Enqueue(new WaitFramesEvent(frameDelay));
                }
            }

            if (TargetIsSouth(targetSlice))
            {
                int distanceToTarget = Convert.ToInt32(targetSlice.MapCoordinates.Y - origin.Y);

                for (int southDistance = 1; southDistance < distanceToTarget; southDistance++)
                {
                    Vector2 coordinatesToMove = new Vector2(origin.X, origin.Y + southDistance);
                    eventQueue.Enqueue(
                        new MoveEntityToCoordinatesEvent(GameContext.ActiveUnit.UnitEntity, coordinatesToMove)
                    );
                    eventQueue.Enqueue(new WaitFramesEvent(frameDelay));
                }
            }

            if (TargetIsEast(targetSlice))
            {
                int distanceToTarget = Convert.ToInt32(targetSlice.MapCoordinates.X - origin.X);

                for (int eastDistance = 1; eastDistance < distanceToTarget; eastDistance++)
                {
                    Vector2 coordinatesToMove = new Vector2(origin.X + eastDistance, origin.Y);
                    eventQueue.Enqueue(
                        new MoveEntityToCoordinatesEvent(GameContext.ActiveUnit.UnitEntity, coordinatesToMove)
                    );
                    eventQueue.Enqueue(new WaitFramesEvent(frameDelay));
                }
            }

            if (TargetIsWest(targetSlice))
            {
                int distanceToTarget = Convert.ToInt32(origin.X - targetSlice.MapCoordinates.X);

                for (int westDistance = 1; westDistance < distanceToTarget; westDistance++)
                {
                    Vector2 coordinatesToMove = new Vector2(origin.X - westDistance, origin.Y);
                    eventQueue.Enqueue(
                        new MoveEntityToCoordinatesEvent(GameContext.ActiveUnit.UnitEntity, coordinatesToMove)
                    );
                    eventQueue.Enqueue(new WaitFramesEvent(frameDelay));
                }
            }

            return eventQueue;
        }


        public static bool PathIsObstructed(MapSlice targetSlice, GameUnit targetUnit)
        {
            if (TargetIsNorth(targetSlice))
            {
                int distanceToTarget = Convert.ToInt32(
                    GameContext.ActiveUnit.UnitEntity.MapCoordinates.Y - targetSlice.MapCoordinates.Y
                );

                for (int northDistance = 1; northDistance < distanceToTarget; northDistance++)
                {
                    Vector2 coordinatesToCheck = new Vector2(
                        GameContext.ActiveUnit.UnitEntity.MapCoordinates.X,
                        GameContext.ActiveUnit.UnitEntity.MapCoordinates.Y - northDistance
                    );
                    MapSlice sliceToCheck = MapContainer.GetMapSliceAtCoordinates(coordinatesToCheck);

                    if (!UnitMovingContext.CanEndMoveAtCoordinates(sliceToCheck.MapCoordinates)) return true;
                    if (SliceIsAtTargetUnit(sliceToCheck, targetUnit)) break;
                }
            }

            if (TargetIsSouth(targetSlice))
            {
                int distanceToTarget = Convert.ToInt32(
                    targetSlice.MapCoordinates.Y - GameContext.ActiveUnit.UnitEntity.MapCoordinates.Y
                );

                for (int southDistance = 1; southDistance < distanceToTarget; southDistance++)
                {
                    Vector2 coordinatesToCheck = new Vector2(
                        GameContext.ActiveUnit.UnitEntity.MapCoordinates.X,
                        GameContext.ActiveUnit.UnitEntity.MapCoordinates.Y + southDistance
                    );
                    MapSlice sliceToCheck = MapContainer.GetMapSliceAtCoordinates(coordinatesToCheck);

                    if (!UnitMovingContext.CanEndMoveAtCoordinates(sliceToCheck.MapCoordinates)) return true;
                    if (SliceIsAtTargetUnit(sliceToCheck, targetUnit)) break;
                }
            }

            if (TargetIsEast(targetSlice))
            {
                int distanceToTarget = Convert.ToInt32(
                    targetSlice.MapCoordinates.X - GameContext.ActiveUnit.UnitEntity.MapCoordinates.X
                );

                for (int eastDistance = 1; eastDistance < distanceToTarget; eastDistance++)
                {
                    Vector2 coordinatesToCheck = new Vector2(
                        GameContext.ActiveUnit.UnitEntity.MapCoordinates.X + eastDistance,
                        GameContext.ActiveUnit.UnitEntity.MapCoordinates.Y
                    );
                    MapSlice sliceToCheck = MapContainer.GetMapSliceAtCoordinates(coordinatesToCheck);

                    if (!UnitMovingContext.CanEndMoveAtCoordinates(sliceToCheck.MapCoordinates)) return true;
                    if (SliceIsAtTargetUnit(sliceToCheck, targetUnit)) break;
                }
            }

            if (TargetIsWest(targetSlice))
            {
                int distanceToTarget = Convert.ToInt32(
                    GameContext.ActiveUnit.UnitEntity.MapCoordinates.X - targetSlice.MapCoordinates.X
                );

                for (int westDistance = 1; westDistance < distanceToTarget; westDistance++)
                {
                    Vector2 coordinatesToCheck = new Vector2(
                        GameContext.ActiveUnit.UnitEntity.MapCoordinates.X - westDistance,
                        GameContext.ActiveUnit.UnitEntity.MapCoordinates.Y
                    );
                    MapSlice sliceToCheck = MapContainer.GetMapSliceAtCoordinates(coordinatesToCheck);

                    if (!UnitMovingContext.CanEndMoveAtCoordinates(sliceToCheck.MapCoordinates)) return true;
                    if (SliceIsAtTargetUnit(sliceToCheck, targetUnit)) break;
                }
            }

            return false;
        }

        private static bool SliceIsAtTargetUnit(MapSlice sliceToCheck, GameUnit targetUnit)
        {
            return sliceToCheck.UnitEntity != null && sliceToCheck.UnitEntity == targetUnit.UnitEntity;
        }

        private static bool TargetIsNorth(MapSlice targetSlice)
        {
            return targetSlice.MapCoordinates.Y < GameContext.ActiveUnit.UnitEntity.MapCoordinates.Y;
        }

        private static bool TargetIsSouth(MapSlice targetSlice)
        {
            return targetSlice.MapCoordinates.Y > GameContext.ActiveUnit.UnitEntity.MapCoordinates.Y;
        }

        private static bool TargetIsEast(MapSlice targetSlice)
        {
            return targetSlice.MapCoordinates.X > GameContext.ActiveUnit.UnitEntity.MapCoordinates.X;
        }

        private static bool TargetIsWest(MapSlice targetSlice)
        {
            return targetSlice.MapCoordinates.X < GameContext.ActiveUnit.UnitEntity.MapCoordinates.X;
        }


        public static void AddTileWithinMapBounds(ICollection<MapDistanceTile> tiles, Vector2 tileCoordinates,
            int distance, IRenderable tileSprite)
        {
            if (GameMapContext.CoordinatesWithinMapBounds(tileCoordinates))
            {
                tiles.Add(new MapDistanceTile(tileSprite, tileCoordinates, distance));
            }
        }

        public static void AddAttackTilesToGameGrid(IEnumerable<MapDistanceTile> visitedTiles, Layer layer)
        {
            foreach (MapDistanceTile tile in visitedTiles)
            {
                MapContainer.GameGrid[(int) layer][(int) tile.MapCoordinates.X, (int) tile.MapCoordinates.Y] = tile;
            }
        }
    }
}