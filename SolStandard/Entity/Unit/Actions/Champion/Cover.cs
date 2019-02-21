﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers;
using SolStandard.Containers.Contexts;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions.Champion
{
    public class Cover : UnitAction
    {
        private readonly int armorPoints;

        public Cover(int armorPoints) : base(
            icon: SkillIconProvider.GetSkillIcon(SkillIcon.BasicAttack, new Vector2(32)),
            name: "Cover",
            description: "Regenerate [" + armorPoints + "] " + UnitStatistics.Abbreviation[Stats.Armor] +
                         " for an ally in range.",
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Action),
            range: new[] {1}
        )
        {
            this.armorPoints = armorPoints;
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            GameUnit targetUnit = UnitSelector.SelectUnit(targetSlice.UnitEntity);

            if (TargetIsAnAllyInRange(targetSlice, targetUnit))
            {
                MapContainer.ClearDynamicAndPreviewGrids();

                Queue<IEvent> eventQueue = new Queue<IEvent>();
                eventQueue.Enqueue(new RegenerateArmorEvent(targetUnit, armorPoints));
                eventQueue.Enqueue(new EndTurnEvent());
                GlobalEventQueue.QueueEvents(eventQueue);

                string toastMessage = "Cover!" + Environment.NewLine +
                                      "Recovered [" + armorPoints + "] " + UnitStatistics.Abbreviation[Stats.Armor] +
                                      "!";
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor(toastMessage, 50);
            }
            else
            {
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Not an ally in range!", 50);
                AssetManager.WarningSFX.Play();
            }
        }
    }
}