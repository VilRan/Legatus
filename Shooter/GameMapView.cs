using Legatus.Input;
using Legatus.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Shooter
{
    public class GameMapView : IUIView
    {
        private BattleGameState State;
        
        public ShooterGame Game { get { return State.Game; } }
        public GameMap Map { get { return State.Map; } }
        public GameDataManager Data { get { return Game.Data; } }
        public Player Player
        {
            get
            {
                GameObject player = null;
                Map.Objects.TryGetValue(State.PlayerID, out player);
                return (Player)player;
            }
        }

        public GameMapView(BattleGameState state)
        {
            State = state;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds)
        {
            spriteBatch.Begin();
            foreach (GameObject obj in Map.Objects.Values)
            {
                obj.Draw(spriteBatch, Data);
            }
            spriteBatch.End();
        }

        public void OnKeyboardAction(KeyboardEventArgs keyboard)
        {
            if (Player == null)
                return;

            switch (keyboard.Key)
            {
                case Keys.W:
                    Player.IsMovingFront = keyboard.Action == KeyboardAction.Held || keyboard.Action == KeyboardAction.Pressed;
                    break;
                case Keys.A:
                    Player.IsMovingLeft = keyboard.Action == KeyboardAction.Held || keyboard.Action == KeyboardAction.Pressed;
                    break;
                case Keys.D:
                    Player.IsMovingRight = keyboard.Action == KeyboardAction.Held || keyboard.Action == KeyboardAction.Pressed;
                    break;
                case Keys.S:
                    Player.IsMovingBack = keyboard.Action == KeyboardAction.Held || keyboard.Action == KeyboardAction.Pressed;
                    break;
                case Keys.Escape:
                    State.Exit(this, EventArgs.Empty);
                    break;
            }
        }

        public void OnMouseAction(MouseActionEventArgs mouse)
        {
            if (Player == null || !Player.CanShoot)
                return;

            switch (mouse.Button)
            {
                case MouseButton.Left:
                    switch (mouse.Action)
                    {
                        case MouseAction.Released:
                            Vector2 velocity = Vector2.Normalize(mouse.Position.ToVector2() - Player.Position) * 1000f;
                            State.SpawnProjectile(Player.Position, velocity);
                            Player.ReloadTimer += 0.1;
                            break;
                    }
                    break;
            }
        }

        public void OnMouseDrag(MouseDragEventArgs mouse)
        {
            /*
            if (Player == null || !Player.CanShoot)
                return;

            switch (mouse.Button)
            {
                case MouseButton.Left:
                    Vector2 velocity = Vector2.Normalize(mouse.CurrentPosition.ToVector2() - Player.Position) * 1000f;
                    State.SpawnProjectile(Player.Position, velocity);
                    Player.ReloadTimer += 0.05;
                    break;
            }
            */
        }

        public void OnMouseOver(MouseOverEventArgs mouse)
        {
            if (Player == null)
                return;

            Vector2 delta = mouse.Position.ToVector2() - Player.Position;
            Player.Heading = (float)Math.Atan2(delta.Y, delta.X);

        }

        public void OnMouseScroll(MouseScrollEventArgs mouse)
        {

        }
    }
}
