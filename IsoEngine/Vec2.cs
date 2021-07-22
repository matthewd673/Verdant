using System;

namespace IsoEngine
{
    public class Vec2
    {
        public float x;
        public float y;

        public static readonly Vec2 zero = new Vec2(0, 0);

        public Vec2()
        {
            x = 0;
            y = 0;
        }
        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2 Copy()
        {
            return new Vec2(x, y);
        }

        public static Vec2 operator +(Vec2 a) => a;
        public static Vec2 operator -(Vec2 a) => new Vec2(-a.x, -a.y);
        
        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.x + b.x, a.y + b.y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.x - b.x, a.y - b.y);
        public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.x * b.x, a.y * b.y);
        public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.x / b.x, a.y / b.y);

        public static Vec2 operator *(Vec2 a, int c) => new Vec2(a.x * c, a.y * c);
        public static Vec2 operator *(Vec2 a, float c) => new Vec2(a.x * c, a.y * c);
        public static Vec2 operator +(Vec2 a, float c) => new Vec2(a.x + c, a.y + c);
        public static Vec2 operator -(Vec2 a, float c) => new Vec2(a.x - c, a.y - c);
    }
}
