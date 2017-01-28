using Legatus;
using Legatus.UI;
using Microsoft.Xna.Framework;
using System;

namespace Shooter
{
    public class ServerCreationGameState : GameState
    {
        private UIText ServerName;
        private UIText ServerPassword;
        private ShooterGame Game;

        public ServerCreationGameState(ShooterGame game)
            : base(game)
        {
            Game = game;
            UIText serverNameLabel = new UIText(350, 100, 200, 20, game.Data.DefaultFont, Color.White, "Server Name: ");
            ServerName = new UITextBox(550, 100, 800, 20, game.Data.DefaultFont, Color.White, "Test Server", 64);
            ServerName.TakesFocusOnMouseAction = true;
            UIText serverPasswordLabel  = new UIText(350, 130, 200, 20, game.Data.DefaultFont, Color.White, "Server Password: ");
            ServerPassword = new UITextBox(550, 130, 800, 20, game.Data.DefaultFont, Color.White, "", 64);
            ServerPassword.IsPassword = true;
            ServerPassword.TakesFocusOnMouseAction = true;
            UIButtonDynamic createButton = new UIButtonDynamic(100, 100, 200, 20, CreateServer, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Create Game");
            UIButtonDynamic backButton = new UIButtonDynamic(100, 130, 200, 20, Back, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Back");
            UI.Add(serverNameLabel, ServerName, serverPasswordLabel, ServerPassword, createButton, backButton);
        }

        public override void OnStart()
        {
            base.OnStart();
            if (!Game.IsOffline)
                Game.NetPeer.Shutdown("");
        }

        private void CreateServer(object sender, EventArgs e)
        {
            Game.ActiveState = new ServerReadyRoomGameState(Game, ServerName.String, ServerPassword.String);
        }

        private void Back(object sender, EventArgs e)
        {
            Game.ActiveState = Game.MainMenu;
        }
    }
}
