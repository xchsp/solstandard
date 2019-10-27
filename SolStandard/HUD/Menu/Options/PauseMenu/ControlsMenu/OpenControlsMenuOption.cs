using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.HUD.Menu.Options.MainMenu;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.HUD.Menu.Options.PauseMenu.ControlsMenu
{
    public class OpenControlsMenuOption : MenuOption
    {
        public OpenControlsMenuOption(Color color) :
            base(new RenderText(AssetManager.MainMenuFont, "Control Config"), color, HorizontalAlignment.Centered)
        {
        }

        public override void Execute()
        {
            GameContext.ControlConfigContext.OpenMenu();
        }

        public override IRenderable Clone()
        {
            return new OpenCodexOption(DefaultColor);
        }
    }
}