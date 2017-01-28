/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Graphics;

namespace TestProject.Testing
{
    internal class TAPerformanceTest : Test
    {
        private class TestObject
        {
            public TAPerformanceTest Test;
            public Texture2D Texture;
            public Rectangle? SourceRectangle;
            public Vector2 Position;
            public Vector2 Velocity;
            public float Angle;
            public float Rotation;

            public TestObject(TAPerformanceTest test, List<Texture2D> textures)
            {
                Texture = textures[test.RNG.Next(textures.Count)];
                SourceRectangle = null;
                Position = new Vector2(test.RNG.Next(test.Width), test.RNG.Next(test.Height));
                Velocity = new Vector2((float)(-test.RNG.NextDouble() + test.RNG.NextDouble()), (float)(-test.RNG.NextDouble() + test.RNG.NextDouble()));
                Rotation = (float)(-test.RNG.NextDouble() + test.RNG.NextDouble());
                Test = test;
            }

            public TestObject(TAPerformanceTest test, TextureAtlas atlas)
            {
                Texture = atlas.Texture;
                SourceRectangle = atlas.ReservedAreas[test.RNG.Next(atlas.ReservedAreas.Count)];
                Position = new Vector2(test.RNG.Next(test.Width), test.RNG.Next(test.Height));
                Velocity = new Vector2((float)(-test.RNG.NextDouble() + test.RNG.NextDouble()), (float)(-test.RNG.NextDouble() + test.RNG.NextDouble()));
                Rotation = (float)(-test.RNG.NextDouble() + test.RNG.NextDouble());
                Test = test;
            }

            public void Update(GameTime gameTime)
            {
                Angle += Rotation;
                Position += Velocity;

                if (Position.X < 0 || Position.X > Test.Width)
                    Velocity += new Vector2(-Velocity.X * 2, 0);
                if (Position.Y < 0 || Position.Y > Test.Height)
                    Velocity += new Vector2(0, -Velocity.Y * 2);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White, Angle, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        public Random RNG;
        public int Width, Height;
        public SpriteFont Font;
        public TextureAtlasManager TAManager;
        private List<TestObject> TestObjects;

        public TAPerformanceTest(Game game)
        {
            RNG = new Random();
            Width = game.Window.ClientBounds.Width;
            Height = game.Window.ClientBounds.Height;
            Font = game.Content.Load<SpriteFont>("Graphics/DefaultFont12");

            string[] filePathArray = { 
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" ,
                                     "Graphics/Button.png", "Graphics/Frame.png", "Graphics/WhiteTile.png" 
                                 };

            List<Texture2D> textureList = new List<Texture2D>();
            foreach (string filePath in filePathArray)
            {
                textureList.Add(game.Content.Load<Texture2D>(filePath));
            }

            string[] filePathArray2 = { 
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" ,
                                     "Content/Graphics/Button.png", "Content/Graphics/Frame.png", "Content/Graphics/WhiteTile.png" 
                                 };

            TAManager = new TextureAtlasManager(game);
            TAManager.AddFromFiles(game.GraphicsDevice, filePathArray2.ToList());
            TestObjects = new List<TestObject>();

            for (int i = 0; i < 10000; i++)
            {
                //TestObjects.Add(new TestObject(this, textureList));
                TestObjects.Add(new TestObject(this, TAManager.Atlases[0]));
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (TestObject testObject in TestObjects)
            {
                testObject.Update(gameTime);
            }
        }

        private int PreviousTime;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (TestObject testObject in TestObjects)
            {
                testObject.Draw(spriteBatch);
            }


            int time = DateTime.Now.Millisecond;
            spriteBatch.DrawString(Font, "ms per frame: " + (time - PreviousTime), new Vector2(1, 1), Color.Black);
            spriteBatch.DrawString(Font, "ms per frame: " + (time - PreviousTime), Vector2.Zero, Color.Turquoise);
            PreviousTime = time;
            spriteBatch.End();
        }
    }
}
*/