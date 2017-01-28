using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Legatus;
using Legatus.MapGeneration;
using Microsoft.Xna.Framework;

namespace Empire.Planetary
{
    public class PlanetSurface
    {
        public Tile[,] TileGrid;
        public List<Cloud> Clouds = new List<Cloud>();

        public int SizeX { get { return TileGrid.GetLength(0); } }
        public int SizeY { get { return TileGrid.GetLength(1); } }
        public int Area { get { return SizeX * SizeY; } }

        public PlanetSurface(int sizeX, int sizeY, GameData data, Random rng)
        {
            TileGrid = new Tile[sizeX, sizeY];
            ProceduralHeightmap map = new ProceduralHeightmap(sizeX, sizeY, -2000, 6000, 0.55, 0.0, true, false);
            map.Generate(rng);

            for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                int altitude = (int)map.Altitude[x, y];// +rng.Next(-500, 500);

                Tile tile = new Tile(this, x, y, altitude, data.Biomes["Grass"], rng);
                TileGrid[x, y] = tile;

                if (altitude > 0)
                {
                    if (tile.Temperature <= 0)
                        tile.SetBiome(data.Biomes["Snow"], rng);
                    else if (altitude > 2000)
                        tile.SetBiome(data.Biomes["Rock"], rng);
                    //else if (tile.Temperature >= 25)
                    //    tile.SetBiome(data.Biomes["Steppe"], rng);
                }
                else
                {
                    if (tile.Temperature >= -2)
                        tile.SetBiome(data.Biomes["Sea"], rng);
                    else
                        tile.SetBiome(data.Biomes["Ice"], rng);
                }
            }

            foreach (Tile tile in TileGrid)
            {
                tile.InitializeNeighbors();
            }
        }

        public void Update(Random rng)
        {
            foreach (Tile tile in TileGrid)
            {
                tile.CalculateUpdates(rng);
            }

            foreach (Tile tile in TileGrid)
            {
                tile.ApplyUpdates();
            }

            foreach (Cloud cloud in Clouds)
            {
                Tile tile = TileGrid[cloud.X, cloud.Y];
                cloud.Update(tile);
            }

        }
    }
}
