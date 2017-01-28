using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legatus
{
    public abstract class BasicGameState
    {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Called when entering the game state.
        /// </summary>
        public abstract void OnStart();

        /// <summary>
        /// Called before the game changes to another state or exits.
        /// </summary>
        public abstract void OnExit();
    }
}
