using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Battle
{
    public class BattleMap
    {
        public readonly int SizeX, SizeY;

        public BattleMap(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
        }
    }
}
