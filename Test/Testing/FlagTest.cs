using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus.Graphics;
using Legatus.Input;
using Legatus;

namespace TestProject.Testing
{
    internal class FlagTest : Test, IInputReceiver
    {
        private Game Game;
        private InputReceiverHandler Input;
        private List<Texture2D> Flags;
        private int Width, Height;

        public FlagTest(LegatusGame game)
        {
            Game = game;
            Input = new InputReceiverHandler(this);
            Width = game.Window.ClientBounds.Width;
            Height = game.Window.ClientBounds.Height;

            Flags = new List<Texture2D>();
            GenerateFlags();
        }

        private void GenerateFlags()
        {
            foreach (Texture2D flag in Flags)
            {
                flag.Dispose();
            }
            
            Flags.Clear();
            FlagGenerator flagGenerator = new FlagGenerator();
            Random rng = new Random();

            for (int i = 0; i < 100; i++)
            {
                Flags.Add(flagGenerator.GenerateRandom(Game.GraphicsDevice, 120, 80, rng));
            }
        }

        public override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int margin = 20;
            Vector2 position = Vector2.Zero;
            spriteBatch.Begin();
            foreach (Texture2D flag in Flags)
            {
                spriteBatch.Draw(flag, position, Color.White);
                position += new Vector2(flag.Width + margin, 0);
                if (position.X > Width - flag.Width)
                    position = new Vector2(0, position.Y + flag.Height + margin);
            }
            spriteBatch.End();
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (Texture2D flag in Flags)
            {
                flag.Dispose();
            }
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
            if (releasedKey == Keys.R)
            {
                GenerateFlags();
            }
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
