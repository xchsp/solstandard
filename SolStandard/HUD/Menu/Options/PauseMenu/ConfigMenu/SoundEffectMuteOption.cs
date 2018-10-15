﻿using Microsoft.Xna.Framework;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Monogame;

namespace SolStandard.HUD.Menu.Options.PauseMenu.ConfigMenu
{
    public class SoundEffectMuteOption : MenuOption
    {
        private const string OnLabel = "SFX: On";
        private const string OffLabel = "SFX: Off";

        public SoundEffectMuteOption(Color color) : base(
            new RenderText(AssetManager.MainMenuFont, (SoundEffectWrapper.Muted) ? OffLabel : OnLabel),
            color
        )
        {
        }

        public override void Execute()
        {
            SoundEffectWrapper.ToggleMute();

            UpdateLabel(new RenderText(AssetManager.MainMenuFont, (SoundEffectWrapper.Muted) ? OffLabel : OnLabel));
        }
    }
}