using Legatus.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace Fortress
{
    public class SpriteGroup : IModAsset
    {
        public List<BasicSprite> Variants = new List<BasicSprite>();

        public SpriteGroup()
        {

        }

        public BasicSprite GetVariant(Random random)
        {
            return Variants[random.Next(Variants.Count)];
        }

        public void Initialize(XmlElement node, ModManager manager)
        {
            foreach (XmlElement variantNode in node.SelectNodes("Variants/Variant"))
            {
                BasicSprite texture = manager.Textures[variantNode.GetAttribute("Texture")];
                int x = texture.X + int.Parse(variantNode.GetAttribute("X"), CultureInfo.InvariantCulture);
                int y = texture.Y + int.Parse(variantNode.GetAttribute("Y"), CultureInfo.InvariantCulture);
                int w = int.Parse(variantNode.GetAttribute("Width"), CultureInfo.InvariantCulture);
                int h = int.Parse(variantNode.GetAttribute("Height"), CultureInfo.InvariantCulture);
                Rectangle rectangle = new Rectangle(x, y, w, h);
                BasicSprite variant = new BasicSprite(texture.Texture, rectangle);
                Variants.Add(variant);
            }
        }
    }
}
