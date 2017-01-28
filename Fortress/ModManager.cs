using Legatus;
using Legatus.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Fortress
{
    public class ModManager
    {
        public Dictionary<string, BasicSprite> Textures = new Dictionary<string, BasicSprite>();
        public Dictionary<string, SpriteGroup> Sprites = new Dictionary<string, SpriteGroup>();
        public Dictionary<string, Terrain> Terrains = new Dictionary<string, Terrain>();

        private LegatusGame Game;
        private TextureAtlasManager TextureAtlases;
        private List<Tuple<IModAsset, XmlElement>> Uninitialized = new List<Tuple<IModAsset, XmlElement>>();

        public ModManager(LegatusGame game)
        {
            Game = game;
            TextureAtlases = new TextureAtlasManager(game);
            Load();
        }

        public void Reload()
        {
            Textures.Clear();
            Sprites.Clear();
            Terrains.Clear();
            TextureAtlases.Dispose();
            GC.Collect();

            Load();
        }

        private void Load()
        {
            XmlDocument settings = new XmlDocument();
            settings.Load("Settings.xml");
            //Language = xml.SelectSingleNode("Settings/Localization").Attributes.GetNamedItem("Language").Value;

            foreach (XmlNode mod in settings.SelectNodes("Settings/Mods/Mod"))
                LoadMod("Mods/" + mod.FirstChild.Value);

            foreach (Tuple<IModAsset, XmlElement> data in Uninitialized)
                data.Item1.Initialize(data.Item2, this);

            Uninitialized.Clear();
            TextureAtlases.ApplyChanges();
        }
        
        private void LoadMod(string modPath)
        {
            if (!Directory.Exists(modPath))
                return;

            string[] xmlPaths = Directory.GetFiles(modPath, "*.xml", SearchOption.AllDirectories);
            foreach (string xmlPath in xmlPaths)
                LoadXml(xmlPath);
        }
        
        private void LoadXml(string xmlPath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath);
            foreach (XmlElement node in xml.DocumentElement.ChildNodes.OfType<XmlElement>())
                LoadNode(node);
        }

        private void LoadNode(XmlElement node)
        {
            string id = node.GetAttribute("ID");
            IModAsset asset;
            switch (node.Name)
            {
                case "Texture": LoadTexture(node); return;
                case "Sprite": asset = new SpriteGroup(); Sprites.Add(id, (SpriteGroup)asset); break;
                case "Terrain": asset = new Terrain(); Terrains.Add(id, (Terrain)asset); break;
                //case "String": LoadLocalization(node); return;
                default: return; //Should throw an exception to inform about invalid node type, maybe.
            }
            Uninitialized.Add(new Tuple<IModAsset, XmlElement>(asset, node));
        }

        private void LoadTexture(XmlElement node)
        {
            string id = node.GetAttribute("ID");
            string file = node.GetAttribute("Source");
            using (Texture2D texture = TextureUtility.LoadTexture(Game.GraphicsDevice, "Mods/" + file))
                Textures.Add(id, TextureAtlases.Copy(texture));
        }
    }
}
