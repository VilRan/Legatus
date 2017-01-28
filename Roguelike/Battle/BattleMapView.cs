using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Legatus.Input;
using Legatus.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Roguelike.Battle
{
    class BattleMapView : IUIView
    {
        public BattleMap Map;

        public BattleMapView(BattleMap map)
        {
            Map = map;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds)
        {

        }

        public void OnKeyboardAction(KeyboardEventArgs keyboard)
        {

        }

        public void OnMouseAction(MouseActionEventArgs mouse)
        {

        }

        public void OnMouseDrag(MouseDragEventArgs mouse)
        {

        }

        public void OnMouseOver(MouseOverEventArgs mouse)
        {

        }

        public void OnMouseScroll(MouseScrollEventArgs mouse)
        {

        }
    }
}
