using System;
using SolStandard.Containers.Components.Global;

namespace SolStandard.Utility.Events.Network
{
    [Serializable]
    public class CloseAdHocDraftMenuEvent : NetworkEvent
    {
        public override void Continue()
        {
            GlobalContext.WorldContext.ClearDraftMenu();
            Complete = true;
        }
    }
}