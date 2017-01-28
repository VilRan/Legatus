using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Input;

namespace Legatus.UI
{
    public abstract class UIBranch : UIActive
    {
        protected List<UIElement> Children = new List<UIElement>();

        public UIBranch(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
        }

        public void AddChild(UIElement element)
        {
            element.Parent = this;
            if (element.IsAlwaysAtBottom)
                Children.Add(element);
            else
                InsertToTop(element);
        }

        public void RemoveChild(UIElement element)
        {
            Children.Remove(element);
        }
        /*
        public override void OnMove(Point delta)
        {
            foreach (UIElement child in Children)
            {
                child.MoveBy(delta);
            }
        }
        */
        public override void Update(GameTime gameTime)
        {
            foreach (UIElement child in Children)
            {
                child.Update(gameTime);
            }
        }

        public override void Predraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (UIElement child in Children)
            {
                child.Predraw(gameTime, spriteBatch);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (UIElement child in Children)
            {
                child.Draw(gameTime, spriteBatch);
            }
        }

        public override UIElement OnMouseOver(MouseOverEventArgs mouse)
        {
            UIElement intercept;
            foreach (UIElement child in Children)
                if ((intercept = child.OnMouseOver(mouse)) != null)
                    return intercept;
            return base.OnMouseOver(mouse);
        }

        public override UIElement OnMouseAction(MouseActionEventArgs mouse)
        {
            UIElement intercept;
            foreach (UIElement child in Children)
                if ((intercept = child.OnMouseAction(mouse)) != null)
                {
                    if (mouse.Button == MouseButton.Left && mouse.Action == MouseAction.Pressed)
                    {
                        if ( ! child.IsAlwaysAtBottom)
                        {
                            Children.Remove(child);
                            InsertToTop(child);
                        }
                    }

                    return intercept;
                }
            return base.OnMouseAction(mouse);
        }

        public override UIElement OnMouseDrag(MouseDragEventArgs mouse)
        {
            UIElement intercept;
            foreach (UIElement child in Children)
                if ((intercept = child.OnMouseDrag(mouse)) != null)
                    return intercept;
            return base.OnMouseDrag(mouse);
        }

        public override UIElement OnMouseScroll(MouseScrollEventArgs mouse)
        {
            UIElement intercept;
            foreach (UIElement child in Children)
                if ((intercept = child.OnMouseScroll(mouse)) != null)
                    return intercept;
            return base.OnMouseScroll(mouse);
        }

        private void InsertToTop(UIElement element)
        {
            int i = 0;
            if (!element.IsAlwaysAtTop)
                foreach (UIElement e in Children)
                    if (e.IsAlwaysAtTop) i++; else break;

            Children.Insert(i, element);
        }
    }
}
