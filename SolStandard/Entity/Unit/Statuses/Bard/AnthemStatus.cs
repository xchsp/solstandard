using System.Linq;
using SolStandard.Containers.Contexts;
using SolStandard.Containers.Contexts.Combat;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.Unit.Statuses.Bard
{
    public class AnthemStatus : SongStatus
    {
        public AnthemStatus(int auraBonus, int selfBonus, int[] auraRange) : base(
            statusIcon: SkillIconProvider.GetSkillIcon(SkillIcon.AtkBuff, GameDriver.CellSizeVector),
            name:
            $"Anthem <+{UnitStatistics.Abbreviation[Stats.Atk]} {auraBonus} Aura/{selfBonus} Solo>",
            description:
            $"Increases {UnitStatistics.Abbreviation[Stats.Atk]} by [{auraBonus} Aura/{selfBonus} Solo] for units within the aura.",
            turnDuration: 99,
            new BonusStatistics(auraBonus, 0, 0, 0),
            new BonusStatistics(selfBonus, 0, 0, 0),
            auraRange,
            false
        )
        {
            SongSprite = AnimatedSpriteProvider.GetAnimatedSprite(
                AnimationType.SongAttack,
                GameDriver.CellSizeVector,
                SongAnimationFrameDelay,
                GetSongColor(GameContext.ActiveUnit.Team)
            );
        }

        public override void ApplyEffect(GameUnit target)
        {
            GameContext.GameMapContext.MapContainer.AddNewToastAtUnit(target.UnitEntity, Name, 50);
            base.ApplyEffect(target);
        }

        protected override void ExecuteEffect(GameUnit target)
        {
            //Do nothing.
        }

        public override void RemoveEffect(GameUnit target)
        {
            //Do nothing.
        }

        public override bool UnitIsAffectedBySong(GameUnit unitAffected)
        {
            GameUnit singer = GameContext.Units.FirstOrDefault(unit => unit.StatusEffects.Contains(this));
            return singer != null &&
                   (unitAffected.Team == singer.Team && UnitIsAffectedBySong(unitAffected, this));
        }
    }
}