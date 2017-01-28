using System;
using Legatus.Input;

namespace Legatus.UI
{
    public delegate void UIButtonDelegate(object sender, EventArgs e);

    public abstract class UIButton : UIActive
    {
        public event UIButtonDelegate OnClick;
        
        public UIButton(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
        }

        public UIButton(int x, int y, int width, int height, UIButtonDelegate onClick)
            : base(x, y, width, height)
        {
            OnClick += onClick;
        }

        public override UIElement OnMouseAction(MouseActionEventArgs mouse)
        {
            UIElement intercept = base.OnMouseAction(mouse);
            if (intercept == this && mouse.Action == MouseAction.Released && mouse.Button == MouseButton.Left && OnClick != null)
            {
                OnClick(this, EventArgs.Empty);
            }
            return intercept;
        }
    }
}
