using System;

namespace IsoEngine
{
    public static class Math
    {

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

    }
}
