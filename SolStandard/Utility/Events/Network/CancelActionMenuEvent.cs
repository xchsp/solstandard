using System;
using SolStandard.Containers.Components.Global;

namespace SolStandard.Utility.Events.Network
{
    [Serializable]
    public class CancelActionMenuEvent : NetworkEvent
    {
        public override void Continue()
        {
            GlobalContext.WorldContext.CancelActionMenu();
            Complete = true;
        }
    }
}