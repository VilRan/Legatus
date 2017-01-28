using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Legatus
{
    public class LegatusGame : Game
    {
        public event ResizeEventDelegate Resized;
        protected GraphicsDeviceManager Graphics;
        protected SpriteBatch SpriteBatch;
        private BasicGameState _ActiveState;

        public BasicGameState ActiveState
        {
            get { return _ActiveState; }
            set
            {
                _ActiveState.OnExit();
                _ActiveState = value;
                _ActiveState.OnStart();
            }
        }
        public Point Center { get { return new Point(Width / 2, Height / 2); } }
        public int Width { get { return Window.ClientBounds.Width; } }
        public int Height { get { return Window.ClientBounds.Height; } }

        public LegatusGame()
            : base()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = Graphics.SynchronizeWithVerticalRetrace;
            Window.IsBorderless = true;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        public void Resize(int width, int height)
        {
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();
            if (Resized != null)
                Resized(this, new ResizeEventArgs(width, height));
        }

        public void Exit(object sender, EventArgs e)
        {
            Exit();
        }

        protected override void Initialize()
        {
            _ActiveState = new GameState(this);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            _ActiveState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _ActiveState.Draw(gameTime, SpriteBatch);

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            _ActiveState.OnExit();
        }
    }

    public class ResizeEventArgs : EventArgs
    {
        public readonly int Width;
        public readonly int Height;

        public ResizeEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    public delegate void ResizeEventDelegate(object sender, ResizeEventArgs e);
}
