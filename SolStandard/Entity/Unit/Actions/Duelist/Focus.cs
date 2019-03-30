using System.Linq;
using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.Unit.Statuses;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.Unit.Actions.Duelist
{
    public class Focus : UnitAction
    {
        private readonly int maxActions;

        public Focus(int maxActions) : base(
            icon: SkillIconProvider.GetSkillIcon(SkillIcon.Focus, new Vector2(GameDriver.CellSize)),
            name: "Focus",
            description: "End your action now and store it for later. Can store up to " + maxActions +
                         " actions at a time.",
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Action),
            range: new[] {0},
            freeAction: false
        )
        {
            this.maxActions = maxActions;
        }

        public override void ExecuteAction(MapSlice targetSlice)
        {
            GameUnit targetUnit = UnitSelector.SelectUnit(targetSlice.UnitEntity);

            if (TargetIsSelfInRange(targetSlice, targetUnit))
            {
                FocusStatus currentFocus =
                    targetUnit.StatusEffects.SingleOrDefault(status => status is FocusStatus) as FocusStatus;

                if (currentFocus != null)
                {
                    if (currentFocus.FocusPoints < maxActions)
                    {
                        AssetManager.SkillBuffSFX.Play();
                        AssetManager.MenuConfirmSFX.Play();
                        GlobalEventQueue.QueueSingleEvent(
                            new CastStatusEffectEvent(targetUnit, new FocusStatus(currentFocus.FocusPoints + 1))
                        );
                        GlobalEventQueue.QueueSingleEvent(new EndTurnEvent());
                    }
                    else
                    {
                        GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Max focus points reached!", 50);
                        AssetManager.WarningSFX.Play();
                    }
                }
                else
                {
                    GlobalEventQueue.QueueSingleEvent(new CastStatusEffectEvent(targetUnit, new FocusStatus(1)));
                    GlobalEventQueue.QueueSingleEvent(new EndTurnEvent());
                }
            }
            else
            {
                GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor("Must target self!", 50);
                AssetManager.WarningSFX.Play();
            }
        }
    }
}