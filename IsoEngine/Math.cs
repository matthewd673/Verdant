using System;

namespace IsoEngine
{
    public static class Math
    {

        public static Random rng = new Random();

        public static float AngleBetweenPoints(Vec2 a, Vec2 target)
        {
            return (float)System.Math.Atan2(target.x - a.x, target.y - a.y);
        }

        public static Vec2 Vec2FromAngle(float angle)
        {
            return new Vec2((float)System.Math.Sin(angle), (float)System.Math.Cos(angle));
        }

        public static float GetDistance(Vec2 a, Vec2 b)
        {
            return (float)System.Math.Sqrt(System.Math.Pow(b.y - a.y, 2) + System.Math.Pow(b.x - a.x, 2));
        }

        public static bool CheckRectIntersection(float x1, float y1, int w1, int h1, float x2, float y2, int w2, int h2)
        {
            return (x1 < x2 + w2 &&
                x1 + w1 > x2 &&
                y1 < y2 + h2 &&
                y1 + h1 > y2);
        }
        public static bool CheckRectIntersection(Vec2 pos1, int w1, int h1, Vec2 pos2, int w2, int h2)
        {
            return CheckRectIntersection(pos1.x, pos1.y, w1, h1, pos2.x, pos2.y, w2, h2);
        }

        public static float RandomFloat()
        {
            double mantissa = rng.NextDouble() * 2.0 - 1.0;
            return (float)System.Math.Abs(mantissa);
        }

    }
}
