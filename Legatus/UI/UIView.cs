using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Input;

namespace Legatus.UI
{
    public interface IUIView
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds);
        void OnKeyboardAction(KeyboardEventArgs keyboard);
        void OnMouseAction(MouseActionEventArgs mouse);
        void OnMouseDrag(MouseDragEventArgs mouse);
        void OnMouseOver(MouseOverEventArgs mouse);
        void OnMouseScroll(MouseScrollEventArgs mouse);
    }
}
