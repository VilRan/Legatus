using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Legatus;

namespace Roguelike.Dungeons
{
    public class Dungeon
    {
        public DungeonRoom[,] Rooms;

        public int Width { get { return Rooms.GetLength(0); } }
        public int Height { get { return Rooms.GetLength(1); } }
        
        public Dungeon(Random rng)
        {
            Rooms = new DungeonRoom[rng.Next(4, 33), rng.Next(4, 33)];
            int x, y;
            for (x = 0; x < Width; x++)
                for (y = 0; y < Height; y++)
                    Rooms[x, y] = new DungeonRoom(x, y);

            List<DungeonRoom> open = new List<DungeonRoom>();

            x = rng.Next(Width);
            y = rng.Next(Height);
            DungeonRoom entrance = Rooms[x, y];
            entrance.Randomize(rng, this);
            entrance.IsEntrance = true;
            entrance.IsClosed = true;
            open.AddRange(GetOpenNeighbors(entrance));

            while (open.Count > 0)
            {
                DungeonRoom room = open[rng.Next(open.Count)];
                room.Randomize(rng, this);
                open.AddRange(GetOpenNeighbors(room));
                open.Remove(room);
                room.IsClosed = true;
            }
        }
        
        private IEnumerable<DungeonRoom> GetOpenNeighbors(DungeonRoom room)
        {
            int x = room.X, y = room.Y;
            if (room.NorthIsOpen && !Rooms[x, y - 1].IsClosed)
                yield return Rooms[x, y - 1];
            if (room.WestIsOpen && !Rooms[x - 1, y].IsClosed)
                yield return Rooms[x - 1, y];
            if (room.SouthIsOpen && !Rooms[x, y + 1].IsClosed)
                yield return Rooms[x, y + 1];
            if (room.EastIsOpen && !Rooms[x + 1, y].IsClosed)
                yield return Rooms[x + 1, y];
        }
    }
}
