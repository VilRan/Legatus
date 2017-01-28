using Legatus.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Fortress
{
    public class Terrain : IModAsset
    {
        public SpriteGroup SpriteGroup;
        public double Cost;

        public Terrain()
        {

        }

        public void Initialize(XmlElement node, ModManager manager)
        {
            SpriteGroup = manager.Sprites[node.GetAttribute("Sprite")];
            Cost = double.Parse(node.GetAttribute("Cost"), CultureInfo.InvariantCulture);
        }
    }
}
