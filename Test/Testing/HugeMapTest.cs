
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus.Input;
using Legatus.MapGeneration;
using Legatus;
using System.Collections.Generic;
using Legatus.Graphics;
using Legatus.Mathematics;

namespace TestProject.Testing
{
    internal class HugeMapTest : Test, IInputReceiver, IDisposable
    {
        public Game Game;
        public InputReceiverHandler Input;
        public Random RNG;
        public Texture2D Map;
        public Vector2 Camera;
        public int Width, Height;

        public HugeMapTest(Game game, int width, int height)
        {
            Game = game;
            Input = new InputReceiverHandler(this);
            RNG = new Random();
            Width = width;
            Height = height;
            Camera = Vector2.Zero;

            Generate();
        }

        public void Generate()
        {
            GeneratePlanet();
            //GenerateIsland();
        }

        public void GeneratePlanet()
        {
            if (Map != null)
                Map.Dispose();

            Map = new Texture2D(Game.GraphicsDevice, Width, Height);
            Color[] data = new Color[Width * Height];

            /*
            double[,] heightmap = new DiamondSquare(RNG, Width, Height, 1, 0.7, true, false).GenerateHeightmap();
            //heightmap = HydraulicErosion(heightmap, 2);
            double heightmapWidth = heightmap.GetUpperBound(0);
            double heightmapHeight = heightmap.GetUpperBound(1);
            double widthRatio = heightmapWidth / (double)Width;
            double heightRatio = heightmapHeight / (double)Height;
            */
            int baseHeight = -500;
            int altitudeScale = 2500;
            int latitudeBonus = -1000;
            int sealevel = 0;// RNG.Next(-10000, 10000);

            ProceduralHeightmap planet = new ProceduralHeightmap(Width, Height, baseHeight, altitudeScale, 0.65, 0.1, true, false, false);
            //ProceduralPlanet planet = new ProceduralPlanet(Width, Height, baseHeight, altitudeScale, 0.65, 0.1);
            planet.Generate(RNG);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    /*
                    int x2 = (int)Math.Round((x) * widthRatio);
                    int y2 = (int)Math.Round((y) * heightRatio);
                    */
                    double latitude = (double)Math.Abs(y * 2 - Height) / Height;
                    double altitude = baseHeight + planet.GetAltitude(x, y) + latitude * latitudeBonus;
                    double temperature = 30 - 70 * latitude * latitude - 0.01 * altitude;
                    if (altitude > sealevel)
                    {
                        if (temperature > -0)
                        {
                            if (altitude > 2000)
                                data[x + y * Width] = Color.SlateGray * (float)((altitude + altitudeScale) / (altitudeScale * 2));
                            //else if (altitude > 3000)
                            //    data[x + y * Width] = Color.BurlyWood * (float)((altitude + altitudeScale) / (altitudeScale * 2));
                            //else if (altitude > 1000)
                            //    data[x + y * Width] = Color.YellowGreen * (float)((altitude + altitudeScale) / (altitudeScale * 2));
                            //else if (altitude < 100)
                            //    data[x + y * Width] = Color.SandyBrown * (float)((altitude + altitudeScale) / (altitudeScale * 2));
                            else
                                data[x + y * Width] = Color.ForestGreen * (float)((altitude + altitudeScale) / (altitudeScale * 2));
                        }
                        else
                            data[x + y * Width] = Color.Snow * (float)((altitude + altitudeScale) / (altitudeScale * 2));
                    }
                    else
                    {
                        //if (altitude > -1000 && temperature > 20)
                        //    data[x + y * Width] = Color.DarkCyan * (float)((altitude + altitudeScale) / (altitudeScale));
                        //else if (temperature > 0)
                        if (temperature > -0)
                            data[x + y * Width] = Color.Navy * (float)((altitude + altitudeScale) / (altitudeScale));
                        else
                            //data[x + y * Width] = Color.DarkCyan * (float)((altitude / 2 + altitudeScale) / (altitudeScale));
                            data[x + y * Width] = Color.Snow * (float)((altitude + altitudeScale) / (altitudeScale * 2));
                    }
                }
            
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    data[x + y * Width] = ColorExtensions.Average(GetColors(data, x, y));
                }
            
            Map.SetData(data);
        }
        
        private IEnumerable<Color> GetColors(Color[] data, int centerX, int centerY)
        {
            int minX = centerX - 1;
            int maxX = centerX + 1;
            int minY = Math.Max(0, centerY - 1);
            int maxY = Math.Min(Height - 1, centerY + 1);

            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++)
                {
                    int d = Math.Abs(centerX - x) + Math.Abs(centerY - y);
                    if (d < 2)
                    {
                        int x2 = MathExtra.Wrap(x, Width);
                        yield return data[x2 + y * Width];
                    }
                }
        }

        public double[,] HydraulicErosion(double[,] heightmap, int iterations)
        {
            int
                width = heightmap.GetLength(0),
                height = heightmap.GetLength(1);

            double 
                rainAmount = 1.0,
                solubility = 1.0,
                evaporation = 0.5,
                capacity = 1.0;

            double[,]
                waterMap = new double[width, height],
                sedimentMap = new double[width, height],
                differenceMap = new double[width, height];

            for (int i = 0; i < iterations; i++)
            {

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        waterMap[x, y] += rainAmount;
                        sedimentMap[x, y] += solubility;
                    }

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        int
                            minX = Math.Max(0, x - 1),
                            minY = Math.Max(0, y - 1),
                            maxX = Math.Min(width - 1, x + 1),
                            maxY = Math.Min(height - 1, y + 1);

                        Point lowest = new Point(x, y);

                        for (int x2 = minX; x2 <= maxX; x2++)
                            for (int y2 = minY; y2 <= maxY; y2++)
                            {
                                if (heightmap[x2, y2] < heightmap[lowest.X, lowest.Y])
                                    lowest = new Point(x2, y2);
                            }

                        double difference = heightmap[x, y] + waterMap[x, y] - heightmap[lowest.X, lowest.Y] - waterMap[lowest.X, lowest.Y];

                        if (difference > waterMap[x, y] + waterMap[lowest.X, lowest.Y])
                        {
                            waterMap[lowest.X, lowest.Y] += waterMap[x, y];
                            sedimentMap[lowest.X, lowest.Y] += sedimentMap[x, y];
                            waterMap[x, y] = 0;
                            sedimentMap[x, y] = 0;
                        }
                        else if (difference > 0)
                        {
                            waterMap[lowest.X, lowest.Y] += difference / 2;
                            sedimentMap[lowest.X, lowest.Y] += difference / 2 * solubility;
                            waterMap[x, y] -= difference / 2;
                            sedimentMap[x, y] -= difference / 2 * solubility;
                        }
                    }

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        waterMap[x, y] *= evaporation;

                        double totalCapacity = waterMap[x, y] * capacity;
                        if (sedimentMap[x, y] > totalCapacity)
                        {
                            heightmap[x, y] += sedimentMap[x, y] - totalCapacity;
                            sedimentMap[x, y] -= totalCapacity;
                        }
                    }

            }

            return heightmap;
        }

        public override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Map, Camera, Color.White);
            spriteBatch.Draw(Map, Camera - new Vector2(Width, 0), Color.White);
            spriteBatch.End();
        }

        public override void Dispose()
        {
            base.Dispose();

            Map.Dispose();
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
            if (releasedKey == Keys.R)
                Generate();
        }
        public void OnMouseOver(Point mousePosition, Keys[] allHeldKeys)
        {

        }
        public void OnMouseScroll(Point mousePosition, int delta, Keys[] allHeldKeys)
        {

        }
        public void OnMousePressed(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }
        public void OnMouseReleased(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }
        public void OnMouseDoubleClick(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }
        public void OnMouseDrag(MouseButton button, Point mousePosition, Point previousPosition, Point startPosition, Keys[] allHeldKeys)
        {
            Camera += new Vector2((mousePosition.X - previousPosition.X), 0);
            if (Camera.X < 0)
                Camera.X += Width;
            else if (Camera.X > Width)
                Camera.X -= Width;
        }
    }
}
