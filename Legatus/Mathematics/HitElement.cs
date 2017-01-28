using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Legatus.Mathematics
{
    public abstract class HitElement
    {
        /// <summary>
        /// Returns null if no intersection.
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        public abstract Vector2? FindIntersection(HitRay ray);
    }

    public class HitRay : HitElement
    {
        public Vector2 Start;
        public Vector2 Length;
        public Vector2 End { get { return Start + Length; } set { Length = value - Start; } }

        public HitRay(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public override Vector2? FindIntersection(HitRay other)
        {
            float denominator = Length.X * other.Length.Y - other.Length.X * Length.Y;
            if (denominator == 0)
                return null;

            float dx = Start.X - other.Start.X;
            float dy = Start.Y - other.Start.Y;
            float s = (Length.X         * dy    - Length.Y          * dx) / denominator;
            float t = (other.Length.X   * dy    - other.Length.Y    * dx) / denominator;

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
                return Start + t * Length;
            
            return null;
        }
    }

    public class HitPolygon : HitElement
    {
        public Vector2[] Points;
        private HitRay[] Sides;

        public HitPolygon(params Vector2[] points)
        {
            Points = points;
            Sides = GenerateSides().ToArray();
        }
        
        public void MoveBy(Vector2 amount)
        {
            for (int i = 0; i < Points.Length; i++)
                Points[i] += amount;
            Sides = GenerateSides().ToArray();
        }
        
        public override Vector2? FindIntersection(HitRay ray)
        {
            List<Vector2> intersections = new List<Vector2>(Sides.Length);
            foreach (HitRay side in Sides)
            {
                Vector2? intersection = side.FindIntersection(ray);
                if (intersection != null)
                    intersections.Add(intersection.Value);
            }

            switch (intersections.Count)
            {
                case 0: return null;
                case 1: return intersections[0];
                default:
                    intersections.OrderBy(e => (e - ray.Start).LengthSquared());
                    return intersections[0];
            }
        }

        private IEnumerable<HitRay> GenerateSides()
        {
            Vector2 previous = Points[Points.Length - 1];
            for (int i = 0; i < Points.Length; i++)
            {
                yield return new HitRay(previous, Points[i]);
                previous = Points[i];
            }
        }
    }

    /*
    public class HitCircle : HitRegion
    {

    }
    */
}
