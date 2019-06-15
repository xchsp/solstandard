using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.Unit.Statuses
{
    public class BlkStatUp : StatusEffect
    {
        private readonly int pointsToIncrease;

        public BlkStatUp(int turnDuration, int pointsToIncrease, string name = null) : base(
            statusIcon: UnitStatistics.GetSpriteAtlas(Stats.Block, new Vector2(GameDriver.CellSize)),
            name: name ?? UnitStatistics.Abbreviation[Stats.Block] + " Up! <+" + pointsToIncrease + ">",
            description: "Increased damage mitigation.",
            turnDuration: turnDuration,
            hasNotification: false,
            canCleanse: false
        )
        {
            this.pointsToIncrease = pointsToIncrease;
        }

        public override void ApplyEffect(GameUnit target)
        {
            AssetManager.SkillBuffSFX.Play();
            target.Stats.BlkModifier += pointsToIncrease;
            GameContext.GameMapContext.MapContainer.AddNewToastAtUnit(target.UnitEntity, Name, 50);
        }

        protected override void ExecuteEffect(GameUnit target)
        {
            //Do nothing.
        }

        public override void RemoveEffect(GameUnit target)
        {
            target.Stats.BlkModifier -= pointsToIncrease;
        }
    }
}