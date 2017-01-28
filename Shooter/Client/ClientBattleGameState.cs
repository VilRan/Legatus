using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Shooter
{
    public class ClientBattleGameState : BattleGameState
    {
        private NetClient Client;

        public ClientBattleGameState(ShooterGame game)
            : base (game)
        {
            Client = (NetClient)game.NetPeer;
        }

        public override void OnStart()
        {
            NetOutgoingMessage msg = Client.CreateMessage();
            msg.Write((byte)MessageContentType.RequestSpawn);
            Client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (Client.ConnectionStatus != NetConnectionStatus.Connected)
            {
                Exit(this, EventArgs.Empty);
                return;
            }

            if (Player != null)
            {
                NetOutgoingMessage msgOut = Client.CreateMessage();
                msgOut.Write((byte)MessageContentType.MapData);
                msgOut.Write(Player.ID);
                msgOut.Write((byte)Player.Type);
                MapView.Player.WriteTo(msgOut);
                Client.SendMessage(msgOut, Client.ServerConnection, NetDeliveryMethod.UnreliableSequenced);
            }

            NetIncomingMessage msgInc;
            while ((msgInc = Client.ReadMessage()) != null)
            {
                switch (msgInc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        MessageContentType contentType = (MessageContentType)msgInc.ReadByte();
                        switch (contentType)
                        {
                            case MessageContentType.ApproveSpawn:
                                PlayerID = msgInc.ReadInt32();
                                break;

                            case MessageContentType.MapData:
                                int n = msgInc.ReadInt32();
                                for (int i = 0; i < n; i++)
                                {
                                    GameObject obj;
                                    int id = msgInc.ReadInt32();
                                    if (Map.Objects.TryGetValue(id, out obj))
                                    {
                                        msgInc.ReadByte(); // Ignore GameObjectType
                                        if (id != PlayerID)
                                            obj.ReadFrom(msgInc);
                                        else
                                            obj.FakeReadFrom(msgInc);
                                    }
                                    else
                                    {
                                        GameObjectType type = (GameObjectType)msgInc.ReadByte();
                                        switch (type)
                                        {
                                            case GameObjectType.Player:
                                                obj = new Player();
                                                break;
                                            case GameObjectType.Projectile:
                                                obj = new Projectile();
                                                break;
                                        }
                                        obj.ReadFrom(msgInc);
                                        Map.AddObject(obj, id);
                                    }
                                }
                                break;
                        }
                        break;
                }
                Client.Recycle(msgInc);
            }
        }

        public override void Exit(object sender, EventArgs e)
        {
            base.Exit(sender, e);
            Client.Shutdown("Client has left the game.");
        }

        public override void SpawnProjectile(Vector2 position, Vector2 velocity)
        {
            NetOutgoingMessage msgOut = Client.CreateMessage();
            msgOut.Write((byte)MessageContentType.RequestProjectile);
            msgOut.Write(position.X);
            msgOut.Write(position.Y);
            msgOut.Write(velocity.X);
            msgOut.Write(velocity.Y);
            Client.SendMessage(msgOut, NetDeliveryMethod.ReliableUnordered);
        }
    }
}
