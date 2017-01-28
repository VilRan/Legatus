using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus.Input;
using Legatus;

namespace TestProject.Testing
{
    internal class ScriptTest : Test, IInputReceiver
    {
        private InputReceiverHandler Input;

        public ScriptTest(LegatusGame game)
        {
            Input = new InputReceiverHandler(this);
        }

        public override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public void OnKeyPressed(Keys pressedKey, Keys[] allHeldKeys)
        {

        }

        public void OnKeyRepeated(Keys repeatedKey, Keys[] allHeldKeys)
        {

        }

        public void OnKeyHeld(Keys heldKey, Keys[] allHeldKeys)
        {

        }

        public void OnKeyReleased(Keys releasedKey, Keys[] allHeldKeys)
        {

        }

        public void OnMouseOver(Point mousePosition, Keys[] allHeldKeys)
        {

        }

        public void OnMouseScroll(Point mousePosition, int delta, Keys[] allHeldKeys)
        {

        }

        public void OnMousePressed(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }

        public void OnMouseReleased(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }

        public void OnMouseDoubleClick(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }

        public void OnMouseDrag(MouseButton button, Point mousePosition, Point previousPosition, Point startPosition, Keys[] allHeldKeys)
        {

        }
    }
}
