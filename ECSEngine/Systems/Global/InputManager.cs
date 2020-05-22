using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.Componets;
using System.Diagnostics;

namespace Engine.Systems
{
    public static class InputManager
    {
        public static KeyboardState OldKeyboardState { get; set; }
        public static KeyboardState NewKeyboardState { get; set; }
        public static MouseState OldMouseState { get; set; }
        public static MouseState NewMouseState { get; set; }
        public static GamePadState OldGamePadState { get; set; }
        public static GamePadState NewGamePadState { get; set; }
        public static bool NewAxis { get; }
        public static bool OldAxis { get; }

        public static Dictionary<string, InputAxis> InputAxises { get; set; } = new Dictionary<string, InputAxis>();

        #region Axis Events

        public static float GetAxis(string axis)
        {
            if (!InputAxises.ContainsKey(axis))
            {
                Debug.WriteLine($"No input axis found for '{axis}'");
                return 0;
            }

            if (KeyPressed(InputAxises[axis].PositiveKey))
            {
                return 1;
            }
            else if (KeyPressed(InputAxises[axis].NegativeKey))
            {
                return -1;
            }

            if (GamePadButtonPressed(InputAxises[axis].PositiveButton))
            {
                return 1;
            }
            else if (GamePadButtonPressed(InputAxises[axis].NegativeButton))
            {
                return -1;
            }

            if (InputAxises[axis].LeftThumbStickX)
            {
                return GetLeftThumbStick().X;
            }

            if (InputAxises[axis].LeftThumbStickY)
            {
                return -GetLeftThumbStick().Y;
            }

            if (InputAxises[axis].RightThumbStickX)
            {
                return GetRightThumbStick().X;
            }

            if (InputAxises[axis].RightThumbStickY)
            {
                return -GetRightThumbStick().Y;
            }

            if (InputAxises[axis].MouseAxisX)
            {
                return GetMousePosition().X;
            }

            if (InputAxises[axis].MouseAxisY)
            {
                return GetMousePosition().Y;
            }

            return 0;
        }

        public static bool GetAxisPressed(string axis)
        {
            return GetAxis(axis) != 0;
        }

        public static bool GetAxisDown(string axis)
        {
            if (!InputAxises.ContainsKey(axis))
            {
                Debug.WriteLine($"No input axis found for '{axis}'");
                return false;
            }

            return KeyDown(InputAxises[axis].PositiveKey) ||
                   KeyDown(InputAxises[axis].NegativeKey) ||
                   GamePadButtonDown(InputAxises[axis].PositiveButton) ||
                   GamePadButtonDown(InputAxises[axis].NegativeButton);
        }
        #endregion

        #region Key Events

        public static bool KeyDown(Keys key)
        {
            return NewKeyboardState.IsKeyDown(key)
                && !OldKeyboardState.IsKeyDown(key);
        }

        public static bool KeyUp(Keys key)
        {
            return NewKeyboardState.IsKeyUp(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return NewKeyboardState.IsKeyDown(key);
        }

        #endregion

        #region GamePad Events

        public static bool GamePadButtonPressed(Buttons button)
        {
            return NewGamePadState.IsButtonDown(button);
        }

        public static bool GamePadButtonUp(Buttons button)
        {
            return NewGamePadState.IsButtonUp(button);
        }

        public static bool GamePadButtonDown(Buttons button)
        {
            return NewGamePadState.IsButtonDown(button) &&
                   OldGamePadState.IsButtonUp(button);
        }

        public static Vector2 GetLeftThumbStick()
        {
            return NewGamePadState.ThumbSticks.Left;
        }

        public static Vector2 GetRightThumbStick()
        {
            return NewGamePadState.ThumbSticks.Right;
        }

        #endregion

        #region Mouse Events

        public static bool LeftMousePressed()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed; 
        }

        public static bool LeftMouseUp()
        {
            return Mouse.GetState().LeftButton == ButtonState.Released;
        }

        public static bool LeftMouseDown()
        {
            return OldMouseState.LeftButton == ButtonState.Released && 
                   NewMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool RightMouseDown()
        {
            return OldMouseState.RightButton == ButtonState.Released &&
                   NewMouseState.RightButton == ButtonState.Pressed;
        }

        public static Vector GetMousePosition()
        {
            return new Vector(Mouse.GetState().X, Mouse.GetState().Y);
        }

        #endregion
    }

    public class InputAxis
    {
        public Keys PositiveKey { get; set; }
        public Keys NegativeKey { get; set; }
        public Buttons PositiveButton { get; set; }
        public Buttons NegativeButton { get; set; }
        public bool MouseAxisX {get; set;}
        public bool MouseAxisY { get; set; }
        public bool LeftThumbStickX { get; set; }
        public bool RightThumbStickX { get; set; }
        public bool LeftThumbStickY { get; set; } 
        public bool RightThumbStickY { get; set; }

        public InputAxis(Keys PositiveKey = Keys.None,
                         Keys NegativeKey = Keys.None,
                         Buttons PositiveButton = Buttons.Back,
                         Buttons NegativeButton = Buttons.Back,
                         bool MouseAxisX = false,
                         bool MouseAxisY = false,
                         bool LeftThumbStickX = false,
                         bool RightThumbStickX = false,
                         bool LeftThumbStickY = false,
                         bool RightThumbStickY = false)
        {
            this.PositiveKey = PositiveKey;
            this.NegativeKey = NegativeKey;
            this.PositiveButton = PositiveButton;
            this.NegativeButton = NegativeButton;
            this.MouseAxisX = MouseAxisX;
            this.MouseAxisY = MouseAxisY;
            this.LeftThumbStickX = LeftThumbStickX;
            this.RightThumbStickX = RightThumbStickX;
            this.LeftThumbStickY = LeftThumbStickY;
            this.RightThumbStickY = RightThumbStickY;
        }
    }
}
