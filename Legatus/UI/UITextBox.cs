using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Legatus.UI
{
    public class UITextBox : UIText
    {
        public bool MultiLine;
        
        public UITextBox(int x, int y, int width, int height, SpriteFont font, Color color, string text = "", int maxLength = int.MaxValue, bool multiline = false)
            : base(x, y, width, height, font, color, text)
        {
            MaxLength = maxLength;
            MultiLine = multiline;
        }

        public override void OnTextInput(TextInputEventArgs input)
        {
            switch (input.Character)
            {
                case '\b':
                    if (String.Length > 0)
                        String = String.Remove(String.Length - 1);
                    break;
                case '\n':
                case '\r':
                    if (MultiLine)
                        String += '\n';
                    break;
                default:
                    if (Font.Characters.Contains(input.Character))
                        String += input.Character;
                    break;
            }
        }

        /*
        public override void OnKeyboardAction(KeyboardEventArgs keyboard)
        {
        if (keyboard.Action == KeyboardAction.Pressed || keyboard.Action == KeyboardAction.Repeated)
            OnKeyPressed(keyboard.Key, keyboard.HeldKeys);

        base.OnKeyboardAction(keyboard);
        }

        private void OnKeyPressed(Keys pressedKey, Keys[] allHeldKeys)
        {
        switch (pressedKey)
        {
            case Keys.Enter:
                if (MultiLine)
                    String += '\n';
                break;
            case Keys.Back:
                if (String.Length > 0)
                    String = String.Remove(String.Length - 1);
                break;
            case Keys.Space:
                String += ' ';
                break;
            case Keys.D0:
            case Keys.NumPad0:
                String += '0';
                break;
            case Keys.D1:
            case Keys.NumPad1:
                String += '1';
                break;
            case Keys.D2:
            case Keys.NumPad2:
                String += '2';
                break;
            case Keys.D3:
            case Keys.NumPad3:
                String += '3';
                break;
            case Keys.D4:
            case Keys.NumPad4:
                String += '4';
                break;
            case Keys.D5:
            case Keys.NumPad5:
                String += '5';
                break;
            case Keys.D6:
            case Keys.NumPad6:
                String += '6';
                break;
            case Keys.D7:
            case Keys.NumPad7:
                String += '7';
                break;
            case Keys.D8:
            case Keys.NumPad8:
                String += '8';
                break;
            case Keys.D9:
            case Keys.NumPad9:
                String += '9';
                break;
            case Keys.OemQuotes:
                if (allHeldKeys.Contains(Keys.LeftShift) || allHeldKeys.Contains(Keys.RightShift))
                    String += '"';
                else
                    String += '\'';
                break;
            case Keys.A:
            case Keys.B:
            case Keys.C:
            case Keys.D:
            case Keys.E:
            case Keys.F:
            case Keys.G:
            case Keys.H:
            case Keys.I:
            case Keys.J:
            case Keys.K:
            case Keys.L:
            case Keys.M:
            case Keys.N:
            case Keys.O:
            case Keys.P:
            case Keys.Q:
            case Keys.R:
            case Keys.S:
            case Keys.T:
            case Keys.U:
            case Keys.V:
            case Keys.W:
            case Keys.X:
            case Keys.Y:
            case Keys.Z:
                if (allHeldKeys.Contains(Keys.LeftShift) || allHeldKeys.Contains(Keys.RightShift))
                    String += pressedKey.ToString();
                else
                    String += pressedKey.ToString().ToLower();
                break;
            }
        }
        */
    }
}