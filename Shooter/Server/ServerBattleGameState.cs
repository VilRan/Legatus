using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Shooter
{
    class ServerBattleGameState : BattleGameState
    {
        private NetServer Server;

        public ServerBattleGameState(ShooterGame game)
            : base(game)
        {
            Server = (NetServer)game.NetPeer;
            PlayerID = Map.AddObject(new Player());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            NetIncomingMessage msgInc;
            while ((msgInc = Server.ReadMessage()) != null)
            {
                switch (msgInc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        ReadData(msgInc); break;
                }
                Server.Recycle(msgInc);
            }
            
            NetOutgoingMessage msgOut = Server.CreateMessage();
            msgOut.Write((byte)MessageContentType.MapData);
            msgOut.Write(Map.Objects.Count);
            foreach (GameObject obj in Map.Objects.Values)
            {
                msgOut.Write(obj.ID);
                msgOut.Write((byte)obj.Type);
                obj.WriteTo(msgOut);
            }
            Server.SendToAll(msgOut, NetDeliveryMethod.UnreliableSequenced);
        }

        private void ReadData(NetIncomingMessage msgInc)
        {
            int id;
            MessageContentType contentType = (MessageContentType)msgInc.ReadByte();
            switch (contentType)
            {
                case MessageContentType.RequestSpawn:
                    Player newPlayer = new Player();
                    id = Map.AddObject(newPlayer);
                    NetOutgoingMessage msgOut = Server.CreateMessage();
                    msgOut.Write((byte)MessageContentType.ApproveSpawn);
                    msgOut.Write(id);
                    Server.SendMessage(msgOut, msgInc.SenderConnection, NetDeliveryMethod.ReliableUnordered);
                    break;

                case MessageContentType.MapData:
                    GameObject obj;
                    id = msgInc.ReadInt32();
                    if (Map.Objects.TryGetValue(id, out obj))
                    {
                        msgInc.ReadByte(); // Ignore GameObjectType
                        obj.ReadFrom(msgInc);
                    }
                    break;

                case MessageContentType.RequestProjectile:
                    Vector2 position = new Vector2(msgInc.ReadFloat(), msgInc.ReadFloat());
                    Vector2 velocity = new Vector2(msgInc.ReadFloat(), msgInc.ReadFloat());
                    SpawnProjectile(position, velocity);
                    break;
            }
        }

        public override void Exit(object sender, EventArgs e)
        {
            base.Exit(sender, e);
            Server.UPnP.DeleteForwardingRule(ShooterGame.NetPort);
            Server.Shutdown("Host has left the game.");
        }

        public override void SpawnProjectile(Vector2 position, Vector2 velocity)
        {
            Projectile proj = new Projectile();
            proj.Position = position;
            proj.Velocity = velocity;
            Map.AddObject(proj);
        }
    }
}
