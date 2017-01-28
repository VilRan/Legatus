using Microsoft.Xna.Framework;

namespace Legatus.Mathematics
{
    public static class PointExtensions
    {
        public static Point Divide(this Point point, int dividend)
        {
            return new Point(point.X / dividend, point.Y / dividend);
        }
    }
}
