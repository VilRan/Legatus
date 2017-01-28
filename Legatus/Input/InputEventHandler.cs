using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Legatus.Input
{
    public delegate void KeyboardEventDelegate(object sender, KeyboardEventArgs e);
    public delegate void MouseOverEventDelegate(object sender, MouseOverEventArgs e);
    public delegate void MouseEventDelegate(object sender, MouseActionEventArgs e);
    public delegate void MouseDragEventDelegate(object sender, MouseDragEventArgs e);
    public delegate void MouseScrollEventDelegate(object sender, MouseScrollEventArgs e);

    public class InputEventHandler : InputHandler
    {
        public event KeyboardEventDelegate KeyboardAction;
        public event MouseOverEventDelegate MouseOver;
        public event MouseEventDelegate MouseAction;
        public event MouseDragEventDelegate MouseDrag;
        public event MouseScrollEventDelegate MouseScroll;

        public InputEventHandler()
        {
        }

        protected override void OnKeyHeld(Keys heldKey, Keys[] allHeldKeys, GameTime gameTime)
        {
            if (KeyboardAction != null)
                KeyboardAction(this, new KeyboardEventArgs(Input.KeyboardAction.Held, heldKey, allHeldKeys, gameTime));
        }
        protected override void OnKeyPressed(Keys pressedKey, Keys[] allHeldKeys, GameTime gameTime)
        {
            if (KeyboardAction != null)
                KeyboardAction(this, new KeyboardEventArgs(Input.KeyboardAction.Pressed, pressedKey, allHeldKeys, gameTime));
        }
        protected override void OnKeyRepeated(Keys repeatedKey, Keys[] allHeldKeys, GameTime gameTime)
        {
            if (KeyboardAction != null)
                KeyboardAction(this, new KeyboardEventArgs(Input.KeyboardAction.Repeated, repeatedKey, allHeldKeys, gameTime));
        }
        protected override void OnKeyReleased(Keys releasedKey, Keys[] allHeldKeys, GameTime gameTime)
        {
            if (KeyboardAction != null)
                KeyboardAction(this, new KeyboardEventArgs(Input.KeyboardAction.Released, releasedKey, allHeldKeys, gameTime));
        }

        protected override void OnMouseOver(Point mousePosition, Keys[] allHeldKeys)
        {
            if (MouseOver != null)
                MouseOver(this, new MouseOverEventArgs(mousePosition, allHeldKeys));
        }
        protected override void OnMousePressed(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {
            if (MouseAction != null)
                MouseAction(this, new MouseActionEventArgs(Input.MouseAction.Pressed, button, mousePosition, allHeldKeys));
        }
        protected override void OnMouseReleased(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {
            if (MouseAction != null)
                MouseAction(this, new MouseActionEventArgs(Input.MouseAction.Released, button, mousePosition, allHeldKeys));
        }
        protected override void OnMouseDoubleClick(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {
            if (MouseAction != null)
                MouseAction(this, new MouseActionEventArgs(Input.MouseAction.DoubleClick, button, mousePosition, allHeldKeys));
        }
        protected override void OnMouseDrag(MouseButton button, Point mousePosition, Point previousPosition, Point startPosition, Keys[] allHeldKeys)
        {
            if (MouseDrag != null)
                MouseDrag(this, new MouseDragEventArgs(button, mousePosition, previousPosition, startPosition, allHeldKeys));
        }
        protected override void OnMouseScroll(Point mousePosition, int delta, Keys[] allHeldKeys)
        {
            if (MouseScroll != null)
                MouseScroll(this, new MouseScrollEventArgs(mousePosition, delta, allHeldKeys));
        }

    }
}
