using Legatus.Input;
using Legatus.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Legatus.UI
{
    /// <summary>
    /// A simple element that draws a sprite at its position and is transparent to input.
    /// </summary>
    public class UIDecoration : UIElement
    {
        public BasicSprite Sprite;

        public UIDecoration(Rectangle bounds, BasicSprite sprite)
            : base(bounds)
        {
            Sprite = sprite;
        }

        public UIDecoration(int x, int y, int width, int height, BasicSprite sprite)
            : base(x, y, width, height)
        {
            Sprite = sprite;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Texture, PositionVector, Sprite.Source, Color);
        }

        public override UIElement OnMouseOver(MouseOverEventArgs mouse)
        {
            return null;
        }

        public override UIElement OnMouseAction(MouseActionEventArgs mouse)
        {
            return null;
        }

        public override UIElement OnMouseDrag(MouseDragEventArgs mouse)
        {
            return null;
        }

        public override UIElement OnMouseScroll(MouseScrollEventArgs mouse)
        {
            return null;
        }
    }
}
