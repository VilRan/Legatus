using Legatus;
using Space.Data;
using Space.ShipDesigner;
using System.Xml;

namespace Space
{
    public class SpaceGame : LegatusGame
    {
        public GameDataManager Data;

        public SpaceGame()
            : base()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("Settings.xml");
            XmlElement display = (XmlElement)xml.SelectSingleNode("Settings/Display");
            switch (display.GetAttribute("Mode"))
            {
                case "Fullscreen": Graphics.IsFullScreen = true; break;
                case "Borderless": Window.IsBorderless = true; break;
                case "Windowed": Window.IsBorderless = false; break;
            }
            int width, height;
            if (int.TryParse(display.GetAttribute("Width"), out width))
                Graphics.PreferredBackBufferWidth = width;
            if (int.TryParse(display.GetAttribute("Height"), out height))
                Graphics.PreferredBackBufferHeight = height;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Data = new GameDataManager(this);
            ActiveState = new ShipDesignerGameState(this);
        }
    }
}
