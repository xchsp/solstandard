using Microsoft.Xna.Framework;
using SolStandard.Containers.View;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Monogame;

namespace SolStandard.HUD.Menu.Options.DialMenu
{
    public class CopyIPAddressOption : MenuOption
    {
        private readonly NetworkMenuView menu;

        public CopyIPAddressOption(Color menuColor, NetworkMenuView menu) : base(
            new RenderText(AssetManager.MainMenuFont, "Copy IP"),
            menuColor,
            HorizontalAlignment.Centered
        )
        {
            this.menu = menu;
        }

        public override void Execute()
        {
            menu.CopyHostIPAddress();
        }

        public override IRenderable Clone()
        {
            return new CopyIPAddressOption(DefaultColor, menu);
        }
    }
}