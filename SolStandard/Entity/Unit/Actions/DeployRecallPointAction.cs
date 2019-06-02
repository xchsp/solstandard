using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.General;
using SolStandard.Entity.General.Item;
using SolStandard.Map;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions
{
    public class DeployRecallPointAction : UnitAction
    {
        private readonly RecallCharm recallSource;


        public DeployRecallPointAction(RecallCharm recallSource, int[] deployRange) : base(
            icon: recallSource.Icon.Clone(),
            name: "Set Recall Point",
            description: "Single Use. " + Environment.NewLine +
                         "Deploy a recall point that can be teleported to by reusing " + recallSource.Name + "." +
                         Environment.NewLine +
                         "Expires after recall source is used again." + Environment.NewLine +
                         "Recall ID: " + recallSource.RecallId,
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Action),
            range: deployRange,
            freeAction: false
        )
        {
            this.recallSource = recallSource;
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            if (CanPlaceRecallPointAtTarget(targetSlice))
            {
                recallSource.DeployRecall();
                RecallPoint recallPoint = GenerateRecallPoint(recallSource.RecallId, targetSlice);

                Queue<IEvent> eventQueue = new Queue<IEvent>();
                eventQueue.Enqueue(new PlaceEntityOnMapEvent(recallPoint, Layer.Entities, AssetManager.CombatBlockSFX));
                eventQueue.Enqueue(new WaitFramesEvent(10));
                eventQueue.Enqueue(new EndTurnEvent());
                GlobalEventQueue.QueueEvents(eventQueue);
            }
            else
            {
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor(
                    "Invalid target! Place on movable tile without terrain entity in range.",
                    50
                );
                AssetManager.WarningSFX.Play();
            }
        }

        private static bool CanPlaceRecallPointAtTarget(MapSlice targetSlice)
        {
            return targetSlice.TerrainEntity == null && targetSlice.DynamicEntity != null &&
                   UnitMovingContext.CanEndMoveAtCoordinates(targetSlice.MapCoordinates);
        }

        private static RecallPoint GenerateRecallPoint(string sourceId, MapSlice targetSlice)
        {
            return new RecallPoint(
                sourceId,
                SkillIconProvider.GetSkillIcon(SkillIcon.Blink, new Vector2(GameDriver.CellSize)),
                targetSlice.MapCoordinates
            );
        }
    }
}