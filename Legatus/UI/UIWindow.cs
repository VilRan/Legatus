using Legatus.Input;

namespace Legatus.UI
{
    public abstract class UIWindow : UIBranch
    {
        private bool BodyCaught;

        public UIWindow(int x, int y, int width, int height)
            : base(x, y, width, height)
        {

        }

        public override UIElement OnMouseAction(MouseActionEventArgs mouse)
        {
            UIElement intercept = base.OnMouseAction(mouse);

            if (mouse.Button == MouseButton.Left)
            {
                if (mouse.Action == MouseAction.Pressed && intercept == this)
                    BodyCaught = true;
                else if (mouse.Action == MouseAction.Released)
                    BodyCaught = false;
            }

            return intercept;
        }

        public override UIElement OnMouseDrag(MouseDragEventArgs mouse)
        {
            if (mouse.Button == MouseButton.Left)
            {
                if (BodyCaught)
                {
                    MoveBy(mouse.CurrentPosition - mouse.PreviousPosition);
                    return this;
                }
            }

            return base.OnMouseDrag(mouse);
        }
    }
}
