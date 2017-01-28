using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus.UI;
using TestProject.Testing;
using Legatus;
using Legatus.Mathematics;
using Legatus.Collections;
using System.Media;
using System.Diagnostics;

namespace TestProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TestGame : LegatusGame
    {
        private Test Test;

        public TestGame()
            : base()
        {
            /*
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = Graphics.SynchronizeWithVerticalRetrace;
            Window.IsBorderless = true;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
            */
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //UI = new GameUI(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            //Test = new TAPerformanceTest(this);
            //Test = new AITest(this, GraphicsDevice.DisplayMode.Width / 32, (GraphicsDevice.DisplayMode.Height - 32) / 32);
            //Test = new PathfinderTest(this, GraphicsDevice.DisplayMode.Width / PathfinderTest.TileSize, (GraphicsDevice.DisplayMode.Height - PathfinderTest.TileSize) / PathfinderTest.TileSize);
            //Test = new FlagTest(this);
            //Test = new HugeMapTest(this, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Width / 2);
            //Test = new IsometricTest(this, 40, 40, 20);
            //Test = new LOSTest(this, 40, 40);
            Test = new LanguageTest(this);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            Test.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            base.Update(gameTime);
            Test.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Test.Draw(gameTime, SpriteBatch);
        }
    }
}
