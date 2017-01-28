using Legatus.Graphics;
using Legatus.Pathfinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Tile : PathfinderTile
    {
        public Terrain Terrain;
        public BasicSprite Sprite;
        public Creature Creature;
        public float DrawDepth;

        public Vector2 ScreenPosition { get { return new Vector2(X * 32 - 16, Y * 32 - 16); } }

        public Tile(GameMap map, ushort x, ushort y, Terrain terrain, Random random)
            : base(map, x, y)
        {
            Terrain = terrain;
            Sprite = terrain.SpriteGroup.GetVariant(random);
            DrawDepth = (float)random.NextDouble();
        }

        public override double GetCost()
        {
            return Terrain.Cost;
        }

        public override double CalculateHeuristic(PathfinderNode end)
        {
            double heuristic = base.CalculateHeuristic(end);
            if (Creature != null)
                heuristic += 10.0;

            return heuristic;
        }
    }
}
