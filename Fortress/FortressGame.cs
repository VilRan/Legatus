using Legatus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fortress
{
    public class FortressGame : LegatusGame
    {
        public ModManager Mods;
        public Session Session;

        public FortressGame()
        {
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            Mods = new ModManager(this);
            Session = new Session(this);
            ActiveState = new MapGameState(this);
        }
    }
}
