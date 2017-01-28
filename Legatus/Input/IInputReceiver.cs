using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Legatus.Input
{
    public interface IInputReceiver
    {
        void OnKeyPressed(Keys pressedKey, Keys[] allHeldKeys);
        void OnKeyRepeated(Keys repeatedKey, Keys[] allHeldKeys);
        void OnKeyHeld(Keys heldKey, Keys[] allHeldKeys);
        void OnKeyReleased(Keys releasedKey, Keys[] allHeldKeys);
        void OnMouseOver(Point mousePosition, Keys[] allHeldKeys);
        void OnMouseScroll(Point mousePosition, int delta, Keys[] allHeldKeys);
        void OnMousePressed(MouseButton button, Point mousePosition, Keys[] allHeldKeys);
        void OnMouseReleased(MouseButton button, Point mousePosition, Keys[] allHeldKeys);
        void OnMouseDoubleClick(MouseButton button, Point mousePosition, Keys[] allHeldKeys);
        void OnMouseDrag(MouseButton button, Point mousePosition, Point previousPosition, Point startPosition, Keys[] allHeldKeys);
        /*
        void OnLeftClick(Point mousePosition);
        void OnLeftDoubleClick(Point mousePosition);
        void OnLeftReleased(Point mousePosition);
        void OnLeftDrag(Point mousePosition, Point previousPosition, Point startPosition);
        void OnRightClick(Point mousePosition);
        void OnRightDoubleClick(Point mousePosition);
        void OnRightReleased(Point mousePosition);
        void OnRightDrag(Point mousePosition, Point previousPosition, Point startPosition);
        void OnMiddleClick(Point mousePosition);
        void OnMiddleDoubleClick(Point mousePosition);
        void OnMiddleReleased(Point mousePosition);
        void OnMiddleDrag(Point mousePosition, Point previousPosition, Point startPosition);
        */
    }
}
