using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Legatus.Mathematics
{
    public class RectangleSizeComparer : IComparer<Rectangle>
    {
        public int Compare(Rectangle a, Rectangle b)
        {
            if (a.Width * a.Height > b.Width * b.Height) return -1;
            else if (a.Width * a.Height < b.Width * b.Height) return 1;
            else return 0;
        }
    }

    public class InverseRectangleSizeComparer : IComparer<Rectangle>
    {
        public int Compare(Rectangle a, Rectangle b)
        {
            if (a.Width * a.Height > b.Width * b.Height) return 1;
            else if (a.Width * a.Height < b.Width * b.Height) return -1;
            else return 0;
        }
    }
}
