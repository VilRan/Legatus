using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Legatus;
using Legatus.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    public class MainMenuGameState : GameState
    {
        private ShooterGame Game;

        public MainMenuGameState(ShooterGame game)
            : base(game)
        {
            Game = game;
            UIButtonDynamic joinButton = new UIButtonDynamic(100, 100, 200, 20, JoinGame, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Join Game");
            UIButtonDynamic hostButton = new UIButtonDynamic(100, 130, 200, 20, HostGame, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Host Game");
            UIButtonDynamic exitButton = new UIButtonDynamic(100, 160, 200, 20, game.Exit, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Exit");
            UI.Add(joinButton, hostButton, exitButton);
        }

        public override void OnStart()
        {
            base.OnStart();
            if ( ! Game.IsOffline)
                Game.NetPeer.Shutdown("");
        }

        private void JoinGame(object sender, EventArgs e)
        {
            Game.ActiveState = new ClientLobbyGameState(Game);
        }

        private void HostGame(object sender, EventArgs e)
        {
            Game.ActiveState = new ServerCreationGameState(Game);
        }
    }
}
