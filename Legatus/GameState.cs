using Legatus.Input;
using Legatus.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Legatus
{
    public class GameState : BasicGameState
    {
        public GameUI UI;
        private InputEventHandler Input;

        public GameState(LegatusGame game)
        {
            Input = new InputEventHandler();
            UI = new GameUI(game, Input);
        }
        
        public override void Update(GameTime gameTime)
        {
            UI.Update(gameTime);
            Input.Update(gameTime);
            UI.PostUpdate(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            UI.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Called when entering the game state.
        /// </summary>
        public override void OnStart()
        {

        }

        /// <summary>
        /// Called before the game changes to another state or exits.
        /// </summary>
        public override void OnExit()
        {

        }
    }
}
