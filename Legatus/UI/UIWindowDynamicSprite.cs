using Legatus.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Legatus.UI
{
    public class UIWindowDynamicSprite : UIDynamicSprite
    {
        public readonly int EdgeMargin;
        
        public UIWindowDynamicSprite(Texture2D texture, int gridSize, int edgeMargin)
            : base(texture, gridSize)
        {
            EdgeMargin = edgeMargin;
        }
        
        public UIWindowDynamicSprite(int edgeMargin,
            BasicSprite topLeft, BasicSprite top, BasicSprite topRight,
            BasicSprite middleLeft, BasicSprite fill, BasicSprite middleRight,
            BasicSprite bottomLeft, BasicSprite bottom, BasicSprite bottomRight
            ) : base(topLeft, top, topRight, middleLeft, fill, middleRight, bottomLeft, bottom, bottomRight)
        {
            EdgeMargin = edgeMargin;
        }
    }
}
