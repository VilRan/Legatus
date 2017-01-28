using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Legatus.Input
{
    public enum KeyboardAction { Pressed, Repeated, Released, Held }
    public enum MouseAction { Pressed, Released, DoubleClick }
    public enum MouseButton { Left, Middle, Right, XButton1, XButton2 }

    public class KeyboardEventArgs : EventArgs
    {
        public readonly KeyboardAction Action;
        public readonly Keys Key;
        public readonly Keys[] HeldKeys;
        public readonly GameTime GameTime;

        public KeyboardEventArgs(KeyboardAction action, Keys key, Keys[] heldKeys, GameTime gameTime)
        {
            Action = action;
            Key = key;
            HeldKeys = heldKeys;
            GameTime = gameTime;
        }
    }

    public class MouseOverEventArgs : EventArgs
    {
        public readonly Point Position;
        public readonly Keys[] HeldKeys;

        public MouseOverEventArgs(Point position, Keys[] heldKeys)
        {
            Position = position;
            HeldKeys = heldKeys;
        }
    }

    public class MouseActionEventArgs : EventArgs
    {
        public readonly MouseAction Action;
        public readonly MouseButton Button;
        public readonly Point Position;
        public readonly Keys[] HeldKeys;

        public MouseActionEventArgs(MouseAction action, MouseButton button, Point position, Keys[] heldKeys)
        {
            Action = action;
            Button = button;
            Position = position;
            HeldKeys = heldKeys;
        }
    }

    public class MouseDragEventArgs : EventArgs
    {
        public readonly MouseButton Button;
        public readonly Point CurrentPosition;
        public readonly Point PreviousPosition;
        public readonly Point StartPosition;
        public readonly Keys[] HeldKeys;

        public Point DeltaFromPrevious { get { return CurrentPosition - PreviousPosition; } }
        public Point DeltaToPrevious { get { return PreviousPosition - CurrentPosition; } }
        public Point DeltaFromStart { get { return CurrentPosition - StartPosition; } }
        public Point DeltaToStart { get { return StartPosition - CurrentPosition; } }

        public MouseDragEventArgs(MouseButton button, Point currentPosition, Point previousPosition, Point startPosition, Keys[] heldKeys)
        {
            Button = button;
            CurrentPosition = currentPosition;
            PreviousPosition = previousPosition;
            StartPosition = startPosition;
            HeldKeys = heldKeys;
        }
    }

    public class MouseScrollEventArgs : EventArgs
    {
        public readonly Point Position;
        public readonly int Delta;
        public readonly Keys[] HeldKeys;

        public MouseScrollEventArgs(Point position, int delta, Keys[] heldKeys)
        {
            Position = position;
            Delta = delta;
            HeldKeys = heldKeys;
        }
    }
}
