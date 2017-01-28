using Legatus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legatus.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus.Graphics;
using Legatus.Pathfinding;

namespace Fortress
{
    public class MapView : IUIView
    {
        private GameMap Map;
        private Vector2 Camera;
        private float Zoom = 1.0f;

        private FortressGame Game { get { return Map.Game; } }
        private ModManager Mods { get { return Game.Mods; } }

        public MapView(GameMap map)
        {
            Map = map;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds)
        {
            float scale = 32 * Zoom;
            int minX = Math.Max((int)(Camera.X / 32 - 1), 0);
            int minY = Math.Max((int)(Camera.Y / 32 - 1), 0);
            int maxX = Math.Min((int)((Camera.X / 32 + bounds.Width / scale) + 2), Map.SizeX);
            int maxY = Math.Min((int)((Camera.Y / 32 + bounds.Height / scale) + 2), Map.SizeY);

            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                {
                    Tile tile = Map.Tiles[x, y];
                    spriteBatch.Draw(tile.Sprite.Texture, (tile.ScreenPosition - Camera) * Zoom, tile.Sprite.Source, 
                        Color.White, 0f, Vector2.Zero, Zoom, SpriteEffects.None, tile.DrawDepth);
                }
            foreach (Creature creature in Map.Creatures)
            {
                BasicSprite sprite = Mods.Textures["Human"];
                spriteBatch.Draw(sprite.Texture, (creature.ScreenPosition - Camera) * Zoom, sprite.Source,
                    Color.White, 0f, Vector2.Zero, Zoom, SpriteEffects.None, 1.0f);
            }
            spriteBatch.End();
        }

        public void OnKeyboardAction(KeyboardEventArgs keyboard)
        {
            switch (keyboard.Action)
            {
                case KeyboardAction.Released:
                    switch (keyboard.Key)
                    {
                        case Keys.R:
                            Map.Regenerate(100, 100, new Random());
                            break;
                    }
                    break;
            }
        }

        public void OnMouseAction(MouseActionEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MouseButton.Left:
                    switch (mouse.Action)
                    {
                        case MouseAction.Released:
                            Point targetPosition = ((mouse.Position.ToVector2() + Camera * Zoom) / Zoom / 32).ToPoint();
                            if (Map.Bounds.Contains(targetPosition))
                            {
                                Tile target = Map.Tiles[targetPosition.X, targetPosition.Y];
                                foreach (Creature creature in Map.Creatures)
                                {
                                    creature.WalkTo(target);
                                }
                            }
                            break;
                    }
                    break;
                case MouseButton.Middle:
                    switch (mouse.Action)
                    {
                        case MouseAction.Released:
                            Zoom = 1.0f;
                            break;
                    }
                    break;
            }
        }

        public void OnMouseDrag(MouseDragEventArgs mouse)
        {
            if (mouse.Button == MouseButton.Right)
                Camera += mouse.DeltaToPrevious.ToVector2() / Zoom;
            
            if (Camera.X < 0)
                Camera.X = 0;
            if (Camera.Y < 0)
                Camera.Y = 0;
            
        }

        public void OnMouseOver(MouseOverEventArgs mouse)
        {

        }

        public void OnMouseScroll(MouseScrollEventArgs mouse)
        { 
            int sign = Math.Sign(mouse.Delta);
            int abs = Math.Abs(mouse.Delta);

            for (int i = 0; i < abs; i++)
            {
                if (sign < 0)
                {
                    Zoom = Zoom * 0.999f;
                }
                else
                {
                    Zoom = Zoom / 0.999f;
                }
            }

            if (Zoom > 4f)
                Zoom = 4f;
        }
    }
}
