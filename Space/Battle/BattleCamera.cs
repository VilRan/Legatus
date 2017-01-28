using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space.Battle
{
    public class BattleCamera
    {
        private Vector2 Position = Vector2.Zero;
        
        public Vector2 PositionVector { get { return PositionPoint.ToVector2(); } }
        public Point PositionPoint { get { return Position.ToPoint(); } }
        public int X { get { return PositionPoint.X; } }
        public int Y { get { return PositionPoint.Y; } }

        public BattleCamera()
        {

        }
        
        public void MoveTo(Vector2 position)
        {
            Position = position;
        }
        
        public void MoveBy(Vector2 delta)
        {
            Position += delta;
        }
    }
}
