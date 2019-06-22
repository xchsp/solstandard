using SolStandard.Utility;

namespace SolStandard.HUD.Window
{
    public interface IWindow : IRenderable
    {
        void FadeAtRate(int opacity, int changeRatePerFrame);
        void ResetOpacity();
    }
}