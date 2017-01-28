using Legatus.Mathematics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legatus.MapGeneration
{
    public class ProceduralPlanetOld
    {
        public double[,] Altitude;
        public double BaseHeight, AltitudeScale, Roughness, Noise;
        public int SizeX, SizeY;

        public ProceduralPlanetOld(int sizeX, int sizeY, double baseHeight, double altitudeScale, double roughness, double noise)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            AltitudeScale = altitudeScale;
            Roughness = roughness;
            Noise = noise;
        }

        public void Generate(Random rng)
        {
            ProceduralHeightmap baseHeightmap = new ProceduralHeightmap(SizeX, SizeY, BaseHeight, AltitudeScale, Roughness, Noise, true, false, false);
            baseHeightmap.Generate(rng);
            Altitude = baseHeightmap.Altitude;

            int n = rng.Next(6, 12);
            List<Continent> continents = new List<Continent>(n);
            while (n-- > 0)
            {
                int x = rng.Next(0, SizeX);
                int y = rng.Next(0, SizeY);
                Vector2 position = new Vector2(x, y);
                continents.Add(new Continent(position, Altitude[x, y]));
            }

            Continent[,] continentGrid = new Continent[SizeX, SizeY];
            for (int x = 0; x < SizeX; x++)
                for (int y = 0; y < SizeY; y++)
                    foreach (Continent continent in continents)
                    {
                        if (continentGrid[x, y] == null)
                            continentGrid[x, y] = continent;
                        else
                        {
                            Continent old = continentGrid[x, y];
                            Vector2 position = new Vector2(x, y);

                            float altitudeModifier = (float)Math.Abs(old.Altitude - Altitude[x, y]) / (float)AltitudeScale;
                            float distanceOld = VectorUtility.WrappingDistance(old.Position, position, SizeX);
                            distanceOld *= 2 - altitudeModifier;

                            altitudeModifier = (float)Math.Abs(continent.Altitude - Altitude[x, y]) / (float)AltitudeScale;
                            float distanceNew = VectorUtility.WrappingDistance(continent.Position, position, SizeX);
                            distanceNew *= 2 - altitudeModifier;

                            if (distanceNew < distanceOld)
                                continentGrid[x, y] = continent;
                        }
                    }

            for (int x = 0; x < SizeX; x++)
                for (int y = 0; y < SizeY; y++)
                {
                    Continent continent = continentGrid[x, y];
                    //Altitude[x, y] = continent.Altitude;
                    
                    int minX = x - 1;
                    int maxX = x + 1;
                    int minY = Math.Max(y - 1, 0);
                    int maxY = Math.Min(y + 1, SizeY - 1);
                    
                    for (int x2 = minX; x2 <= maxX; x2++)
                        for (int y2= minY; y2 <= maxY; y2++)
                        {
                            Continent other = continentGrid[MathExtra.Wrap(x2, SizeX), y2];
                            if (continent != other)
                            {
                                //Altitude[x2, y2] -= AltitudeScale / 8;
                                RaiseMountain(x, y, 10, rng);
                                x2 = int.MaxValue - 1; // Breaks the outer loop.
                                break;
                            }
                        }
                        
                }

        }

        private void CarveTrench(int x, int y, int radius)
        {
            int minX = x - radius;
            int maxX = x + radius;
            int minY = Math.Max(y - radius, 0);
            int maxY = Math.Min(y + radius, SizeY - 1);
            int radiusSquared = radius * radius;

            for (int x2 = minX; x2 <= maxX; x2++)
                for (int y2 = minY; y2 <= maxY; y2++)
                {
                    int dx = x2 - x;
                    int dy = y2 - y;
                    int distanceSquared = dx * dx + dy * dy;
                    if (distanceSquared < radiusSquared)
                    {
                        Altitude[MathExtra.Wrap(x2, SizeX), y2] -= AltitudeScale / 64;
                    } 
                }
        }

        private void RaiseMountain(int x, int y, int radius, Random rng)
        {
            int minX = x - radius;
            int maxX = x + radius;
            int minY = Math.Max(y - radius, 0);
            int maxY = Math.Min(y + radius, SizeY - 1);
            int radiusSquared = radius * radius;

            for (int x2 = minX; x2 <= maxX; x2++)
                for (int y2 = minY; y2 <= maxY; y2++)
                {
                    int dx = x2 - x;
                    int dy = y2 - y;
                    int distanceSquared = dx * dx + dy * dy;
                    if (distanceSquared < radiusSquared)
                    {
                        int delta = radiusSquared - distanceSquared;
                        double change = AltitudeScale / 4096 * delta * rng.NextDouble();
                        Altitude[MathExtra.Wrap(x2, SizeX), y2] += change;
                    }
                }
        }

        public double GetAltitude(int x, int y)
        {
            return Altitude[x, y];
        }

        private class Continent
        {
            public Vector2 Position;
            //public Vector2 Velocity;
            public double Altitude;

            public Continent(Vector2 position, double altitude)
            {
                Position = position;
                Altitude = altitude;
            }
        }
    }
}
