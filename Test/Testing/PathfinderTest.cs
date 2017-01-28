using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus.Input;
using Legatus.Pathfinding;
using Legatus;

namespace TestProject.Testing
{
    internal class PathfinderTest : Test, IInputReceiver
    {
        private class Tile : PathfinderNode
        {
            public class TileLink : SimpleLink
            {
                public readonly bool IsDiagonal;

                public TileLink(Tile target, int cost, bool isDiagonal)
                    : base(target, cost)
                {
                    IsDiagonal = isDiagonal;
                }
            }

            public readonly int X, Y;
            public List<TileLink> Neighbors = new List<TileLink>();
            public Unit Unit;
            public bool IsObstacle;

            public Vector2 VectorPosition { get { return new Vector2(X, Y); } }
            public Vector2 ScreenPosition { get { return new Vector2(X * TileSize, Y * TileSize); } }

            public Tile(int x, int y)
            {
                X = x;
                Y = y;
            }

            public void Initialize(PathfinderTest test)
            {
                int minX = Math.Max(0, X - 1);
                int minY = Math.Max(0, Y - 1);
                int maxX = Math.Min(test.Width - 1, X + 1);
                int maxY = Math.Min(test.Height - 1, Y + 1);
                
                for (int x = minX; x <= maxX; x++)
                    for (int y = minY; y <= maxY; y++)
                    {
                        Tile tile = test.TileGrid[x, y];

                        if (tile != this)
                        {
                            bool diagonal = (Math.Abs(X - x) + Math.Abs(Y - y) == 2);
                            Neighbors.Add(new TileLink(tile, 10, diagonal));
                        }
                    }
            }

            public void UpdateNeighbors()
            {
                foreach (TileLink link in Neighbors)
                {
                    Tile tile = link.Target as Tile;

                    if (tile.IsObstacle)
                    {
                        link.Cost = 10000000;
                    }
                    else
                    {
                        link.Cost = 1000;
                    }

                    if (link.IsDiagonal)
                    {
                        //link.Cost = (int)(link.Cost * Math.Sqrt(2));
                        link.Cost = link.Cost * Math.Sqrt(2);
                    }
                }
            }

            public override double CalculateHeuristic(PathfinderNode end)
            {
                Tile other = (Tile)end;
                /*
                int d, dx = Math.Abs(other.X - X), dy = Math.Abs(other.Y - Y);
                if (dx > dy)
                {
                    d = dx - dy;
                    return (dx - d) * 1500 + d * 1000;
                }
                else
                {
                    d = dy - dx;
                    return (dy - d) * 1500 + d * 1000;
                }
                */
                return 1200 * Vector2.Distance(VectorPosition, ((Tile)end).VectorPosition);
                //return (int)(1200 * Vector2.DistanceSquared(VectorPosition, ((Tile)end).VectorPosition));
                //return 1000 * (Math.Abs(other.X - X) + Math.Abs(other.Y - Y));
            }

            public static List<Tile> ToTileList(IEnumerable<PathfinderNode> nodes)
            {
                List<Tile> tileList = new List<Tile>();

                foreach (PathfinderNode node in nodes)
                {
                    tileList.Add(node as Tile);
                }

                return tileList;
            }

            public override IEnumerable<PathfinderLink> GetNeighbors()
            {
                return Neighbors;
            }
        }

        private class Unit
        {
            public Tile Tile;
            public List<Tile> Path;

            public Vector2 VectorPosition { get { return new Vector2(X, Y); } }
            public Vector2 ScreenPosition { get { return new Vector2(X * TileSize, Y * TileSize); } }
            public int X { get { return Tile.X; } }
            public int Y { get { return Tile.Y; } }

            public Unit(Tile tile)
            {
                Tile = tile;
            }

            public void Update()
            {
                if (Path != null && Path.Count > 0)
                {
                    Tile next = Path.First();
                    if (next.Unit == null)
                    {
                        Tile.Unit = null;
                        Tile = next;
                        next.Unit = this;
                        Path.RemoveAt(0);
                    }
                    else
                    {

                    }
                }
            }

            public void MoveTo(Tile target)
            {
                Path = Tile.ToTileList(Pathfinder.FindPath(Tile, target, 10000));
                Path.RemoveAt(0);
            }
        }

        public const int TileSize = 32;

        private Tile[,] TileGrid;
        private List<Unit> Units = new List<Unit>();
        private Texture2D TileTexture;
        private SpriteFont Font;
        private Tile Start, End;
        private List<Tile> Path;
        private InputReceiverHandler Input;
        private Stopwatch Stopwatch = new Stopwatch();
        private long ElapsedTime;

        public int Width { get { return TileGrid.GetLength(0); } }
        public int Height { get { return TileGrid.GetLength(1); } }
        public Rectangle Bounds { get { return new Rectangle(0, 0, Width, Height); } }

        public PathfinderTest(Game game, int width, int height)
        {
            TileGrid = new Tile[width, height];
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                TileGrid[x, y] = new Tile(x, y);
            }
            foreach (Tile tile in TileGrid)
            {
                tile.Initialize(this);
            }
            Start = End = TileGrid[0, 0];
            Path = new List<Tile>();

            TileTexture = game.Content.Load<Texture2D>("Graphics/WhiteTile.png");
            Font = game.Content.Load<SpriteFont>("Graphics/DefaultFont8");
            Input = new InputReceiverHandler(this);
        }

        public override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);

            foreach (Unit unit in Units)
            {
                unit.Update();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (Tile tile in TileGrid)
            {
                if (tile.IsObstacle)
                {
                    spriteBatch.Draw(TileTexture, tile.ScreenPosition, null, Color.Brown, 0f, Vector2.Zero, TileSize / (float)32, SpriteEffects.None, 0.5f);
                }
                else
                {
                    spriteBatch.Draw(TileTexture, tile.ScreenPosition, null, Color.Green, 0f, Vector2.Zero, TileSize / (float)32, SpriteEffects.None, 0.5f);
                }

                if (tile.Unit != null)
                {
                    spriteBatch.Draw(TileTexture, tile.ScreenPosition, null, Color.Yellow * 0.5f, 0f, new Vector2(-TileSize / 2, -TileSize /2), TileSize / (float)64, SpriteEffects.None, 0.5f);
                }
            }
            
            int i = 0;
            foreach (Tile tile in Path)
            {
                spriteBatch.Draw(TileTexture, tile.ScreenPosition, null, Color.Blue * 0.5f, 0f, Vector2.Zero, TileSize / (float)32, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(Font, "" + i, tile.ScreenPosition, Color.White);
                //spriteBatch.DrawString(Font, "" + (tile.Heuristic) / 100, tile.ScreenPosition, Color.White);
                i++;
            }
            spriteBatch.DrawString(Font, "Time: " + ElapsedTime + " ms", Vector2.Zero, Color.White);
            
            spriteBatch.End();
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
            int x = mousePosition.X / TileSize;
            int y = mousePosition.Y / TileSize;

            if (Bounds.Contains(new Point(x, y)))
            {
                Tile tile = TileGrid[x, y];
                if (button == MouseButton.Left)
                    Start = tile;
                else if (button == MouseButton.Right)
                {
                    End = tile;
                    foreach (Tile tile2 in TileGrid)
                    {
                        tile2.UpdateNeighbors();
                    }

                    Stopwatch.Restart();
                    long time1 = Stopwatch.ElapsedMilliseconds;
                    IEnumerable<PathfinderNode> path = new List<PathfinderNode>();
                    for (int i = 0; i < 1000; i++)
                        path = Pathfinder.FindPathDebug(Start, End, 10000);
                    //Path = Tile.ToTileList(Pathfinder.FindPathDebug(Start, End, 10000));
                    //Path = Tile.ConvertNodeList(Pathfinder.FindNodesWithinRange(Start, 5000));
                    long time2 = Stopwatch.ElapsedMilliseconds;
                    ElapsedTime = time2 - time1;
                    Path = Tile.ToTileList(path);

                    foreach (Unit unit in Units)
                    {
                        unit.MoveTo(tile);
                    }
                }
                else if (button == MouseButton.Middle)
                {
                    if (tile.Unit == null)
                    {
                        Unit unit = new Unit(tile);
                        tile.Unit = unit;
                        Units.Add(unit);
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
            int x = mousePosition.X / TileSize;
            int y = mousePosition.Y / TileSize;

            if (Bounds.Contains(new Point(x, y)))
            {

                Tile tile = TileGrid[x, y];
                if (tile != null)
                {
                    if (allHeldKeys.Contains(Keys.LeftControl))
                    {
                        if (button == MouseButton.Left)
                            tile.IsObstacle = true;
                        else if (button == MouseButton.Right)
                            tile.IsObstacle = false;
                    }
                    else if (allHeldKeys.Contains(Keys.LeftShift))
                    {
                        if (button == MouseButton.Left)
                            Start = tile;
                        else if (button == MouseButton.Right)
                        {
                            End = tile;
                            foreach (Tile tile2 in TileGrid)
                            {
                                tile2.UpdateNeighbors();
                            }

                            Stopwatch.Restart();
                            long time1 = Stopwatch.ElapsedMilliseconds;
                            Path = Tile.ToTileList(Pathfinder.FindPath(Start, End, 10000));
                            long time2 = Stopwatch.ElapsedMilliseconds;
                            ElapsedTime = time2 - time1;
                        }
                    }
                }
            }
        }
    }
}
