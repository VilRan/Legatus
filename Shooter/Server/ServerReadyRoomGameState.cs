using Legatus;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Legatus.UI;

namespace Shooter
{
    public class ServerReadyRoomGameState : GameState
    {
        private ShooterGame Game;
        private GameServer Server;
        private UIText Connections;
        private string ServerName;
        private string ServerPassword;

        public ServerReadyRoomGameState(ShooterGame game, string serverName, string serverPassword)
            : base(game)
        {
            Game = game;
            ServerName = serverName;
            ServerPassword = serverPassword;
            UIButtonDynamic startButton = new UIButtonDynamic(100, 100, 200, 20, Start, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Start");
            UIButtonDynamic backButton = new UIButtonDynamic(100, 130, 200, 20, Back, game.Data.DefaultButtonFrame, game.Data.DefaultFont, "Back");
            Connections = new UIText(350, 100, 1000, 1000, game.Data.DefaultFont, Color.White, "Connections: 0");
            UI.Add(startButton, backButton, Connections);
        }

        public override void OnStart()
        {
            base.OnStart();

            if (Game.IsOffline)
            {
                NetPeerConfiguration config = new NetPeerConfiguration(ShooterGame.NetIdentifier);
                config.Port = ShooterGame.NetPort;
                config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
                config.EnableUPnP = true;

                Game.NetPeer = Server = new GameServer(config);
                Server.Start();
                Server.UPnP.ForwardPort(ShooterGame.NetPort, "SJ" + ServerName);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            NetIncomingMessage msg;
            while ((msg = Server.ReadMessage()) != null)
            {
                ReadMessage(msg);
                Server.Recycle(msg);
            }
            
            Connections.String = "Connections: " + Server.ConnectionsCount;
            foreach (NetConnection connection in Server.Connections)
            {
                Connections.String += '\n' + connection.RemoteEndPoint.Address.ToString();
                Connections.String += " " + Math.Round(connection.AverageRoundtripTime * 1000) + " ms, ";
                Connections.String += Server.PlayerInfos[connection].Name;
            }
            
        }
        
        private void ReadMessage(NetIncomingMessage msg)
        {
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.NatIntroductionSuccess:
                    Console.WriteLine("NatIntroductionSuccess" + msg.ToString());
                    break;
                case NetIncomingMessageType.DiscoveryRequest:
                    NetOutgoingMessage response = Server.CreateMessage();
                    response.Write(ServerName);
                    response.Write(ServerPassword != "");
                    Server.SendDiscoveryResponse(response, msg.SenderEndPoint);
                    break;
                case NetIncomingMessageType.ConnectionApproval:
                    string password = msg.ReadString();
                    if (password == ServerPassword)
                    {
                        msg.SenderConnection.Approve();
                        PlayerInfo info = new PlayerInfo();
                        info.Name = msg.ReadString();
                        Server.PlayerInfos.Add(msg.SenderConnection, info);
                    }
                    else
                    {
                        msg.SenderConnection.Deny();
                    }
                    break;
                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                    if (status == NetConnectionStatus.Disconnected)
                    {
                        Server.PlayerInfos.Remove(msg.SenderConnection);
                    }
                    Console.WriteLine(status);
                    break;
                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.ErrorMessage:
                    Console.WriteLine(msg.ReadString());
                    break;
                default:
                    Console.WriteLine("Unhandled type: " + msg.MessageType);
                    break;
            }
        }

        private void Start(object sender, EventArgs e)
        {
            NetOutgoingMessage msg = Server.CreateMessage();
            msg.Write((byte)MessageContentType.StartGame);
            Server.SendToAll(msg, NetDeliveryMethod.ReliableUnordered);
            Game.ActiveState = new ServerBattleGameState(Game);
        }

        private void Back(object sender, EventArgs e)
        {
            Game.ActiveState = new ServerCreationGameState(Game);
            Server.UPnP.DeleteForwardingRule(ShooterGame.NetPort);
            Server.Shutdown("Host has left the game.");
        }
    }
}
