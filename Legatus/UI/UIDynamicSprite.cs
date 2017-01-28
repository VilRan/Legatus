using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Graphics;

namespace Legatus.UI
{
    public class UIDynamicSprite
    {
        public readonly BasicSprite TopLeft;
        public readonly BasicSprite Top;
        public readonly BasicSprite TopRight;
        public readonly BasicSprite MiddleLeft;
        public readonly BasicSprite Fill;
        public readonly BasicSprite MiddleRight;
        public readonly BasicSprite BottomLeft;
        public readonly BasicSprite Bottom;
        public readonly BasicSprite BottomRight;

        public int MinimumWidth
        {
            get
            {
                return 
                    ( TopLeft.Width > BottomLeft.Width ? TopLeft.Width : BottomLeft.Width ) 
                    + ( TopRight.Width > BottomRight.Width ? TopRight.Width : BottomRight.Width );
            }
        }
        public int MinimumHeight
        {
            get
            {
                return
                    (TopLeft.Height > TopRight.Height ? TopLeft.Height : TopRight.Height)
                    + (BottomLeft.Height > BottomRight.Height ? BottomLeft.Height : BottomRight.Height);
            }
        }
        
        public UIDynamicSprite(Texture2D texture, int gridSize)
        {
            TopLeft = new BasicSprite(texture, new Rectangle(0 * gridSize, 0 * gridSize, gridSize, gridSize));
            Top = new BasicSprite(texture, new Rectangle(1 * gridSize, 0 * gridSize, gridSize, gridSize));
            TopRight = new BasicSprite(texture, new Rectangle(2 * gridSize, 0 * gridSize, gridSize, gridSize));
            MiddleLeft = new BasicSprite(texture, new Rectangle(0 * gridSize, 1 * gridSize, gridSize, gridSize));
            Fill = new BasicSprite(texture, new Rectangle(1 * gridSize, 1 * gridSize, gridSize, gridSize));
            MiddleRight = new BasicSprite(texture, new Rectangle(2 * gridSize, 1 * gridSize, gridSize, gridSize));
            BottomLeft = new BasicSprite(texture, new Rectangle(0 * gridSize, 2 * gridSize, gridSize, gridSize));
            Bottom = new BasicSprite(texture, new Rectangle(1 * gridSize, 2 * gridSize, gridSize, gridSize));
            BottomRight = new BasicSprite(texture, new Rectangle(2 * gridSize, 2 * gridSize, gridSize, gridSize));
        }

        public UIDynamicSprite(
            BasicSprite topLeft, BasicSprite top, BasicSprite topRight, 
            BasicSprite middleLeft, BasicSprite fill, BasicSprite middleRight,
            BasicSprite bottomLeft, BasicSprite bottom, BasicSprite bottomRight
            )
        {
            TopLeft = topLeft;
            Top = top;
            TopRight = topRight;
            MiddleLeft = middleLeft;
            Fill = fill;
            MiddleRight = middleRight;
            BottomLeft = bottomLeft;
            Bottom = bottom;
            BottomRight = bottomRight;
        }
    }
}
