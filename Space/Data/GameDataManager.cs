using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Legatus.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;

namespace Space.Data
{
    public class GameDataManager
    {
        public Dictionary<string, string> Localization = new Dictionary<string, string>();
        public Dictionary<string, SpriteData> Sprites = new Dictionary<string, SpriteData>();
        public Dictionary<string, CrewTypeData> CrewTypes = new Dictionary<string, CrewTypeData>();
        public Dictionary<string, ModuleData> Modules = new Dictionary<string, ModuleData>();
        public Dictionary<string, ProjectileData> Projectiles = new Dictionary<string, ProjectileData>();
        public Dictionary<string, ResourceData> Resources = new Dictionary<string, ResourceData>();
        public Dictionary<string, TraitData> Traits = new Dictionary<string, TraitData>();
        public Dictionary<string, WeaponData> Weapons = new Dictionary<string, WeaponData>();
        public Dictionary<string, WorkTypeData> WorkTypes = new Dictionary<string, WorkTypeData>();
        public Dictionary<string, BasicSprite> Textures = new Dictionary<string, BasicSprite>();
        public int GridSize;

        private List<Tuple<GameData, XmlElement>> Uninitialized = new List<Tuple<GameData, XmlElement>>();
        private TextureAtlasManager TextureAtlases;
        private SpaceGame Game;
        private string Language;

        public GameDataManager(SpaceGame game)
        {
            Game = game;
            TextureAtlases = new TextureAtlasManager(game);
            Load();
        }

        public void Reload()
        {
            Localization.Clear();
            CrewTypes.Clear();
            Modules.Clear();
            Projectiles.Clear();
            Resources.Clear();
            Traits.Clear();
            Weapons.Clear();
            WorkTypes.Clear();
            Sprites.Clear();
            Textures.Clear();
            TextureAtlases.Dispose();
            GC.Collect();

            Load();
        }

        private void Load()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("Settings.xml");
            Language = xml.SelectSingleNode("Settings/Localization").Attributes.GetNamedItem("Language").Value;

            foreach (XmlNode node in xml.SelectNodes("Settings/Mods/Mod"))
                LoadMod("Content/" + node.Attributes.GetNamedItem("ID").Value);

            foreach (Tuple<GameData, XmlElement> data in Uninitialized)
                data.Item1.Initialize(data.Item2, this);

            Uninitialized.Clear();
            TextureAtlases.ApplyChanges();
        }

        private void LoadMod(string modPath)
        {
            if ( ! Directory.Exists(modPath))
                return;

            string[] xmlFiles = Directory.GetFiles(modPath, "*.xml", SearchOption.AllDirectories);
            foreach (string xmlFile in xmlFiles)
                LoadFile(xmlFile);
        }

        private void LoadFile(string xmlFile)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);
            foreach (XmlElement node in xml.DocumentElement.ChildNodes.OfType<XmlElement>())
                LoadNode(node);
        }

        private void LoadNode(XmlElement node)
        {
            string id = node.GetAttribute("ID");
            GameData data;
            switch (node.Name)
            {
                case "GridSize": GridSize = int.Parse(node.GetAttribute("Pixels"), CultureInfo.InvariantCulture); return;
                case "Sprite": data = new SpriteData(); Sprites.Add(id, (SpriteData)data); break;
                case "CrewType": data = new CrewTypeData(); CrewTypes.Add(id, (CrewTypeData)data); break;
                case "Module": data = new ModuleData(); Modules.Add(id, (ModuleData)data); break;
                case "Projectile": data = new ProjectileData(); Projectiles.Add(id, (ProjectileData)data); break;
                case "Resource": data = new ResourceData(); Resources.Add(id, (ResourceData)data); break;
                case "Trait": data = new TraitData(); Traits.Add(id, (TraitData)data); break;
                case "Weapon": data = new WeaponData(); Weapons.Add(id, (WeaponData)data); break;
                case "WorkType": data = new WorkTypeData(); WorkTypes.Add(id, (WorkTypeData)data); break;
                case "Texture": LoadTexture(node); return;
                case "String": LoadLocalization(node); return;
                default: return; //Should throw an exception to inform about invalid node type, maybe.
            }
            Uninitialized.Add(new Tuple<GameData, XmlElement>(data, node));
        }
        
        private void LoadTexture(XmlElement node)
        {
            string id = node.GetAttribute("ID");
            string file = node.GetAttribute("File");
            using (Texture2D texture = TextureUtility.LoadTexture(Game.GraphicsDevice, "Content/" + file))
                Textures.Add(id, TextureAtlases.Copy(texture));
        }

        private void LoadLocalization(XmlElement node)
        {
            string id = node.OwnerDocument.DocumentElement.GetAttribute("Language") + node.GetAttribute("ID");
            foreach (XmlNode subnode in node.ChildNodes)
            {
                if (subnode.NodeType == XmlNodeType.Text)
                    Localization.Add(id, subnode.InnerText.Trim('\r', '\n', ' ').Replace("\\n", "\n"));
                else if (subnode.NodeType == XmlNodeType.Element)
                    Localization.Add(id + subnode.Name, subnode.InnerText.Trim('\r', '\n', ' ').Replace("\\n", "\n"));
            }
        }
    }
}
