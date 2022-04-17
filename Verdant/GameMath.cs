using System;

namespace Verdant
{
    public static class GameMath
    {

        public static Random Random { get; } = new Random();

        /// <summary>
        /// Given two points in 2D space, get the angle between them.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="target">The second point.</param>
        /// <param name="easyAdjust">If true, apply a useful adjustment (negate the angle and add PI/2) to the result automatically.</param>
        /// <returns>A float representing the angle between the two points.</returns>
        public static float AngleBetweenPoints(Vec2 a, Vec2 target, bool easyAdjust = false)
        {
            float angle = (float)Math.Atan2(target.X - a.X, target.Y - a.Y);

            if (easyAdjust)
                return -angle + (float)Math.PI / 2;
            return angle;
        }

        /// <summary>
        /// Given an angle, calculate an appropriate Vec2.
        /// </summary>
        /// <param name="angle">The angle of the vector.</param>
        /// <returns>A Vec2 with a magnitude of 1, pointing in the direction of the angle.</returns>
        public static Vec2 Vec2FromAngle(float angle)
        {
            return new Vec2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }

        /// <summary>
        /// Given two points in 2D space, get the distance between them.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The distance between the two points.</returns>
        public static float GetDistance(Vec2 a, Vec2 b)
        {
            return (float)Math.Sqrt(Math.Pow(b.Y - a.Y, 2) + Math.Pow(b.X - a.X, 2));
        }

        /// <summary>
        /// Check if two AABBs are intersecting.
        /// </summary>
        /// <param name="x1">The x coordinate of the first AABB.</param>
        /// <param name="y1">The y coordinate of the first AABB.</param>
        /// <param name="w1">The width of the first AABB.</param>
        /// <param name="h1">The height of the first AABB.</param>
        /// <param name="x2">The x coordinate of the second AABB.</param>
        /// <param name="y2">The y coordinate of the second AABB.</param>
        /// <param name="w2">The width of the second AABB.</param>
        /// <param name="h2">The height of the second AABB.</param>
        /// <returns>Returns true if the AABBs are intersecting, and false otherwise.</returns>
        public static bool CheckRectIntersection(float x1, float y1, int w1, int h1, float x2, float y2, int w2, int h2)
        {
            return (x1 < x2 + w2 &&
                x1 + w1 > x2 &&
                y1 < y2 + h2 &&
                y1 + h1 > y2);
        }
        /// <summary>
        /// Check if two AABBs are intersecting.
        /// </summary>
        /// <param name="pos1">The position of the first AABB.</param>
        /// <param name="w1">The width of the first AABB.</param>
        /// <param name="h1">The height of the first AABB.</param>
        /// <param name="pos2">The position of the second AABB.</param>
        /// <param name="w2">The width of the second AABB.</param>
        /// <param name="h2">The height of the second AABB.</param>
        /// <returns>Returns true if the AABBs are intersecting, and false otherwise.</returns>
        public static bool CheckRectIntersection(Vec2 pos1, int w1, int h1, Vec2 pos2, int w2, int h2)
        {
            return CheckRectIntersection(pos1.X, pos1.Y, w1, h1, pos2.X, pos2.Y, w2, h2);
        }

        /// <summary>
        /// Check if a 2D point is intersecting with an AABB.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="x">The x coordinate of the AABB.</param>
        /// <param name="y">The y coordinate of the AABB.</param>
        /// <param name="w">The width of the AABB.</param>
        /// <param name="h">The height of the AABB.</param>
        /// <returns>Returns true if the point is within the AABB, and false otherwise.</returns>
        public static bool CheckPointOnRectIntersection(Vec2 point, float x, float y, int w, int h)
        {
            return (point.X < x + w &&
                point.X > x &&
                point.Y < y + h &&
                point.Y > y);
        }

        /// <summary>
        /// Generate a random float value.
        /// </summary>
        /// <returns>A random float value.</returns>
        public static float RandomFloat()
        {
            double mantissa = Random.NextDouble() * 2.0 - 1.0;
            return (float)Math.Abs(mantissa);
        }

        /// <summary>
        /// Generate a random float value within a given bounds.
        /// </summary>
        /// <param name="min">The minimum value of the float.</param>
        /// <param name="max">The maximum value of the float.</param>
        /// <returns>A random float value.</returns>
        public static float RandomFloat(float min, float max)
        {
            double mantissa = Random.NextDouble() * 2.0 - 1.0;
            float final = (float)Math.Abs(mantissa) * (max - min) + min;
            return final;
        }

    }
}
