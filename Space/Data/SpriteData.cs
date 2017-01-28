using System.Xml;
using Legatus.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space.Data
{
    public class SpriteData : GameData
    {
        public BasicSprite Base { private set; get; }
        public Texture2D Texture { get { return Base.Texture; } }
        public Rectangle SourceRectangle { get { return Base.Source; } }
        public Vector2 Origin { get { return new Vector2(Width / 2, Height / 2); } }
        public int Width { get { return SourceRectangle.Width; } }
        public int Height { get { return SourceRectangle.Height; } }


        public SpriteData()
        {

        }

        public override void Initialize(XmlElement node, GameDataManager manager)
        {
            BasicSprite sprite = manager.Textures[node.GetAttribute("Texture")];
            Texture2D texture = sprite.Texture;
            string[] offset = node.GetAttribute("Offset").Split(',');
            int offsetX = int.Parse(offset[0]);
            int offsetY = int.Parse(offset[1]);
            string[] size = node.GetAttribute("Size").Split(',');
            int sizeX = int.Parse(size[0]);
            int sizeY = int.Parse(size[1]);
            Rectangle source = new Rectangle(offsetX, offsetY, sizeX, sizeY);
            source.Offset(sprite.Source.Location);
            Base = new BasicSprite(texture, source);
        }
    }
}
