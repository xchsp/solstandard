using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Exceptions;

namespace SolStandard.Utility.Buttons.Gamepad
{
    public class GamepadController : IController
    {
        private readonly Dictionary<Input, GameControl> inputs;

        public ControlType ControlType => ControlType.Gamepad;

        public GameControl Confirm { get; }
        public GameControl Cancel { get; }
        public GameControl ResetToUnit { get; }
        public GameControl CenterCamera { get; }

        public GameControl CursorUp { get; }
        public GameControl CursorDown { get; }
        public GameControl CursorLeft { get; }
        public GameControl CursorRight { get; }

        public GameControl CameraUp { get; }
        public GameControl CameraDown { get; }
        public GameControl CameraLeft { get; }
        public GameControl CameraRight { get; }

        public GameControl Menu { get; }
        public GameControl Status { get; }

        public GameControl SetWideZoom { get; }
        public GameControl SetCloseZoom { get; }
        public GameControl AdjustZoomOut { get; }
        public GameControl AdjustZoomIn { get; }

        public GamepadController(PlayerIndex playerIndex)
        {
            Confirm = new GamepadA(playerIndex);
            Cancel = new GamepadB(playerIndex);
            ResetToUnit = new GamepadX(playerIndex);
            CenterCamera = new GamepadY(playerIndex);

            CursorUp = new GamepadUp(playerIndex);
            CursorDown = new GamepadDown(playerIndex);
            CursorLeft = new GamepadLeft(playerIndex);
            CursorRight = new GamepadRight(playerIndex);

            CameraUp = new GamepadRsUp(playerIndex);
            CameraDown = new GamepadRsDown(playerIndex);
            CameraLeft = new GamepadRsLeft(playerIndex);
            CameraRight = new GamepadRsRight(playerIndex);

            Menu = new GamepadStart(playerIndex);
            Status = new GamepadSelect(playerIndex);

            SetWideZoom = new GamepadLeftBumper(playerIndex);
            SetCloseZoom = new GamepadRightBumper(playerIndex);
            AdjustZoomOut = new GamepadLeftTrigger(playerIndex);
            AdjustZoomIn = new GamepadRightTrigger(playerIndex);

            inputs = new Dictionary<Input, GameControl>
            {
                {Input.None, new VoidInput()},

                {Input.Confirm, Confirm},
                {Input.Cancel, Cancel},
                {Input.PreviewUnit, ResetToUnit},
                {Input.PreviewItem, CenterCamera},

                {Input.CursorUp, CursorUp},
                {Input.CursorDown, CursorDown},
                {Input.CursorLeft, CursorLeft},
                {Input.CursorRight, CursorRight},

                {Input.CameraUp, CameraUp},
                {Input.CameraDown, CameraDown},
                {Input.CameraLeft, CameraLeft},
                {Input.CameraRight, CameraRight},

                {Input.Menu, Menu},
                {Input.Status, Status},

                {Input.TabLeft, SetWideZoom},
                {Input.TabRight, SetCloseZoom},
                {Input.ZoomOut, AdjustZoomOut},
                {Input.ZoomIn, AdjustZoomIn}
            };
        }

        public GameControl GetInput(Input input)
        {
            return inputs[input];
        }

        public void RemapControl(Input inputToRemap, GameControl newInput)
        {
            if (InputAlreadySet(newInput)) throw new DuplicateInputException();

            inputs[inputToRemap] = newInput;
        }

        private bool InputAlreadySet(GameControl potentialControl)
        {
            return ((Input[]) Enum.GetValues(typeof(Input))).Any(input => inputs[input] == potentialControl);
        }

        private bool Equals(IController other)
        {
            return Equals(Confirm, other.Confirm) && Equals(Cancel, other.Cancel) &&
                   Equals(ResetToUnit, other.ResetToUnit) && Equals(CenterCamera, other.CenterCamera) &&
                   Equals(CursorUp, other.CursorUp) && Equals(CursorDown, other.CursorDown) &&
                   Equals(CursorLeft, other.CursorLeft) && Equals(CursorRight, other.CursorRight) &&
                   Equals(CameraUp, other.CameraUp) && Equals(CameraDown, other.CameraDown) &&
                   Equals(CameraLeft, other.CameraLeft) && Equals(CameraRight, other.CameraRight) &&
                   Equals(Menu, other.Menu) && Equals(Status, other.Status) && Equals(SetWideZoom, other.SetWideZoom) &&
                   Equals(SetCloseZoom, other.SetCloseZoom) && Equals(AdjustZoomOut, other.AdjustZoomOut) &&
                   Equals(AdjustZoomIn, other.AdjustZoomIn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((GamepadController) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Confirm != null ? Confirm.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Cancel != null ? Cancel.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ResetToUnit != null ? ResetToUnit.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CenterCamera != null ? CenterCamera.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CursorUp != null ? CursorUp.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CursorDown != null ? CursorDown.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CursorLeft != null ? CursorLeft.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CursorRight != null ? CursorRight.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CameraUp != null ? CameraUp.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CameraDown != null ? CameraDown.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CameraLeft != null ? CameraLeft.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CameraRight != null ? CameraRight.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Menu != null ? Menu.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Status != null ? Status.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SetWideZoom != null ? SetWideZoom.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SetCloseZoom != null ? SetCloseZoom.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AdjustZoomOut != null ? AdjustZoomOut.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AdjustZoomIn != null ? AdjustZoomIn.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}