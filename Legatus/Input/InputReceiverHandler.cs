using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Legatus.Input
{
    public class InputReceiverHandler : InputHandler
    {
        private readonly IInputReceiver Parent;

        public InputReceiverHandler(IInputReceiver parent)
        {
            Parent = parent;
        }

        protected override void OnKeyHeld(Keys heldKey, Keys[] allHeldKeys, GameTime gameTime)
        {
            Parent.OnKeyHeld(heldKey, allHeldKeys);
        }
        protected override void OnKeyPressed(Keys pressedKey, Keys[] allHeldKeys, GameTime gameTime)
        {
            Parent.OnKeyPressed(pressedKey, allHeldKeys);
        }
        protected override void OnKeyRepeated(Keys repeatedKey, Keys[] allHeldKeys, GameTime gameTime)
        {
            Parent.OnKeyRepeated(repeatedKey, allHeldKeys);
        }
        protected override void OnKeyReleased(Keys releasedKey, Keys[] allHeldKeys, GameTime gameTime)
        {
            Parent.OnKeyReleased(releasedKey, allHeldKeys);
        }

        protected override void OnMouseDoubleClick(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {
            Parent.OnMouseDoubleClick(button, mousePosition, allHeldKeys);
        }
        protected override void OnMouseDrag(MouseButton button, Point mousePosition, Point previousPosition, Point startPosition, Keys[] allHeldKeys)
        {
            Parent.OnMouseDrag(button, mousePosition, previousPosition, startPosition, allHeldKeys);
        }
        protected override void OnMouseOver(Point mousePosition, Keys[] allHeldKeys)
        {
            Parent.OnMouseOver(mousePosition, allHeldKeys);
        }
        protected override void OnMousePressed(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {
            Parent.OnMousePressed(button, mousePosition, allHeldKeys);
        }
        protected override void OnMouseReleased(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {
            Parent.OnMouseReleased(button, mousePosition, allHeldKeys);
        }
        protected override void OnMouseScroll(Point mousePosition, int delta, Keys[] allHeldKeys)
        {
            Parent.OnMouseScroll(mousePosition, delta, allHeldKeys);
        }

    }
}
