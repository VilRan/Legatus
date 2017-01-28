using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legatus.Mathematics
{
    public static class VectorExtensions
    {
        public static Vector2 WrapX(this Vector2 vector, float maxX)
        {
            return new Vector2(MathExtra.Wrap(vector.X, maxX), vector.Y);
        }

    }
}
