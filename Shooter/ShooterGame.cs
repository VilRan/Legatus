using System;
using Legatus;
using Lidgren.Network;

namespace Shooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ShooterGame : LegatusGame
    {
        public const string NetIdentifier = "ShooterGame";
        public const int NetPort = 14242;

        public GameDataManager Data;
        public MainMenuGameState MainMenu;
        public NetPeer NetPeer;

        public bool IsOffline { get { return NetPeer == null || NetPeer.Status == NetPeerStatus.NotRunning || NetPeer.Status == NetPeerStatus.ShutdownRequested; } }

        protected override void Initialize()
        {
            base.Initialize();
            Data = new GameDataManager(this);
            ActiveState = MainMenu = new MainMenuGameState(this);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            if (NetPeer != null)
                NetPeer.Shutdown("");
        }
    }
}
