﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SolStandard.Utility.Buttons.Gamepad
{
    public class GamepadUp: GamePadControl
    {
        public GamepadUp(PlayerIndex playerIndex) : base(playerIndex)
        {
        }

        public override bool Pressed =>
            GamePad.GetState(PlayerIndex).DPad.Up == ButtonState.Pressed ||
            GamePad.GetState(PlayerIndex).ThumbSticks.Left.Y > ControlMapper.StickDeadzone;
    }
}