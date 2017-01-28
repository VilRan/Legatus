using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Legatus.Input
{
    public abstract class InputHandler
    {
        private static readonly MouseButton[] AllButtons = { MouseButton.Left, MouseButton.Middle, MouseButton.Right, MouseButton.XButton1, MouseButton.XButton2 };

        private const int KeyRepeatDelayFull = 500;
        private const int KeyRepeatDelayShort = 25;
        private const int DoubleClickDelayMax = 300;

        private KeyboardState PreviousKeyboard;
        private Keys[] PreviousPressedKeys;
        private int KeyRepeatDelayRemaining;
        private MouseState PreviousMouse;
        private ButtonState[] PreviousButtons;
        private Point[] DragStartPosition;
        private int[] DoubleClickDelayRemaining;

        public InputHandler()
        {
            PreviousPressedKeys = new Keys[0];
            KeyRepeatDelayRemaining = KeyRepeatDelayFull;
            PreviousMouse = Mouse.GetState();
            ButtonState[] buttons = { PreviousMouse.LeftButton, PreviousMouse.MiddleButton, PreviousMouse.RightButton, PreviousMouse.XButton1, PreviousMouse.XButton2 };
            PreviousButtons = buttons;
            DragStartPosition = new Point[buttons.Length];
            DoubleClickDelayRemaining = new int[buttons.Length];
        }
        
        public void Update(GameTime gameTime)
        {
            UpdateKeyboard(gameTime);
            UpdateMouse(gameTime);
        }

        private void UpdateKeyboard(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            Keys[] pressedKeys = keyboard.GetPressedKeys();
            
            if (pressedKeys.Length > 0)
            {
                if (pressedKeys.Contains(Keys.LeftShift) || pressedKeys.Contains(Keys.RightShift))
                {
                    if (pressedKeys.Length > 1)
                    {
                        KeyRepeatDelayRemaining -= gameTime.ElapsedGameTime.Milliseconds;
                        if (KeyRepeatDelayRemaining < 0)
                            KeyRepeatDelayRemaining = 0;
                    }
                }
                else
                {
                    KeyRepeatDelayRemaining -= gameTime.ElapsedGameTime.Milliseconds;
                    if (KeyRepeatDelayRemaining < 0)
                        KeyRepeatDelayRemaining = 0;
                }
            }
            else
            {
                KeyRepeatDelayRemaining = KeyRepeatDelayFull;
            }

            foreach (Keys key in pressedKeys)
            {
                if (!PreviousPressedKeys.Contains(key))
                {
                    OnKeyPressed(key, pressedKeys, gameTime);
                }
                else if (KeyRepeatDelayRemaining == 0)
                {
                    OnKeyRepeated(key, pressedKeys, gameTime);
                }

                OnKeyHeld(key, pressedKeys, gameTime);
            }

            foreach (Keys key in PreviousPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                {
                    OnKeyReleased(key, pressedKeys, gameTime);
                }
            }

            if (KeyRepeatDelayRemaining == 0 && pressedKeys.Length > 0)
            {
                KeyRepeatDelayRemaining = KeyRepeatDelayShort;
            }

            PreviousPressedKeys = pressedKeys;
            PreviousKeyboard = keyboard;
        }

        protected abstract void OnKeyPressed(Keys pressedKey, Keys[] allHeldKeys, GameTime gameTime);
        protected abstract void OnKeyRepeated(Keys repeatedKey, Keys[] allHeldKeys, GameTime gameTime);
        protected abstract void OnKeyHeld(Keys heldKey, Keys[] allHeldKeys, GameTime gameTime);
        protected abstract void OnKeyReleased(Keys releasedKey, Keys[] allHeldKeys, GameTime gameTime);

        private void UpdateMouse(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            OnMouseOver(mouse.Position, PreviousPressedKeys);
            if (mouse.ScrollWheelValue - PreviousMouse.ScrollWheelValue != 0)
                OnMouseScroll(mouse.Position, mouse.ScrollWheelValue - PreviousMouse.ScrollWheelValue, PreviousPressedKeys);

            ButtonState[] buttons = { mouse.LeftButton, mouse.MiddleButton, mouse.RightButton, mouse.XButton1, mouse.XButton2 };

            for (int i = 0; i < buttons.Length; i++)
            {
                ButtonState button = buttons[i];
                ButtonState previous = PreviousButtons[i];

                if (button == ButtonState.Pressed)
                {
                    if (previous == ButtonState.Released)
                    {
                        OnMousePressed(AllButtons[i], mouse.Position, PreviousPressedKeys);
                        DragStartPosition[i] = mouse.Position;

                        if (DoubleClickDelayRemaining[i] > 0)
                        {
                            OnMouseDoubleClick(AllButtons[i], mouse.Position, PreviousPressedKeys);
                            DoubleClickDelayRemaining[i] = 0;
                        }
                        else
                        {
                            DoubleClickDelayRemaining[i] = DoubleClickDelayMax;
                        }
                    }
                    else
                    {
                        OnMouseDrag(AllButtons[i], mouse.Position, PreviousMouse.Position, DragStartPosition[i], PreviousPressedKeys);
                    }
                }
                else if (previous == ButtonState.Pressed)
                {
                    OnMouseReleased(AllButtons[i], mouse.Position, PreviousPressedKeys);
                }

                DoubleClickDelayRemaining[i] -= gameTime.ElapsedGameTime.Milliseconds;
                if (DoubleClickDelayRemaining[i] < 0)
                    DoubleClickDelayRemaining[i] = 0;
            }

            PreviousMouse = mouse;
            PreviousButtons = buttons;
        }

        protected abstract void OnMouseOver(Point mousePosition, Keys[] allHeldKeys);
        protected abstract void OnMouseScroll(Point mousePosition, int delta, Keys[] allHeldKeys);
        protected abstract void OnMousePressed(MouseButton button, Point mousePosition, Keys[] allHeldKeys);
        protected abstract void OnMouseReleased(MouseButton button, Point mousePosition, Keys[] allHeldKeys);
        protected abstract void OnMouseDoubleClick(MouseButton button, Point mousePosition, Keys[] allHeldKeys);
        protected abstract void OnMouseDrag(MouseButton button, Point mousePosition, Point previousPosition, Point startPosition, Keys[] allHeldKeys);
    }
}
