using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Empire.Planetary
{
    public class PlanetSurfaceCamera
    {
        private const int TileSize = 32;

        private PlanetSurfaceView View;
        private Point Position = Point.Zero;

        public PlanetSurface Surface { get { return View.Surface; } }
        public Vector2 PositionVector { get { return Position.ToVector2(); } }
        public Point PositionPoint { get { return Position; } }
        public int X { get { return Position.X; } }
        public int Y { get { return Position.Y; } }

        public PlanetSurfaceCamera(PlanetSurfaceView view)
        {
            View = view;
        }

        public void MoveTo(Point position)
        {
            Position = position;

            if (Position.X < 0)
                Position.X += Surface.SizeX * TileSize;
            else if (Position.X > Surface.SizeX * TileSize)
                Position.X -= Surface.SizeX * TileSize;
        }

        public void MoveBy(Point delta)
        {
            MoveTo(Position + delta);
        }
    }
}
