using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike
{
    public class GameData
    {
        public Dictionary<string, FloorSprite> FloorSprites = new Dictionary<string, FloorSprite>();
        public Dictionary<string, WallSprite> WallSprites = new Dictionary<string, WallSprite>();
    }
}
