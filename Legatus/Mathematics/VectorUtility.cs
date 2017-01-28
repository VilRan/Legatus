using System;
using Microsoft.Xna.Framework;

namespace Legatus.Mathematics
{
    public static class VectorUtility
    {
        /// <summary>
        /// Returns a value between -pi and pi
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float AngleBetweenVectors(Vector2 v1, Vector2 v2)
        {
            return MathHelper.WrapAngle((float)(Math.Atan2(v2.Y, v2.X) - Math.Atan2(v1.Y, v1.X)));
        }

        public static Vector2 CreateVector(double angleRadians, float magnitude)
        {
            return new Vector2((float)Math.Cos(angleRadians), (float)Math.Sin(angleRadians)) * magnitude;
        }

        public static Vector2 RotateUnitVector(Vector2 vector, float radians)
        {
            radians += (float)Math.Atan2(vector.Y, vector.X);
            return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        public static Vector2 RotateUnitVector(Vector2 vector, double radians)
        {
            radians += Math.Atan2(vector.Y, vector.X);
            return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        public static Vector2 RotateVector(Vector2 vector, float radians)
        {
            return Vector2.Transform(vector, Matrix.CreateRotationZ(radians));
        }
        
        public static float WrappingDistanceSquared(Vector2 v1, Vector2 v2, float maxX)
        {
            float halfMaxX = maxX / 2;
            if (v2.X - v1.X > halfMaxX)
                v2.X -= maxX;
            if (v1.X - v2.X > halfMaxX)
                v1.X -= maxX;

            return (v2 - v1).LengthSquared();
        }

        public static float WrappingDistance(Vector2 v1, Vector2 v2, float maxX)
        {
            float halfMaxX = maxX / 2;
            if (v2.X - v1.X > halfMaxX)
                v2.X -= maxX;
            if (v1.X - v2.X > halfMaxX)
                v1.X -= maxX;

            return (v2 - v1).Length();
        }

        /// <summary>
        /// Returns null if interception is impossible.
        /// </summary>
        /// <param name="shooterPosition"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetVelocity"></param>
        /// <param name="projectileSpeed"></param>
        /// <returns></returns>
        public static Vector2? FindInterceptPoint(Vector2 shooterPosition, Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
        {
            return FindInterceptPoint(shooterPosition, Vector2.Zero, targetPosition, targetVelocity, projectileSpeed);
        }

        /// <summary>
        /// Returns null if interception is impossible.
        /// IMPORTANT: don't use this overload unless the shooter's velocity is added to the projectile's velocity.
        /// </summary>
        /// <param name="shooterPosition"></param>
        /// <param name="shooterVelocity"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetVelocity"></param>
        /// <param name="projectileSpeed"></param>
        /// <returns></returns>
        public static Vector2? FindInterceptPoint(Vector2 shooterPosition, Vector2 shooterVelocity, Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
        {
            Vector2 relativePosition = targetPosition - shooterPosition;
            Vector2 relativeVelocity = targetVelocity - shooterVelocity;
            float a = projectileSpeed * projectileSpeed - relativeVelocity.LengthSquared();//Vector2.Dot(targetVelocity, targetVelocity);
            float b = -2 * Vector2.Dot(relativeVelocity, relativePosition);
            float c = -relativePosition.LengthSquared();// Vector2.Dot(-relativePosition, relativePosition);
            float d = b * b - 4 * a * c;
            if (d > 0)
            {
                float result = ( b + (float)Math.Sqrt(d) ) / (2 * a);
                return targetPosition + result * relativeVelocity;
            }
            return null;
        }
    }
}
