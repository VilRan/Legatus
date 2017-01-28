using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Dungeons
{
    public class DungeonRoom
    {
        public int X, Y;
        public bool NorthIsOpen, WestIsOpen, SouthIsOpen, EastIsOpen;
        public bool IsEntrance;
        public bool IsClosed;

        public DungeonRoom(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Randomize(Random rng, Dungeon dungeon)
        {
            if (Y > 0)
                if (dungeon.Rooms[X, Y - 1].SouthIsOpen)
                    NorthIsOpen = true;
                else
                    NorthIsOpen = rng.Next(2) == 1;

            if (X > 0)
                if (dungeon.Rooms[X - 1, Y].EastIsOpen)
                    WestIsOpen = true;
                else
                    WestIsOpen = rng.Next(2) == 1;

            if (Y < dungeon.Height)
                if (dungeon.Rooms[X, Y + 1].NorthIsOpen)
                    SouthIsOpen = true;
                else
                    SouthIsOpen = rng.Next(2) == 1;

            if (X < dungeon.Width)
                if (dungeon.Rooms[X + 1, Y].WestIsOpen)
                    EastIsOpen = true;
                else
                    EastIsOpen = rng.Next(2) == 1;
        }
    }
}
