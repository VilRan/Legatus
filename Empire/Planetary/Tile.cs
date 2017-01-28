using System;
using Microsoft.Xna.Framework;
using Legatus.Graphics;
using Legatus.Mathematics;
using Legatus.Pathfinding;
using System.Collections.Generic;

namespace Empire.Planetary
{
    public class Tile : PathfinderNode
    {
        private const int AltitudeMax = 10000;
        private const int AltitudeMin = -10000;
        private const float AltitudeColorMin = 0.2f;
        private const float AltitudeColorRange = 1.0f - AltitudeColorMin;
        private const float DrawDepthMin = 0.5f;
        private const float DrawDepthMax = 0.9f;
        private const float DrawDepthRange = DrawDepthMax - DrawDepthMin;
        private const float IntertropicalMax = 0.05f / 0.9f; // 5°
        private const float HorseLatitudesMin = 0.3f / 0.9f; // 30°
        private const float HorseLatitudesMax = 0.35f / 0.9f; // 35°
        private const float PolarFrontMin = 0.6f / 0.9f; // 60°
        private const float PolarFrontMax = 0.65f / 0.9f; // 65°
        private const int EvaporationMax = 100;
        private const int RainMax = 200;

        private static Vector2 NorthernPolarEasterlies = new Vector2(-0.25f, 0.25f);
        private static Vector2 NorthernPolarFront = new Vector2(0f, 0f);
        private static Vector2 NorthernWesterlies = new Vector2(0.5f, -0.5f);
        private static Vector2 CalmsOfCancer = new Vector2(0f, 0f);
        private static Vector2 NortheasterlyTrades = new Vector2(-0.5f, 0.5f);
        private static Vector2 IntertropicalConvergence = new Vector2(-0.1f, 0f);
        private static Vector2 SoutheasterlyTrades = new Vector2(-0.5f, -0.5f);
        private static Vector2 CalmsOfCapricorn = new Vector2(0f, 0f);
        private static Vector2 SouthernWesterlies = new Vector2(0.5f, 0.5f);
        private static Vector2 SouthernPolarFront = new Vector2(0f, 0f);
        private static Vector2 SouthernPolarEasterlies = new Vector2(-0.25f, -0.25f);

        public readonly int X, Y;
        private readonly PlanetSurface Surface;

        public List<PathfinderLink> Neighbors = new List<PathfinderLink>();
        public Vector2 Wind;
        public int Altitude;
        public int Moisture;
        public Biome Biome { private set; get; }
        public float DrawDepth { private set; get; }
        private Vector2 WindUpdate;
        private int SpriteIndex;

        public BasicSprite Sprite { get { return Biome.Sprites[SpriteIndex]; } }
        public Vector2 PositionVector { get { return new Vector2(X, Y); } }
        public Color Color { get { return new Color(AltitudeColorModifier, AltitudeColorModifier, AltitudeColorModifier); } }
        public float AltitudeColorModifier { get { return AltitudeColorMin + AltitudeColorRange * (Altitude - AltitudeMin) / (float)(AltitudeMax - AltitudeMin); } }
        public float LongitudeValue { get { return (2 * X - Surface.SizeX) / (float)Surface.SizeX; } }
        public float LatitudeValue { get { return (2 * Y - Surface.SizeY) / (float)Surface.SizeY; } }
        public string Longitude
        {
            get
            {
                if (LongitudeValue > 0)
                    return "" + Math.Round(LongitudeValue * 180, 1) + "° E";
                else if (LongitudeValue < 0)
                    return "" + Math.Round(Math.Abs(LongitudeValue * 180), 1) + "° W";
                else
                    return "" + 0 + "°";
            } 
        }
        public string Latitude 
        { 
            get 
            { 
                if (LatitudeValue > 0)
                    return "" + Math.Round(LatitudeValue * 90, 1) + "° S"; 
                else if (LatitudeValue < 0)
                    return "" + Math.Round(Math.Abs(LatitudeValue * 90), 1) + "° N"; 
                else
                    return "" + 0 + "°";
            } 
        }
        public float Temperature
        {
            get
            {
                float temperature = 30 - 60 * LatitudeValue * LatitudeValue;
                if (Altitude > 0)
                    temperature -= 0.0075f * Altitude;
                else
                    temperature -= 0.002f * Altitude;
                return temperature;
            }
        }
        public Vector2 PrevailingWind
        {
            get
            {
                if (LatitudeValue <= 0) // Northern Hemisphere
                {
                    float latitude = LatitudeValue;

                    if (latitude <= IntertropicalMax)
                        return IntertropicalConvergence;
                    else if (latitude <= HorseLatitudesMin)
                        return NortheasterlyTrades;
                    else if (latitude <= HorseLatitudesMax)
                        return CalmsOfCancer;
                    else if (latitude <= PolarFrontMin)
                        return NorthernWesterlies;
                    else if (latitude <= PolarFrontMax)
                        return NorthernPolarFront;
                    else
                        return NorthernPolarEasterlies;
                }
                else // Southern Hemisphere
                {
                    float latitude = -LatitudeValue;

                    if (latitude <= IntertropicalMax)
                        return IntertropicalConvergence;
                    else if (latitude <= HorseLatitudesMin)
                        return SoutheasterlyTrades;
                    else if (latitude <= HorseLatitudesMax)
                        return CalmsOfCapricorn;
                    else if (latitude <= PolarFrontMin)
                        return SouthernWesterlies;
                    else if (latitude <= PolarFrontMax)
                        return SouthernPolarFront;
                    else
                        return SouthernPolarEasterlies;
                }
            }
        }


        public Tile(PlanetSurface surface, int x, int y, int altitude, Biome biome, Random rng)
        {
            Surface = surface;
            X = x;
            Y = y;
            Altitude = altitude;
            SetBiome(biome, rng);
            DrawDepth = DrawDepthMin + DrawDepthRange * (float)rng.NextDouble();
        }

        public void SetBiome(Biome biome, Random rng)
        {
            Biome = biome;
            SpriteIndex = rng.Next(Biome.Sprites.Length);
        }

        public void CalculateUpdates(Random rng)
        {
            Vector2 variation = Vector2.Zero;
            foreach (SimpleLink link in Neighbors)
            {
                Tile other = (Tile)link.Target;
                variation += other.Wind;
            }
            variation /= Neighbors.Count;
            variation += VectorUtility.CreateVector(rng.NextDouble() * MathHelper.TwoPi, (float)(rng.NextDouble() * 0.5));
            WindUpdate = PrevailingWind + variation;
            WindUpdate *= 0.5f;
        }

        public void ApplyUpdates()
        {
            Wind = WindUpdate;
        }

        public void InitializeNeighbors()
        {
            int minX = X - 1;
            int minY = Math.Max(0, Y - 1);
            int maxX = X + 1;
            int maxY = Math.Min(Surface.SizeY - 1, Y + 1);

            for (int x = minX; x <= maxX; x++)
            for (int y = minY; y <= maxY; y++)
            {
                int x2 = MathExtra.Wrap(x, Surface.SizeX);
                Tile tile = Surface.TileGrid[x2, y];

                if (tile != this)
                {
                    bool diagonal = (Math.Abs(X - x) + Math.Abs(Y - y) == 2);
                    int cost = diagonal ? 1500 : 1000;
                    Neighbors.Add(new SimpleLink(tile, cost));
                }
            }
        }

        public override double CalculateHeuristic(PathfinderNode end)
        {
            return 1200 * Vector2.Distance(PositionVector, ((Tile)end).PositionVector);
        }

        public override IEnumerable<PathfinderLink> GetNeighbors()
        {
            return Neighbors;
        }
    }
}
