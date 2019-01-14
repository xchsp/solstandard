using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.Containers.View
{
    public class NetworkMenuView : IUserInterface
    {
        private readonly SpriteAtlas title;
        private readonly AnimatedSpriteSheet logo;
        private readonly SpriteAtlas background;
        private bool visible;
        private readonly Window networkStatusMenu;

        public NetworkMenuView(SpriteAtlas title, AnimatedSpriteSheet logo, SpriteAtlas background)
        {
            this.title = title;
            this.logo = logo;
            this.background = background;
            visible = true;
            networkStatusMenu =
                new Window(new WindowContentGrid(
                        new IRenderable[,]
                        {
                            { new RenderText(AssetManager.MainMenuFont, "___.___.___.___") },
                            { new RenderText(AssetManager.MainMenuFont, "Waiting for connection...") }
                        },
                        2,
                        HorizontalAlignment.Centered
                    ),
                    MainMenuView.MenuColor, HorizontalAlignment.Centered
                );
        }

        public void EnterNumber()
        {
            //TODO Read keyboard input and update IP Address based on input
            //TODO Read any digit or period "." input
            //TODO Read backspace to undo character input
        }

        public void ToggleVisible()
        {
            visible = !visible;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                Vector2 centerScreen = GameDriver.ScreenSize / 2;

                Vector2 backgroundCenter = new Vector2(background.Width, background.Height) / 2;
                background.Draw(spriteBatch, centerScreen - backgroundCenter);

                const int titleVertCoordinate = 30;
                Vector2 titleCenter = new Vector2(title.Width, title.Height) / 2;
                Vector2 titlePosition = new Vector2(centerScreen.X - titleCenter.X, titleVertCoordinate);
                logo.Draw(spriteBatch, titlePosition);
                title.Draw(spriteBatch, titlePosition + new Vector2(100));

                const int titlePadding = 200;
                Vector2 mainMenuCenter = new Vector2(networkStatusMenu.Width, networkStatusMenu.Height) / 2;
                Vector2 mainMenuPosition =
                    new Vector2(centerScreen.X - mainMenuCenter.X, titlePosition.Y + title.Height + titlePadding);
                networkStatusMenu.Draw(spriteBatch, mainMenuPosition);
            }
        }
    }
}