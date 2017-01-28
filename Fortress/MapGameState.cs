using Legatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Legatus.UI;

namespace Fortress
{
    public class MapGameState : GameState
    {
        private FortressGame Game;

        public MapGameState(FortressGame game)
            : base(game)
        {
            Game = game;
            MapView view = new MapView(game.Session.Map);
            UIViewport viewport = new UIViewport(0, 0, game.Width, game.Height, view, game);
            UI.Add(viewport);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Game.Session.Map.Update(gameTime);
        }
    }
}
