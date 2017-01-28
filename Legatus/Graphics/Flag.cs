using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Legatus.Graphics
{
    public class Flag
    {
        public class Region
        {
            public Texture2D Texture;
            public Color Color;
            public float X, Y, Width, Height;

            public Region(Texture2D texture, Color color, float x, float y, float width, float height)
            {
                Texture = texture;
                Color = color;
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
        }

        public List<Region> Regions;

        public Flag()
        {
            Regions = new List<Region>();
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            foreach (Region region in Regions)
            {
                Rectangle destination = new Rectangle((int)Math.Round(position.X * region.X), (int)Math.Round(position.Y * region.Y), 
                    (int)Math.Round(position.Width * region.Width), (int)Math.Round(position.Height * region.Height));

                spriteBatch.Draw(region.Texture, destination, region.Color);
            }
        }
    }
}
