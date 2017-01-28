using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus;
using Legatus.UI;
using Legatus.Input;

namespace TestProject.Testing
{
    internal class LOSTest : Test, IInputReceiver
    {
        private class Tile
        {
            //public readonly LOSTest Map;
            public readonly int X, Y, Index;
            public bool IsObstacle;
            public BitArray Visibility;

            public Point Position { get { return new Point(X, Y); } }
            public Vector2 ScreenPosition { get { return new Vector2(X * 32, Y * 32); } }
            
            public Tile(LOSTest map, int x, int y)
            {
                //Map = map;
                X = x;
                Y = y;
                Index = x + y * map.Width;
                Visibility = new BitArray(map.Size);
            }

            public bool IsVisibleFrom(Tile other)
            {
                return other.Visibility[Index];
            }
        }

        private readonly int Width, Height, Size;
        private Tile[,] Tiles;
        private Tile POVTile;
        private InputReceiverHandler Input;
        private Texture2D TileTexture;
        private List<Point> Line;
        private Point LineOrigin;

        public LOSTest(LegatusGame game, int width, int height)
        {
            Width = width;
            Height = height;
            Size = Width * Height;
            Tiles = new Tile[Width, Height];
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                Tiles[x, y] = new Tile(this, x, y);
            }
            POVTile = Tiles[0, 0];
            Input = new InputReceiverHandler(this);
            TileTexture = game.Content.Load<Texture2D>("Graphics/WhiteTile.png");

            Line = new List<Point>();
            LineOrigin = Point.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (Tile tile in Tiles)
            {
                if (tile.IsObstacle)
                {
                    spriteBatch.Draw(TileTexture, tile.ScreenPosition, Color.Brown);
                }
                else
                {
                    spriteBatch.Draw(TileTexture, tile.ScreenPosition, Color.Green);
                }

                if ( ! tile.IsVisibleFrom(POVTile))
                    spriteBatch.Draw(TileTexture, tile.ScreenPosition, Color.Black * 0.8f);
            }

            foreach (Point point in Line)
            {
                spriteBatch.Draw(TileTexture, new Vector2(point.X * 32, point.Y * 32), Color.Blue * 0.5f);
            }

            spriteBatch.End();
        }

        public void UpdateVisibility()
        {
            foreach (Tile tile in Tiles)
            {
                foreach (Tile other in Tiles)
                {
                    tile.Visibility[other.Index] = true;

                    foreach (Point point in Bresenham.GetPointsOnLine(tile.X, tile.Y, other.X, other.Y))
                    {
                        if (Tiles[point.X, point.Y].IsObstacle)
                        {
                            tile.Visibility[other.Index] = false;
                            break;
                        }
                    }
                }
            }
        }

        public void OnKeyPressed(Keys pressedKey, Keys[] allHeldKeys)
        {

        }
        public void OnKeyRepeated(Keys repeatedKey, Keys[] allHeldKeys)
        {

        }
        public void OnKeyHeld(Keys heldKey, Keys[] allHeldKeys)
        {

        }
        public void OnKeyReleased(Keys releasedKey, Keys[] allHeldKeys)
        {

        }
        public void OnMouseOver(Point mousePosition, Keys[] allHeldKeys)
        {

        }
        public void OnMouseScroll(Point mousePosition, int delta, Keys[] allHeldKeys)
        {

        }
        public void OnMousePressed(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {
            int x = mousePosition.X / 32;
            int y = mousePosition.Y / 32;
            Tile tile = Tiles[x, y];
            if (tile != null)
            {
                if (!allHeldKeys.Contains(Keys.LeftControl))
                {
                    if (button == MouseButton.Middle)
                    {
                        POVTile = tile;
                        UpdateVisibility();
                    }
                    else if (button == MouseButton.Left)
                    {
                        LineOrigin = tile.Position;
                    }
                    else if (button == MouseButton.Right)
                    {
                        if (allHeldKeys.Contains(Keys.LeftShift))
                            foreach (Point point in Bresenham.GetPointsOnLine(LineOrigin.X, LineOrigin.Y, tile.X, tile.Y))
                            {
                                Line.Add(point);
                            }
                        else
                            Line = Bresenham.GetPointsOnLine(LineOrigin.X, LineOrigin.Y, tile.X, tile.Y).ToList();
                    }
                }
            }
        }
        public void OnMouseReleased(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }
        public void OnMouseDoubleClick(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {
            
        }
        public void OnMouseDrag(MouseButton button, Point mousePosition, Point previousPosition, Point startPosition, Keys[] allHeldKeys)
        {
            int x = mousePosition.X / 32;
            int y = mousePosition.Y / 32;
            Tile tile = Tiles[x, y];
            if (tile != null)
            {
                if (allHeldKeys.Contains(Keys.LeftControl))
                {
                    if (button == MouseButton.Left)
                        tile.IsObstacle = true;
                    else if (button == MouseButton.Right)
                        tile.IsObstacle = false;
                }
            }
        }
    }
}
