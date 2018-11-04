﻿using System.Collections.Generic;
using SolStandard.Containers;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.General;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions.Terrain
{
    public class ToggleLockAction : UnitAction
    {
        private readonly Key key;

        public ToggleLockAction(Key key) : base(
            icon: key.Icon,
            name: "Use: " + key.Name,
            description: "Locks or unlocks the target if you have the appropriate key.",
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Action),
            range: new[] {1}
        )
        {
            this.key = key;
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            ILockable targetUnlockable = targetSlice.TerrainEntity as ILockable;

            if (KeyWorksOnLock(targetSlice, targetUnlockable))
            {
                MapContainer.ClearDynamicAndPreviewGrids();

                Queue<IEvent> eventQueue = new Queue<IEvent>();
                eventQueue.Enqueue(new ToggleLockEvent(targetUnlockable));
                eventQueue.Enqueue(new WaitFramesEvent(5));
                eventQueue.Enqueue(new ToggleOpenEvent(targetSlice.TerrainEntity as IOpenable));

                Chest targetChest = targetUnlockable as Chest;
                if (targetChest != null)
                {
                    eventQueue.Enqueue(new IncreaseUnitGoldEvent(targetChest.Gold));
                    eventQueue.Enqueue(new WaitFramesEvent(5));
                    //Keys are one-time use; delete the key after using it.
                    eventQueue.Enqueue(new DeleteItemEvent(key));
                }

                eventQueue.Enqueue(new WaitFramesEvent(10));
                eventQueue.Enqueue(new EndTurnEvent());
                GlobalEventQueue.QueueEvents(eventQueue);
            }
            else
            {
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Key doesn't work here!", 50);
                AssetManager.WarningSFX.Play();
            }
        }

        private bool KeyWorksOnLock(MapSlice targetSlice, ILockable targetUnlockable)
        {
            return targetUnlockable != null
                   && targetSlice.DynamicEntity != null
                   && targetSlice.UnitEntity == null
                   && key.UsedWith == targetSlice.TerrainEntity.Name
                   && ChestIsNotOpen(targetUnlockable);
        }

        private bool ChestIsNotOpen(ILockable targetUnlockable)
        {
            Chest targetChest = targetUnlockable as Chest;
            return targetChest != null && !targetChest.IsOpen;
        }
    }
}