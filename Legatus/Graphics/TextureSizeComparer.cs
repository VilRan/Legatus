using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Legatus.Graphics
{
    public class TextureSizeComparer : IComparer<Texture2D>
    {
        public int Compare(Texture2D a, Texture2D b)
        {
            if (a.Width * a.Height > b.Width * b.Height) return -1;
            else if (a.Width * a.Height < b.Width * b.Height) return 1;
            else return 0;
        }
    }
}