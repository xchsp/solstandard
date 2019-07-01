using Microsoft.Xna.Framework;
using SolStandard.Containers.View;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.HUD.Menu.Options.DialMenu
{
    public class PasteIPAddressOption : MenuOption
    {
        private readonly NetworkMenuView menu;

        public PasteIPAddressOption(Color menuColor, NetworkMenuView menu) : base(
            new RenderText(AssetManager.WindowFont, "Paste IP"),
            menuColor,
            HorizontalAlignment.Centered
        )
        {
            this.menu = menu;
        }

        public override void Execute()
        {
            menu.PasteIPAddressFromClipboard();
        }

        public override IRenderable Clone()
        {
            return new CopyIPAddressOption(DefaultColor, menu);
        }
    }
}