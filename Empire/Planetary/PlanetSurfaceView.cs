using System;
using Legatus.Input;
using Legatus.Mathematics;
using Legatus.Graphics;
using Legatus.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Empire.Planetary
{
    public class PlanetSurfaceView : IUIView
    {
        private const int TileSize = 32;

        private static readonly Vector2 TileMargin = new Vector2(-16, -16);

        public PlanetSurface Surface;
        private readonly EmpireGame Game;
        private readonly PlanetSurfaceCamera Camera;
        private Tile FocusTile;
        private SpriteFont Font;
        private bool ShouldDrawGrid;
        private bool ShouldDrawInfo;

        public PlanetSurfaceView(EmpireGame game, PlanetSurface surface)
        {
            Game = game;
            Surface = surface;
            FocusTile = surface.TileGrid[0, 0];
            Camera = new PlanetSurfaceCamera(this);
            Font = game.Content.Load<SpriteFont>("Graphics/DefaultFont12");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, null, null);

            Vector2 TileOffset = TileMargin - Camera.PositionVector;

            int minX = Camera.X / TileSize;
            int minY = Math.Max(0, Camera.Y / TileSize);
            int maxX = minX + bounds.Width / TileSize + 2;
            int maxY = Math.Min(Surface.SizeY, minY + bounds.Height / TileSize + 2);

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    int x2 = x % Surface.SizeX;
                
                    Tile tile = Surface.TileGrid[x2, y];
                    spriteBatch.Draw(tile.Sprite.Texture, new Vector2(x * TileSize, y * TileSize) + TileOffset, tile.Sprite.Source, 
                        tile.Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, tile.DrawDepth);
                    //spriteBatch.Draw(tile.Texture, new Vector2(x * TileSize, y * TileSize) + TileOffset - Camera.PositionVector, tile.Color);

                    if (ShouldDrawGrid)
                        spriteBatch.DrawLine(new Vector2(minX * TileSize, y * TileSize) - Camera.PositionVector, new Vector2(maxX * TileSize, y * TileSize) - Camera.PositionVector, Color.Black, 0.1f);
                }

                if (ShouldDrawGrid)
                    spriteBatch.DrawLine(new Vector2(x * TileSize, minY * TileSize) - Camera.PositionVector, new Vector2(x * TileSize, maxY * TileSize) - Camera.PositionVector, Color.Black, 0.1f);
            }

            if (ShouldDrawInfo)
                DrawInfo(gameTime, spriteBatch);

            //Game.Textures.TestDraw(spriteBatch);

            spriteBatch.End();
        }

        private void DrawInfo(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 textPosition = new Vector2();
            spriteBatch.DrawString(Font, "FPS: " + Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds, 1) + " (" + Math.Round(gameTime.ElapsedGameTime.TotalMilliseconds, 1) + " ms/f)", textPosition, Color.White); textPosition += new Vector2(0, 16);
            spriteBatch.DrawString(Font, "Planet", textPosition, Color.White); textPosition += new Vector2(0, 16);
            spriteBatch.DrawString(Font, "  Size:       " + Surface.SizeX + " x " + Surface.SizeY + " = " + Surface.Area, textPosition, Color.White); textPosition += new Vector2(0, 16);
            spriteBatch.DrawString(Font, "Tile ( " + FocusTile.X + ", " + FocusTile.Y + " )", textPosition, Color.White); textPosition += new Vector2(0, 16);
            spriteBatch.DrawString(Font, "  Coordinates:    " + FocusTile.Longitude + ", " + FocusTile.Latitude, textPosition, Color.White); textPosition += new Vector2(0, 16);
            spriteBatch.DrawString(Font, "  Altitude:       " + FocusTile.Altitude + " m", textPosition, Color.White); textPosition += new Vector2(0, 16);
            spriteBatch.DrawString(Font, "  Temperature:       " + Math.Round(FocusTile.Temperature) + " °C", textPosition, Color.White); textPosition += new Vector2(0, 16);
            spriteBatch.DrawString(Font, "  Biome:          " + FocusTile.Biome.Name, textPosition, Color.White); textPosition += new Vector2(0, 16);
            spriteBatch.DrawString(Font, "  Wind:           " + FocusTile.Wind.ToString(), textPosition, Color.White); textPosition += new Vector2(0, 16);
        }

        public void OnKeyboardAction(KeyboardEventArgs keyboard)
        {
            if (keyboard.Action == KeyboardAction.Released)
            {
                switch (keyboard.Key)
                {
                    case Keys.G:
                        ShouldDrawGrid = !ShouldDrawGrid; break;
                    case Keys.I:
                        ShouldDrawInfo = !ShouldDrawInfo; break;
                    case Keys.R:
                        int width = Game.RNG.Next(10, 100);
                        Surface = new PlanetSurface(width * 2, width, Game.Data, Game.RNG);
                        Camera.MoveTo(Point.Zero);
                        break;
                    case Keys.Space:
                        Surface.Update(Game.RNG);
                        break;
                }
            }
        }

        public void OnMouseAction(MouseActionEventArgs mouse)
        {

        }

        public void OnMouseDrag(MouseDragEventArgs mouse)
        {
            Camera.MoveBy(mouse.PreviousPosition - mouse.CurrentPosition);
        }

        public void OnMouseOver(MouseOverEventArgs mouse)
        {
            Point position = ScreenToWorld(mouse.Position);
            FocusTile = Surface.TileGrid[position.X, position.Y];
        }

        public void OnMouseScroll(MouseScrollEventArgs mouse)
        {

        }

        private Point ScreenToWorld(Point position)
        {
            position += Camera.PositionPoint;

            position.X /= TileSize;
            position.Y /= TileSize;

            position.X = MathExtra.Wrap(position.X, Surface.SizeX);
            position.Y = MathHelper.Clamp(position.Y, 0, Surface.SizeY - 1);
            /*
            if (position.X < 0)
                position.X += position.X % Surface.SizeX;
            else if (position.X >= Surface.SizeX)
                position.X %= Surface.SizeX;
            if (position.Y < 0)
                position.Y = 0;
            else if (position.Y >= Surface.SizeY)
                position.Y = Surface.SizeY - 1;
            */

            return position;
        }
    }
}
