using Legatus.Input;
using Microsoft.Xna.Framework;

namespace Legatus.UI
{
    public abstract class UIActive : UIElement
    {
        public UIActive(int x, int y, int width, int height)
            : base(x, y, width, height)
        {

        }
        
        public override UIElement OnMouseOver(MouseOverEventArgs mouse) { return CheckIntercept(mouse.Position); }
        public override UIElement OnMouseAction(MouseActionEventArgs mouse) { return CheckIntercept(mouse.Position); }
        public override UIElement OnMouseDrag(MouseDragEventArgs mouse) { return CheckIntercept(mouse.StartPosition); }
        public override UIElement OnMouseScroll(MouseScrollEventArgs mouse) { return CheckIntercept(mouse.Position); }

        private UIElement CheckIntercept(Point point)
        {
            if (Bounds.Contains(point)) return this; else return null;
        }
    }
}
