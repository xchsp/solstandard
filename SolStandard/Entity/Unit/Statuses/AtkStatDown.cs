using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.Unit.Statuses
{
    public class AtkStatDown : StatusEffect
    {
        private readonly int pointsToReduce;

        public AtkStatDown(int turnDuration, int pointsToReduce, string name = null) : base(
            statusIcon: UnitStatistics.GetSpriteAtlas(Stats.Atk, new Vector2(GameDriver.CellSize)),
            name: name ?? UnitStatistics.Abbreviation[Stats.Atk] + " Down! <-" + pointsToReduce + ">",
            description: "Decreased attack power.",
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
            target.Stats.AtkModifier -= pointsToReduce;
            GameContext.GameMapContext.MapContainer.AddNewToastAtUnit(target.UnitEntity, Name, 50);
        }

        protected override void ExecuteEffect(GameUnit target)
        {
            //Do nothing.
        }

        public override void RemoveEffect(GameUnit target)
        {
            target.Stats.AtkModifier += pointsToReduce;
        }
    }
}