using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus;
using Legatus.Input;
using Legatus.MapGeneration;

namespace TestProject.Testing
{
    internal class IsometricTest : Test
    {
        private class Tile
        {
            public readonly IsometricTest Test;
            public readonly Vector2 ScreenPosition;
            public readonly int X, Y, Z;
            public Texture2D Texture;
            public Color Color;
            public bool IsSolid;
            public bool IsLiquid;

            public bool IsVisible
            {
                get
                {
                    if (Z == Test.SizeZ
                        || X == 0
                        || X == Test.SizeX - 1
                        || Y == 0
                        || Y == Test.SizeY - 1)
                        return true;
                    return !(Test.TileGrid[X, Y, Z + 1].IsSolid 
                        && Test.TileGrid[X + 1, Y, Z].IsSolid
                        && Test.TileGrid[X - 1, Y, Z].IsSolid
                        && Test.TileGrid[X, Y + 1, Z].IsSolid
                        && Test.TileGrid[X, Y - 1, Z].IsSolid);
                }
            }

            public Tile(IsometricTest test, int x, int y, int z)
            {
                Test = test;
                X = x;
                Y = y;
                Z = z;

                ScreenPosition = new Vector2(X * 32 + Y * -32, X * 16 + Y * 16 + Z * -32);
            }
        }

        private Game Game;
        private InputEventHandler Input;
        private Random RNG;
        private int _FocusLevel;
        private Vector2 Camera;
        private Texture2D Block;
        private Texture2D BorderlessBlock;
        private Texture2D Floor;
        private Texture2D BorderlessFloor;
        private Texture2D Wall;
        private Texture2D White;
        private SpriteFont Font;
        private Tile[, ,] TileGrid;
        private bool ShowAllLevels;

        private int FocusLevel { get { return _FocusLevel; } set { _FocusLevel = MathHelper.Clamp(value, 0, SizeZ - 1); } }

        private Rectangle WholeScreen { get { return new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height); } }
        private int SizeX { get { return TileGrid.GetLength(0); } }
        private int SizeY { get { return TileGrid.GetLength(1); } }
        private int SizeZ { get { return TileGrid.GetLength(2); } }

        public IsometricTest(LegatusGame game, int width, int depth, int height)
        {
            Game = game;
            Input = new InputEventHandler();
            Input.KeyboardAction += OnKeyboardAction;
            Input.MouseDrag += OnMouseDrag;
            Input.MouseScroll += OnMouseScroll;
            RNG = new Random();
            Block = game.Content.Load<Texture2D>("Graphics/Isometric/IsometricBlock.png");
            BorderlessBlock = game.Content.Load<Texture2D>("Graphics/Isometric/IsometricBlockBorderless.png");
            Floor = game.Content.Load<Texture2D>("Graphics/Isometric/IsometricFloor.png");
            BorderlessFloor = game.Content.Load<Texture2D>("Graphics/Isometric/IsometricFloorBorderless.png");
            Wall = game.Content.Load<Texture2D>("Graphics/Isometric/IsometricWall.png");
            White = game.Content.Load<Texture2D>("Graphics/WhiteTile.png");
            Font = game.Content.Load<SpriteFont>("Graphics/DefaultFont12");
            TileGrid = new Tile[width, depth, height];
            for (int z = 0; z < height; z++)
            for (int y = 0; y < depth; y++)
            for (int x = 0; x < width; x++)
            {
                TileGrid[x, y, z] = new Tile(this, x, y, z);
            }
            GenerateMap();
        }

        private void GenerateMap()
        {
            double[,] heightmap = new DiamondSquare(RNG, SizeX, SizeY, 1, 0.55, false, false).GenerateHeightmap();
            //heightmap = HydraulicErosion(heightmap, 2);
            double heightmapWidth = heightmap.GetUpperBound(0);
            double heightmapDepth = heightmap.GetUpperBound(1);
            double widthRatio = heightmapWidth / (double)SizeX;
            double heightRatio = heightmapDepth / (double)SizeY;
            int baseHeight = 5;
            int altitudeScale = 10;
            int sealevel = 5;// RNG.Next(-10000, 10000);

            for (int x = 0; x < SizeX; x++)
            for (int y = 0; y < SizeX; y++)
            {
                int x2 = (int)Math.Round((x) * widthRatio);
                int y2 = (int)Math.Round((y) * heightRatio);
                double altitude = baseHeight + heightmap[x2, y2] * altitudeScale;
                double temperature = 30 - 0.01 * altitude;
                
                for (int z = 0; z < SizeZ; z++)
                {
                    Tile tile = TileGrid[x, y, z];
                    if (z <= altitude)
                    {
                        tile.IsSolid = true;
                        tile.IsLiquid = false;
                        tile.Texture = BorderlessBlock;
                        tile.Color = Color.ForestGreen;
                    }
                    else if (z <= sealevel)
                    {
                        tile.IsLiquid = true;
                        tile.IsSolid = false;
                        tile.Texture = BorderlessFloor;
                        tile.Color = Color.Navy * 0.35f;
                    }
                    else
                    {
                        tile.IsSolid = false;
                        tile.IsLiquid = false;
                    }
                }
            }

        }

        public override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int minZ;
            if (ShowAllLevels)
                minZ = 0;
            else
                minZ = Math.Max(0, FocusLevel - 8);

            spriteBatch.Begin(SpriteSortMode.Immediate);
            for (int z = minZ; (ShowAllLevels || z <= FocusLevel) && z < SizeZ; z++)
            {
                for (int y = 0; y < SizeY; y++)
                for (int x = 0; x < SizeX; x++)
                {
                    Tile tile = TileGrid[x, y, z];
                    if (tile.IsSolid)
                    {
                        if (tile.IsVisible)
                            spriteBatch.Draw(tile.Texture, tile.ScreenPosition + Camera, tile.Color);
                        else if (z == FocusLevel)
                            spriteBatch.Draw(tile.Texture, tile.ScreenPosition + Camera, Color.Black);
                    }
                    else if (tile.IsLiquid)
                    {
                        if (tile.IsVisible)
                            spriteBatch.Draw(tile.Texture, tile.ScreenPosition + Camera, tile.Color);
                        else if (z == FocusLevel)
                            spriteBatch.Draw(tile.Texture, tile.ScreenPosition + Camera, Color.Black);
                    }
                }

                if (!ShowAllLevels && z < FocusLevel)
                {
                    float opacity = 0.05f;
                    spriteBatch.Draw(White, WholeScreen, Color.Black * opacity);
                }
            }

            int time = gameTime.ElapsedGameTime.Milliseconds;
            spriteBatch.DrawString(Font, "FPS: " + (1000 / Math.Max(1, time)) + " (" + time + " ms)", Vector2.Zero, Color.White);

            spriteBatch.End();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void OnKeyboardAction(object sender, KeyboardEventArgs keyboard)
        {
            if (keyboard.Action == KeyboardAction.Released)
                OnKeyReleased(sender, keyboard.Key, keyboard.HeldKeys);
        }

        private void OnKeyReleased(object sender, Keys releasedKey, Keys[] allHeldKeys)
        {
            if (releasedKey == Keys.R)
                GenerateMap();
            else if (releasedKey == Keys.S)
                ShowAllLevels = !ShowAllLevels;
        }

        private void OnMouseScroll(object sender, MouseScrollEventArgs args)
        {
            if (args.Delta> 0)
            {
                FocusLevel += 1;
                Camera += new Vector2(0, 32);
            }
            else if (args.Delta < 0)
            {
                FocusLevel -= 1;
                Camera -= new Vector2(0, 32);
            }
        }

        private void OnMouseDrag(object sender, MouseDragEventArgs args)
        {
            Camera -= (args.PreviousPosition- args.CurrentPosition).ToVector2();
        }
    }
}
