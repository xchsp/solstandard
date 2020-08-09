using SolStandard.Containers.Components.Global;
using SolStandard.Containers.Components.World;

namespace SolStandard.Utility.Events
{
    public class AdditionalActionEvent : IEvent
    {
        public bool Complete { get; private set; }

        public void Continue()
        {
            if (GlobalContext.ActiveUnit.IsAlive)
            {
                StartExtraAction("Extra action!");
            }
            else
            {
                GameMapContext.FinishTurn(true);
            }

            Complete = true;
        }

        public static void StartExtraAction(string message)
        {
            GlobalContext.GameMapContext.ResetToActionMenu();
            GlobalContext.GameMapContext.MapContainer.AddNewToastAtMapCursor(message, 50);
            GameMapContext.UpdateWindowsEachTurn();
        }
    }
}