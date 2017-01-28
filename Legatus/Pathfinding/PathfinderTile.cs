using System;
using Legatus.Mathematics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Legatus.Pathfinding
{
    /// <summary>
    /// A generic pathfinding node for a tile-based map.
    /// </summary>
    public abstract class PathfinderTile : PathfinderNode
    {
        public List<PathfinderLink> Neighbors = new List<PathfinderLink>();
        public readonly IPathfinderTileMap Map;
        public readonly ushort X, Y;
        public Vector2 VectorPosition { get { return new Vector2(X, Y); } }

        public PathfinderTile(IPathfinderTileMap map, ushort x, ushort y)
        {
            Map = map;
            X = x;
            Y = y;
        }

        public void InitializeNeighbors()
        {
            /*
            int minX = Map.WrapsX? X - 1 : Math.Max(0, X - 1);
            int minY = Map.WrapsY? Y - 1 : Math.Max(0, Y - 1);
            int maxX = Map.WrapsX? X + 1 : Math.Min(Map.SizeX - 1, X + 1);
            int maxY = Map.WrapsY? Y + 1 : Math.Min(Map.SizeY - 1, Y + 1);
            
            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++)
                {
                    int x2 = Map.WrapsX? MathExtra.Wrap(x, Map.SizeX) : x;
                    int y2 = Map.WrapsY? MathExtra.Wrap(y, Map.SizeY) : y;
                    PathfinderTile tile = Map.TileGrid[x2, y2];

                    if (tile != this)
                    {
                        double cost = tile.GetCost();
                        bool isDiagonal = (Math.Abs(X - x) + Math.Abs(Y - y) == 2);
                        if (isDiagonal)
                            cost *= Math.Sqrt(2);
                        Neighbors.Add(new SimpleLink(tile, cost));
                    }
                }
                */
        }

        public override IEnumerable<PathfinderLink> GetNeighbors()
        {
            //return Neighbors;
            int minX = Map.WrapsX ? X - 1 : Math.Max(0, X - 1);
            int minY = Map.WrapsY ? Y - 1 : Math.Max(0, Y - 1);
            int maxX = Map.WrapsX ? X + 1 : Math.Min(Map.SizeX - 1, X + 1);
            int maxY = Map.WrapsY ? Y + 1 : Math.Min(Map.SizeY - 1, Y + 1);

            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++)
                {
                    int x2 = Map.WrapsX ? MathExtra.Wrap(x, Map.SizeX) : x;
                    int y2 = Map.WrapsY ? MathExtra.Wrap(y, Map.SizeY) : y;
                    PathfinderTile tile = Map.TileGrid[x2, y2];

                    if (tile != this)
                    {
                        double cost = tile.GetCost();
                        bool isDiagonal = (Math.Abs(X - x) + Math.Abs(Y - y) == 2);
                        if (isDiagonal)
                            cost *= Math.Sqrt(2);
                        //Neighbors.Add(new SimpleLink(tile, cost));
                        yield return new SimpleLink(tile, cost);
                    }
                }
        }

        public override double CalculateHeuristic(PathfinderNode end)
        {
            PathfinderTile other = (PathfinderTile)end;
            int ox = other.X, 
                oy = other.Y;

            // Check if it's faster to take a shortcut over the map's edge.
            if (Map.WrapsX)
            {
                int dx = ox - X;
                if (dx > Map.SizeX / 2)
                    ox -= Map.SizeX;
                else if (dx < -Map.SizeX / 2)
                    ox += Map.SizeX;
            }
            if (Map.WrapsY)
            {
                int dy = oy - Y;
                if (dy > Map.SizeY / 2)
                    oy -= Map.SizeY;
                else if (dy < -Map.SizeY / 2)
                    dy += Map.SizeY;
            }

            return 1.2 * Vector2.Distance( VectorPosition, new Vector2(ox, oy) );
            //return 1.2 * Vector2.DistanceSquared( VectorPosition, new Vector2(ox, oy) );
        }

        public abstract double GetCost();
    }
}
