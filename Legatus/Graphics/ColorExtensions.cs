using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Legatus.Graphics
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns the RGBA average of the two Colors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color Mix(this Color a, Color b)
        {
            return new Color((a.R + b.R) / 2, (a.G + b.G) / 2, (a.B + b.B) / 2, (a.A + b.A) / 2);
        }

        public static Color Average(IEnumerable<Color> colors)
        {
            int r = 0, g = 0, b = 0, a = 0, n = 0;
            foreach (Color color in colors)
            {
                r += color.R;
                g += color.G;
                b += color.B;
                a += color.A;
                n++;
            }
            r /= n;
            g /= n;
            b /= n;
            a /= n;

            return new Color(r, g, b, a);
        }
    }
}
