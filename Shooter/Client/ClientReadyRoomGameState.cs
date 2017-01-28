using Legatus;
using Legatus.UI;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    public class ClientReadyRoomGameState : GameState
    {
        private ShooterGame Game;
        private NetClient Client;

        public ClientReadyRoomGameState(ShooterGame game)
            : base(game)
        {
            Game = game;
            Client = (NetClient)game.NetPeer;
            UIButtonDynamic backButton = new UIButtonDynamic(100, 100, 200, 20, Back, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Back");
            UI.Add(backButton);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            NetIncomingMessage msg;
            while ((msg = Client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        ReadDataMessage(msg);
                        break;
                }
                Client.Recycle(msg);
            }
        }

        private void ReadDataMessage(NetIncomingMessage msg)
        {
            MessageContentType contentType = (MessageContentType)msg.ReadByte();
            switch (contentType)
            {
                case MessageContentType.StartGame:
                    Game.ActiveState = new ClientBattleGameState(Game);
                    break;
            }
        }

        private void Back(object sender, EventArgs e)
        {
            Game.ActiveState = Game.MainMenu;
            Client.Shutdown("Client has left the lobby");
        }
    }
}
