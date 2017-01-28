using Legatus;
using Legatus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Shooter
{
    public abstract class BattleGameState : GameState
    {
        public ShooterGame Game;
        public GameMap Map;
        public long PlayerID;
        protected GameMapView MapView;

        public Player Player { get { return MapView.Player; } }

        public BattleGameState(ShooterGame game)
            : base(game)
        {
            Game = game;
            Map = new GameMap();
            MapView = new GameMapView(this);
            UIViewport mapViewport = new UIViewport(0, 0, UI.Width, UI.Height, MapView, game);
            mapViewport.IsAlwaysAtBottom = true;

            UI.Add(mapViewport);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Map.Update(gameTime);
        }
        
        public virtual void Exit(object sender, EventArgs e)
        {
            Game.ActiveState = new MainMenuGameState(Game);
        }

        public abstract void SpawnProjectile(Vector2 position, Vector2 velocity);
    }
}
