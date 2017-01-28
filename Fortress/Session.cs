using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Session
    {
        public GameMap Map;

        public Session(FortressGame game)
        {
            Random random = new Random();
            Map = new GameMap(100, 100, random, game);
        }
    }
}
