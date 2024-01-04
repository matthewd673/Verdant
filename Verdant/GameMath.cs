using System;
using System.Collections.Generic;

namespace Verdant
{
    /// <summary>
    /// A collection of helpful math functions.
    /// </summary>
    public static class GameMath
    {

        // The global Random instance, used by any internal random number generation.
        public static Random Random { get; } = new Random();

        /// <summary>
        /// Given two points in 2D space, get the angle between them.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="target">The second point.</param>
        /// <returns>A float representing the angle between the two points.</returns>
        public static float AngleBetweenPoints(Vec2 a, Vec2 target)
        {
            return (float)Math.Atan2(target.X - a.X, target.Y - a.Y);
        }

        /// <summary>
        /// Given an angle, calculate an appropriate Vec2.
        /// </summary>
        /// <param name="angle">The angle of the vector.</param>
        /// <returns>A Vec2 with a magnitude of 1, pointing in the direction of the angle.</returns>
        public static Vec2 AngleToVec2(float angle)
        {
            return new Vec2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }

        /// <summary>
        /// Given a Vec2, calculate its angle from the origin.
        /// </summary>
        /// <param name="vec">The vector to calculate from.</param>
        /// <returns>The angle from the origin to the given vector.</returns>
        public static float Vec2ToAngle(Vec2 vec)
        {
            return AngleBetweenPoints(Vec2.Zero, vec);
        }

        /// <summary>
        /// Given two points in 2D space, get the distance between them.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The distance between the two points.</returns>
        public static float DistanceBetweenPoints(Vec2 a, Vec2 b)
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
        public static bool RectIntersection(float x1, float y1, int w1, int h1, float x2, float y2, int w2, int h2)
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
        public static bool RectIntersection(Vec2 pos1, int w1, int h1, Vec2 pos2, int w2, int h2)
        {
            return RectIntersection(pos1.X, pos1.Y, w1, h1, pos2.X, pos2.Y, w2, h2);
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
        public static bool PointOnRectIntersection(Vec2 point, float x, float y, float w, float h)
        {
            return (point.X < x + w &&
                point.X > x &&
                point.Y < y + h &&
                point.Y > y);
        }

        public static bool LineIntersection(float x1, float y1, float x2, float y2,
                                            float x3, float y3, float x4, float y4)
        {
            // https://www.jeffreythompson.org/collision-detection/line-line.php
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) /
                       ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) /
                       ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;
        }

        public static bool LineIntersection(Vec2 aStart, Vec2 aEnd,
                                            Vec2 bStart, Vec2 bEnd)
        {
            return LineIntersection(aStart.X, aStart.Y, aEnd.X, aEnd.Y,
                                    bStart.X, bStart.Y, bEnd.X, bEnd.Y);
        }

        public static bool LineOnRectIntersection(Vec2 start, Vec2 end,
                                                  float x1, float y1,
                                                  float x2, float y2)
        {
            // essentially testing one line against four others
            return LineIntersection(start.X, start.Y, end.X, end.Y,
                                    x1, y1, x2, y1) ||
                   LineIntersection(start.X, start.Y, end.X, end.Y,
                                    x1, y1, x1, y2) ||
                   LineIntersection(start.X, start.Y, end.X, end.Y,
                                    x2, y1, x2, y2) ||
                   LineIntersection(start.X, start.Y, end.X, end.Y,
                                    x1, y2, x2, y2);
        }

        public static Vec2 LineIntersectionPoint(float x1, float y1, float x2, float y2,
                                            float x3, float y3, float x4, float y4)
        {
            // https://www.jeffreythompson.org/collision-detection/line-line.php
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) /
                       ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) /
                       ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if collision, return collision points
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                return new(x1 + (uA * (x2 - x1)), y1 + (uA * (y2 - y1)));
            }

            return null;
        }

        public static Vec2 LineIntersectionPoint(Vec2 aStart, Vec2 aEnd,
                                                 Vec2 bStart, Vec2 bEnd)
        {
            return LineIntersectionPoint(aStart.X, aStart.Y, aEnd.X, aEnd.Y,
                                         bStart.X, bStart.Y, bEnd.X, bEnd.Y);
        }

        public static List<Vec2> LineOnRectIntersectionPoints(Vec2 start, Vec2 end,
                                                              float x1, float y1,
                                                              float x2, float y2)
        {
            List<Vec2> points = new();

            Vec2 c1 = LineIntersectionPoint(start.X, start.Y, end.X, end.Y,
                                            x1, y1, x2, y1);
            if (c1 != null)
            {
                points.Add(c1);
            }

            Vec2 c2 = LineIntersectionPoint(start.X, start.Y, end.X, end.Y,
                                            x1, y1, x1, y2);
            if (c2 != null)
            {
                points.Add(c2);
            }

            Vec2 c3 = LineIntersectionPoint(start.X, start.Y, end.X, end.Y,
                                            x2, y1, x2, y2);
            if (c3 != null)
            {
                points.Add(c3);
            }

            Vec2 c4 = LineIntersectionPoint(start.X, start.Y, end.X, end.Y,
                                            x1, y2, x2, y2);
            if (c4 != null)
            {
                points.Add(c4);
            }

            return points;
        }

        /// <summary>
        /// Generate a random float value between 0 and 1.
        /// </summary>
        /// <returns>A random float value.</returns>
        public static float RandomFloat()
        {
            double mantissa = Random.NextDouble() * 2.0 - 1.0;
            return (float)Math.Abs(mantissa);
        }

        /// <summary>
        /// Generate a random float value within the given bounds.
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
