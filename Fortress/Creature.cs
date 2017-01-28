using Legatus.Mathematics;
using Legatus.Pathfinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Creature
    {
        public readonly GameMap Map;
        public int X;
        public int Y;
        public double Speed;
        public double Initiative;

        public Vector2 ScreenPosition { get { return new Vector2(X * 32, Y * 32); } }
        public List<Tile> Path;
        public Tile Destination;

        public Creature(GameMap map, int x, int y, Random random)
        {
            Map = map;
            X = x;
            Y = y;
            do Speed = random.NextGaussian(5.0, 2.0);
            while (Speed <= 1.0);
            Initiative = random.NextDouble() / Speed;
        }

        public void Update(GameTime gameTime)
        {
            if (Initiative > 0)
            {
                Initiative -= gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            if (Path != null && Path.Count > 0 && (Path[0].Creature == null || Path[0].Creature == this))
            {
                Initiative += Path[0].Terrain.Cost / Speed;
                MoveTo(Path[0]);
                Path.RemoveAt(0);

                if (Path.Count == 0)
                {
                    Path = null;
                    Destination = null;
                }
            }
            else if (Destination != null)
            {
                Path = Pathfinder.FindPath(GetTile(), Destination).Cast<Tile>().ToList();
            }
        }

        public Tile GetTile()
        {
            return Map.Tiles[X, Y];
        }

        public void WalkTo(Tile tile)
        {
            Destination = tile;
            Path = null;
        }

        public void MoveTo(Tile tile)
        {
            MoveTo(tile.X, tile.Y);
        }

        public void MoveTo(int x, int y)
        {
            GetTile().Creature = null;
            X = x;
            Y = y;
            GetTile().Creature = this;
        }
    }
}
