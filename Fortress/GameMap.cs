using Legatus.Collections;
using Legatus.MapGeneration;
using Legatus.Pathfinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class GameMap : IPathfinderTileMap
    {
        public FortressGame Game;
        public Tile[,] Tiles;
        public List<Creature> Creatures = new List<Creature>();

        public PathfinderTile[,] TileGrid { get { return Tiles; } }
        public Rectangle Bounds { get { return new Rectangle(0, 0, SizeX, SizeY); } }
        public int SizeX { get { return Tiles.GetLength(0); } }
        public int SizeY { get { return Tiles.GetLength(1); } }
        public bool WrapsX { get { return false; } }
        public bool WrapsY { get { return false; } }
        private ModManager Mods { get { return Game.Mods; } }

        public GameMap(int width, int height, Random random, FortressGame game)
        {
            Game = game;
            Regenerate(width, height, random);
        }

        public void Regenerate(int width, int height, Random random)
        {
            Creatures.Clear();

            ProceduralHeightmap heightmap = new ProceduralHeightmap(width, height, 0, 1, 0.65, 0.05, false, false, false);
            heightmap.Generate(random);

            Tiles = new Tile[width, height];
            for (ushort x = 0; x < width; x++)
                for (ushort y = 0; y < height; y++)
                {
                    Terrain terrain = Mods.Terrains["Grass"];
                    double altitude = heightmap.GetAltitude(x, y);
                    if (altitude < 0)
                        terrain = Mods.Terrains["Water"];
                    else if (altitude > 0.5)
                        terrain = Mods.Terrains["Rock"];
                    else if (altitude < 0.1 || random.NextDouble() < 0.25)
                        terrain = Mods.Terrains["Dirt"];
                    Tiles[x, y] = new Tile(this, x, y, terrain, random);

                    if (terrain != Mods.Terrains["Water"] && random.NextDouble() < 0.01)
                    {
                        Creatures.Add(new Creature(this, x, y, random));
                    }
                }

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Tiles[x, y].InitializeNeighbors();
                }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Creature creature in Creatures)
                creature.Update(gameTime);
        }
    }
}
