using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Graphics;

namespace Legatus.UI
{
    public class UIButtonSimple : UIButton
    {
        public BasicSprite Sprite;

        public UIButtonSimple(int x, int y, int width, int height, BasicSprite sprite)
            : base(x, y, width, height)
        {
            Sprite = sprite;
        }

        public UIButtonSimple(int x, int y, int width, int height, UIButtonDelegate onClick, BasicSprite sprite)
            : base(x, y, width, height, onClick)
        {
            Sprite = sprite;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Texture, PositionVector, Sprite.Source, Color);
        }
    }
}
