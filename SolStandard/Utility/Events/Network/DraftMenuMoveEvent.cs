using System;
using SolStandard.Containers.Contexts;
using SolStandard.Map.Elements;

namespace SolStandard.Utility.Events.Network
{
    [Serializable]
    public class DraftMenuMoveEvent : NetworkEvent
    {
        private readonly Direction direction;

        public DraftMenuMoveEvent(Direction direction)
        {
            this.direction = direction;
        }
        public override void Continue()
        {
            GameContext.DraftContext.MoveCursor(direction);
            Complete = true;
        }
    }
}