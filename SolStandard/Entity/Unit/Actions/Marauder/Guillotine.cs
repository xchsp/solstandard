using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.Unit.Statuses;
using SolStandard.Map;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions.Marauder
{
    public class Guillotine : UnitAction
    {
        public Guillotine() : base(
            icon: SkillIconProvider.GetSkillIcon(SkillIcon.Guillotine, new Vector2(GameDriver.CellSize)),
            name: "Guillotine",
            description: "Perform a basic attack. If the target unit is defeated, regenerate <1/4> of missing " +
                         UnitStatistics.Abbreviation[Stats.Hp] + " (rounded down)." + Environment.NewLine +
                         "If currently enraged, regenerate <1/3> of missing " + UnitStatistics.Abbreviation[Stats.Hp] +
                         " instead, and lose enraged status.",
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Attack),
            range: null,
            freeAction: false
        )
        {
        }

        public override void GenerateActionGrid(Vector2 origin, Layer mapLayer = Layer.Dynamic)
        {
            Range = GameContext.ActiveUnit.Stats.CurrentAtkRange;
            base.GenerateActionGrid(origin, mapLayer);
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            GameUnit targetUnit = UnitSelector.SelectUnit(targetSlice.UnitEntity);

            if (TargetIsAnEnemyInRange(targetSlice, targetUnit))
            {
                bool attackerIsEnraged = AttackerHasEnragedStatus();

                Queue<IEvent> eventQueue = new Queue<IEvent>();
                eventQueue.Enqueue(
                    new CastStatusEffectEvent(GameContext.ActiveUnit, new GuillotineStatus(Icon, 0, attackerIsEnraged))
                );
                eventQueue.Enqueue(new StartCombatEvent(targetUnit));
                GlobalEventQueue.QueueEvents(eventQueue);
            }
            else
            {
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Can't attack here!", 50);
                AssetManager.WarningSFX.Play();
            }
        }

        private static bool AttackerHasEnragedStatus()
        {
            return GameContext.ActiveUnit.StatusEffects.Exists(status => status is EnragedStatus);
        }
    }
}