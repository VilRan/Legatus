using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Empire.Planetary
{
    public class Cloud
    {
        public Vector2 Position;
        public int Volume;
        //private bool Raining;

        public int X { get { return (int)Position.X; } }
        public int Y { get { return (int)Position.Y; } }

        public void Update(Tile tile)
        {
            Position += tile.Wind;

            //if (Raining)
            {
                tile.Moisture += Volume / 2;
                Volume /= 2;
            }
        }
    }
}
