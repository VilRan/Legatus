using Legatus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Legatus.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space.Data;

namespace Space.ShipDesigner
{
    public class ShipDesignerModuleView : IUIView
    {
        public ShipDesignerMap Map;
        private int SelectionGridSize = 48;

        public ShipDesignerModuleView(ShipDesignerMap map)
        {
            Map = map;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds)
        {
            Vector2 position = new Vector2(SelectionGridSize, SelectionGridSize);
            spriteBatch.Begin();
            foreach (ModuleData module in Map.Modules)
            {
                Vector2 offset = new Vector2(-module.Sprite.Width / 2, -module.Sprite.Height / 2);
                spriteBatch.Draw(module.Sprite.Texture, position + offset, module.Sprite.SourceRectangle, Color.White);
                foreach (TurretData turret in module.Turrets)
                {
                    //offset += turret.Position * Map.Data.GridWidth;
                    spriteBatch.Draw(turret.Sprite.Texture, position + offset, turret.Sprite.SourceRectangle, Color.White);
                }

                position += new Vector2(0, SelectionGridSize);
            }
            spriteBatch.End();
        }

        public void OnKeyboardAction(KeyboardEventArgs keyboard)
        {

        }

        public void OnMouseAction(MouseActionEventArgs mouse)
        {
            if (mouse.Button == MouseButton.Left)
            {
                if (mouse.Action == MouseAction.Released)
                {
                    int i = (mouse.Position.Y - SelectionGridSize / 2) / SelectionGridSize;
                    if (i >= 0 && i < Map.Modules.Length)
                    {
                        Map.SelectedModule = Map.Modules[i];
                    }
                    else
                        Map.SelectedModule = null;
                }
            }
            else if (mouse.Button == MouseButton.Right)
            {
                Map.SelectedModule = null;
            }

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
