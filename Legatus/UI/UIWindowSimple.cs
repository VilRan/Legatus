using Legatus.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Legatus.UI
{
    public class UIWindowSimple : UIWindow
    {
        BasicSprite Sprite;

        public UIWindowSimple(int x, int y, BasicSprite sprite)
            : base (x, y, sprite.Width, sprite.Height)
        {
            Sprite = sprite;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Texture, PositionVector, Sprite.Source, Color);

            base.Draw(gameTime, spriteBatch);
        }
    }
}
