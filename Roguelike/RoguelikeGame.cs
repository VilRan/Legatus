using Legatus;
using Legatus.Graphics;
using Legatus.UI;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Battle;
using System;
using System.IO;
using System.Xml;

namespace Roguelike
{
    public class RoguelikeGame : LegatusGame
    {
        GameData Data = new GameData();

        new GameState ActiveState { get { return (GameState)base.ActiveState; } }

        public RoguelikeGame()
            : base()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            BattleMap testMap = new BattleMap(40, 40);
            BattleMapView view = new BattleMapView(testMap);
            UIViewport viewport = new UIViewport(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height, view, this);
            ActiveState.UI.Add(viewport);

            LoadData();
        }

        private void LoadData()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("Settings.xml");
            foreach (XmlNode node in xml.SelectNodes("Settings/Mods/Mod"))
            {
                LoadMod("Content/" + node.Attributes.GetNamedItem("ID").Value);
            }
        }

        private void LoadMod(string modPath)
        {
            string[] xmlFiles = Directory.GetFiles(modPath, "*.xml", SearchOption.AllDirectories);
            foreach (string xmlFile in xmlFiles)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(xmlFile);
                string xmlName = xml.DocumentElement.Name;
                switch(xmlName)
                {
                    case "FloorSprites": LoadFloorSprites(xml); break;
                    case "WallSprites": LoadWallSprites(xml); break;
                }
            }
        }

        private void LoadFloorSprites(XmlDocument xml)
        {
            foreach (XmlNode node in xml.SelectNodes("FloorSprites/FloorSprite"))
            {
                string imageFile = node.Attributes.GetNamedItem("File").Value;
                using (Texture2D texture = TextureUtility.LoadTexture(GraphicsDevice, imageFile))
                {

                }
                //Data.FloorSprites.Add(node.Attributes.GetNamedItem("ID").Value, node.Attributes.GetNamedItem("File").Value);
            }
        }

        private void LoadWallSprites(XmlDocument xml)
        {
            foreach (XmlNode node in xml.SelectNodes("WallSprites/WallSprite"))
            {

            }
        }

    }
}
