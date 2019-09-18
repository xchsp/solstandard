using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SolStandard.Containers;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.General;
using SolStandard.Map;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions.Mage
{
    public class Frostbite : LayTrap
    {
        public Frostbite(int damage, int maxTriggers) : base(
            skillIcon: SkillIconProvider.GetSkillIcon(SkillIcon.Frostbite, GameDriver.CellSizeVector),
            trapSprite: AnimatedSpriteProvider.GetAnimatedSprite(AnimationType.Ice, GameDriver.CellSizeVector, 6),
            title: "Cryomancy - Frostbite",
            damage: damage,
            maxTriggers: maxTriggers,
            range: new[] {0, 1},
            description:
            $"Place a trap that will deal [{damage}] damage and slow units that start the round on it." +
            Environment.NewLine +
            $"Max activations: [{maxTriggers}]",
            freeAction: true
        )
        {
        }

        public override void GenerateActionGrid(Vector2 origin, Layer mapLayer = Layer.Dynamic)
        {
            UnitTargetingContext unitTargetingContext = new UnitTargetingContext(TileSprite);
            unitTargetingContext.GenerateTargetingGrid(origin, Range, mapLayer);
            RemoveActionTilesOnUnplaceableSpaces(mapLayer);
        }

        private static void RemoveActionTilesOnUnplaceableSpaces(Layer mapLayer)
        {
            List<MapElement> targetTiles = MapContainer.GetMapElementsFromLayer(mapLayer);

            List<MapElement> tilesToRemove = targetTiles
                .Where(element => TargetHasEntityOrWall(MapContainer.GetMapSliceAtCoordinates(element.MapCoordinates)))
                .ToList();

            foreach (MapElement tile in tilesToRemove)
            {
                MapContainer.GameGrid[(int) mapLayer][(int) tile.MapCoordinates.X, (int) tile.MapCoordinates.Y] = null;
            }
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            if (TargetIsInRange(targetSlice))
            {
                if (!TargetHasEntityOrWall(targetSlice))
                {
                    TrapEntity trapToPlace = new TrapEntity("Ice Spikes", TrapSprite.Clone(),
                        targetSlice.MapCoordinates, Damage, MaxTriggers, true, true, false, true);

                    MapContainer.ClearDynamicAndPreviewGrids();
                    Queue<IEvent> eventQueue = new Queue<IEvent>();
                    eventQueue.Enqueue(
                        new PlayAnimationAtCoordinatesEvent(AnimatedIconType.Interact, targetSlice.MapCoordinates)
                    );
                    eventQueue.Enqueue(new PlaceEntityOnMapEvent((TrapEntity) trapToPlace.Duplicate(), Layer.Entities,
                        AssetManager.DropItemSFX));
                    eventQueue.Enqueue(new WaitFramesEvent(30));
                    eventQueue.Enqueue(new AdditionalActionEvent());
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
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Not in range!", 50);
                AssetManager.WarningSFX.Play();
            }
        }
    }
}