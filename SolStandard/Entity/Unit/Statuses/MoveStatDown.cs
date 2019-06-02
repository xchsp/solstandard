using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.Unit.Statuses
{
    public class MoveStatDown : StatusEffect
    {
        private readonly int pointsToReduce;
        private const int SlowFrameDelay = 16;

        public MoveStatDown(int turnDuration, int pointsToReduce, string name = null) : base(
            statusIcon: UnitStatistics.GetSpriteAtlas(Stats.Mv, new Vector2(GameDriver.CellSize)),
            name: name ?? UnitStatistics.Abbreviation[Stats.Mv] + " Down! <-" + pointsToReduce + ">",
            description: "Decreased movement distance.",
            turnDuration: turnDuration,
            hasNotification: false,
            canCleanse: true
        )
        {
            this.pointsToReduce = pointsToReduce;
        }

        public override void ApplyEffect(GameUnit target)
        {
            AssetManager.SkillBuffSFX.Play();
            target.Stats.MvModifier -= pointsToReduce;
            target.UnitEntity.UnitSpriteSheet.SetFrameDelay(SlowFrameDelay);
            GameContext.GameMapContext.MapContainer.AddNewToastAtUnit(target.UnitEntity, Name, 50);
        }

        protected override void ExecuteEffect(GameUnit target)
        {
            //Do nothing
        }

        public override void RemoveEffect(GameUnit target)
        {
            target.Stats.MvModifier += pointsToReduce;
            target.UnitEntity.UnitSpriteSheet.ResetFrameDelay();
        }
    }
}