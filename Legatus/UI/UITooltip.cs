using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Legatus.UI
{
    public class UITooltip : UIWindowDynamic
    {
        public UIText Text;

        public UITooltip(int x, int y, int width, int height, UIWindowDynamicSprite sprite, SpriteFont font)
            : base (x, y, width, height, sprite)
        {
            Color = new Color(Color.Black, 0.5f);
            Text = new UIText(x, y, width, height, font, Color.White);
            Children.Add(Text);
        }

        public override void OnResize(Point delta)
        {
            Text.ResizeBy(delta);
        }
    }
}
