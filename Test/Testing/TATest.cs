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
    internal class TATest : Test
    {
        //public TextureAtlas TextureAtlas;
        //public List<TextureAtlas> TextureAtlases;
        public TextureAtlasManager TAManager;

        public TATest(Game game)
        {
            string[] filePathArray = { 
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
            /*
            List<string> filePaths = filePathArray.ToList();
            Console.WriteLine("Total files: " + filePaths.Count);
            TextureAtlas = new TextureAtlas(game.GraphicsDevice, 256);
            TextureAtlas.AddFromFiles(game.GraphicsDevice, filePaths);
            Console.WriteLine("Skipped files: " + filePaths.Count);

            List<string> filePaths = filePathArray.ToList();
            TAManager = new TextureAtlasManager(game);
            TAManager.AddFromFiles(game.GraphicsDevice, filePaths);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(TextureAtlas.Texture, Vector2.Zero, Color.White);
            TAManager.TestDraw(spriteBatch);
            spriteBatch.End();
        }

        public override void Dispose()
        {
            base.Dispose();

            //TextureAtlas.Dispose();
            TAManager.Dispose();
        }
    }
}
*/