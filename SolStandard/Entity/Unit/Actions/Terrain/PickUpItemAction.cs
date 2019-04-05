﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers;
using SolStandard.Containers.Contexts;
using SolStandard.Map;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions.Terrain
{
    public class PickUpItemAction : UnitAction
    {
        private readonly IItem item;
        private readonly Vector2 itemCoordinates;

        public PickUpItemAction(IItem item, Vector2 itemCoordinates) : base(
            icon: item.Icon,
            name: "Pick Up",
            description: "Add the item to the active unit's inventory.",
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Action),
            range: null,
            freeAction: true
        )
        {
            this.item = item;
            this.itemCoordinates = itemCoordinates;
        }

        public override void GenerateActionGrid(Vector2 origin, Layer mapLayer = Layer.Dynamic)
        {
            MapContainer.GameGrid[(int) mapLayer][(int) itemCoordinates.X, (int) itemCoordinates.Y] =
                new MapDistanceTile(TileSprite, itemCoordinates);
            GameContext.GameMapContext.MapContainer.MapCursor.SnapCursorToCoordinates(itemCoordinates);
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            if (SelectingItemAtUnitLocation(targetSlice))
            {
                MapContainer.ClearDynamicAndPreviewGrids();

                Queue<IEvent> eventQueue = new Queue<IEvent>();
                eventQueue.Enqueue(new PickUpItemEvent(item, itemCoordinates));
                eventQueue.Enqueue(new WaitFramesEvent(30));
                eventQueue.Enqueue(new AdditionalActionEvent());
                GlobalEventQueue.QueueEvents(eventQueue);
            }
            else
            {
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Cannot pickup item here!", 50);
                AssetManager.WarningSFX.Play();
            }
        }

        private bool SelectingItemAtUnitLocation(MapSlice targetSlice)
        {
            return itemCoordinates == targetSlice.MapCoordinates &&
                   targetSlice.DynamicEntity != null;
        }
    }
}