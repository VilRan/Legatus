using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Shooter
{
    class GameServer : NetServer
    {
        public Dictionary<NetConnection, PlayerInfo> PlayerInfos = new Dictionary<NetConnection, PlayerInfo>();

        public GameServer(NetPeerConfiguration config)
            : base (config)
        {

        }
    }
}
