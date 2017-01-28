using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus;
using Legatus.Graphics;
using Legatus.UI;
using Empire.Planetary;

namespace Empire
{
    public class EmpireGame : LegatusGame
    {
        public GameData Data { private set; get; }
        public Random RNG { private set; get; }
        public TextureAtlasManager Textures;

        new GameState ActiveState { get { return (GameState)base.ActiveState; } }

        protected override void Initialize()
        {
            base.Initialize();
            Data = new GameData();
            RNG = new Random();
            Textures = new TextureAtlasManager(this);
            LoadBiomes();

            PlanetSurface surface = new PlanetSurface(200, 100, Data, RNG);
            PlanetSurfaceView view = new PlanetSurfaceView(this, surface);
            UIViewport viewport = new UIViewport(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height, view, this);
            ActiveState.UI.Add(viewport);
        }

        private void LoadBiomes()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("Content/XML/Biomes.xml");
            foreach (XmlNode node in xml.SelectNodes("Biomes/Biome"))
            {
                string uid = node.Attributes.GetNamedItem("UID").Value;
                string file = node.Attributes.GetNamedItem("File").Value;
                string name = node.Attributes.GetNamedItem("Name").Value;
                try
                {
                    using (Texture2D texture = TextureUtility.LoadTexture(GraphicsDevice, "Content/Graphics/Biomes/" + file))
                    {
                        Texture2D[,] variants = TextureUtility.Split(GraphicsDevice, texture, 64, 64);
                        BasicSprite[] sprites = new BasicSprite[variants.Length];
                        int i = 0, maxY = variants.GetLength(1), maxX = variants.GetLength(0);
                        for (int y = 0; y < maxY; y++)
                        for (int x = 0; x < maxX; x++)
                        {
                            sprites[i] = Textures.Copy(variants[x, y]);
                            i++;
                        }
                        Biome biome = new Biome(uid, name, sprites);
                        Data.Biomes.Add(uid, biome);
                    }
                }
                catch
                {

                }
            }
            Textures.ApplyChanges();
        }
    }
}
