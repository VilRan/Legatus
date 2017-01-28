using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Legatus;
using Legatus.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Empire.Planetary
{
    public class Biome
    {
        public readonly BasicSprite[] Sprites;
        public readonly string UID;
        public readonly string Name;

        public Biome(string uid, string name, BasicSprite[] sprites)
        {
            UID = uid;
            Name = name;
            Sprites = sprites;
        }
    }
}
