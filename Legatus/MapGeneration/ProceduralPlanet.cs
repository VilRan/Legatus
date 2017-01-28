using Legatus.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legatus.MapGeneration
{
    public class ProceduralPlanet
    {
        public double AltitudeScale = 1.0;
        public double Roughness = 0.65;
        public bool SmoothEdges = true;
        public bool WrapX = false;
        public bool WrapY = false;

        private double[,] Altitude;
        private double[,] Temperature;
        private double[,] Water;
        private double[,] Vapor;
        private int XSize, YSize;
        private bool DoReset;

        public ProceduralPlanet(int xSize, int ySize)
        {
            XSize = xSize;
            YSize = ySize;
            Altitude = new double[XSize, YSize];
            Temperature = new double[XSize, YSize];
            Water = new double[XSize, YSize];
            Vapor = new double[XSize, YSize];
            DoReset = false;
        }

        public void Generate(Random random)
        {
            if (DoReset)
            {
                Array.Clear(Altitude, 0, Altitude.Length);
                Array.Clear(Temperature, 0, Temperature.Length);
                Array.Clear(Water, 0, Water.Length);
                Array.Clear(Vapor, 0, Vapor.Length);
            }

            double[,] heightmap = new DiamondSquare(random, XSize, YSize, AltitudeScale, Roughness, WrapX, WrapY).GenerateHeightmap();
            double heightmapWidth = heightmap.GetUpperBound(0);
            double heightmapDepth = heightmap.GetUpperBound(1);
            double widthRatio = heightmapWidth / XSize;
            double heightRatio = heightmapDepth / YSize;
            int middleX = XSize / 2;
            int middleY = YSize / 2;
            int edgeSizeX = XSize / 8;
            int edgeSizeY = YSize / 8;
            int smoothThresholdX = middleX - edgeSizeX;
            int smoothThresholdY = middleY - edgeSizeY;

            for (int x = 0; x < XSize; x++)
                for (int y = 0; y < YSize; y++)
                {
                    double multiplier = 1.0;
                    if (SmoothEdges)
                    {
                        int dx = Math.Abs(middleX - x);
                        int dy = Math.Abs(middleY - y);
                        if (dx >= smoothThresholdX)
                            multiplier *= 1 - (dx - smoothThresholdX) / (double)(edgeSizeX);
                        if (dy >= smoothThresholdY)
                            multiplier *= 1 - (dy - smoothThresholdY) / (double)(edgeSizeY);
                    }

                    int x2 = (int)Math.Round((x) * widthRatio);
                    int y2 = (int)Math.Round((y) * heightRatio);
                    Altitude[x, y] = -AltitudeScale + (AltitudeScale + heightmap[x2, y2]) * multiplier;
                    Temperature[x, y] = 300;
                    Water[x, y] = 4000.0;
                    //Vapor[x, y] = 0;
                }


            DoReset = true;
        }

        private void SimulateOneRound()
        {

        }
    }
}
