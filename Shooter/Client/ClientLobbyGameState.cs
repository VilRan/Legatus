using Legatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.Net;
using Legatus.UI;

namespace Shooter
{
    public class ClientLobbyGameState : GameState
    {
        private ShooterGame Game;
        private List<ServerInfo> KnownServers = new List<ServerInfo>();
        private UIText ServerList;
        private UIText PlayerName;
        private NetClient Client;

        public ClientLobbyGameState(ShooterGame game)
            : base(game)
        {
            Game = game;
            UIButtonDynamic refreshButton = new UIButtonDynamic(100, 100, 200, 20, Refresh, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Refresh");
            UIButtonDynamic connectButton = new UIButtonDynamic(100, 130, 200, 20, Connect, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Connect");
            UIButtonDynamic backButton = new UIButtonDynamic(100, 160, 200, 20, Back, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Back");
            UIText playerNameLabel = new UITextBox(350, 100, 200, 20, game.Data.DefaultFont, Color.White, "Player Name: ");
            PlayerName = new UITextBox(550, 100, 1000, 20, game.Data.DefaultFont, Color.White, "Test Player", 32);
            PlayerName.TakesFocusOnMouseAction = true;
            ServerList = new UIText(350, 130, 1000, 1000, game.Data.DefaultFont, Color.White);
            ServerList.String = "Server list: \n";
            UI.Add(refreshButton, connectButton, backButton, playerNameLabel, PlayerName, ServerList);
        }

        public override void OnStart()
        {
            base.OnStart();

            if (Game.IsOffline)
            {
                NetPeerConfiguration config = new NetPeerConfiguration(ShooterGame.NetIdentifier);
                config.Port = ShooterGame.NetPort;
                config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

                Game.NetPeer = Client = new NetClient(config);
                Client.Start();
                Client.DiscoverLocalPeers(ShooterGame.NetPort);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            NetIncomingMessage msg;
            while ((msg = Client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        ReadDiscoveryResponse(msg); break;
                    case NetIncomingMessageType.StatusChanged:
                        ReadStatusChange(msg); break;
                    case NetIncomingMessageType.Data:
                        ReadData(msg); break;
                }
                Client.Recycle(msg);
            }
        }

        private void ReadDiscoveryResponse(NetIncomingMessage msg)
        {
            ServerInfo server = new ServerInfo(msg.SenderEndPoint, msg.ReadString(), msg.ReadBoolean());
            if (!KnownServers.Contains(server))
            {
                KnownServers.Add(server);
                ServerList.String += server.Name;// + " " + server.EndPoint + '\n';
                if (server.RequiresPassword)
                    ServerList.String += " Requires Password";
                ServerList.String += '\n';
            }
        }

        private void ReadStatusChange(NetIncomingMessage msg)
        {
            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
            if (status == NetConnectionStatus.Connected)
                Game.ActiveState = new ClientReadyRoomGameState(Game);
        }

        private void ReadData(NetIncomingMessage msg)
        {
            MessageContentType contentType = (MessageContentType)msg.ReadByte();
            switch (contentType)
            {
                case MessageContentType.StartGame:
                    Game.ActiveState = new ClientBattleGameState(Game);
                    break;
            }
        }

        private void Refresh(object sender, EventArgs e)
        {
            KnownServers.Clear();
            ServerList.String = "Server list: \n";
            Client.DiscoverLocalPeers(ShooterGame.NetPort);
        }

        private void Connect(object sender, EventArgs e)
        {
            if (KnownServers.Count == 0)
                return;

            ServerInfo server = KnownServers[0];

            NetOutgoingMessage approval = Client.CreateMessage();
            if (server.RequiresPassword)
                approval.Write("Test Password");
            else
                approval.Write("");
            approval.Write(PlayerName.String);
            Client.Connect(server.EndPoint, approval);
        }

        private void Back(object sender, EventArgs e)
        {
            Game.ActiveState = Game.MainMenu;
            Client.Shutdown("Client has left the lobby");
        }
    }
}
