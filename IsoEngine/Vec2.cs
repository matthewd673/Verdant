using System;
using Microsoft.Xna.Framework;

namespace IsoEngine
{
    public class Vec2
    {
        public float X;
        public float Y;

        public static Vec2 Zero { get; } = new Vec2(0, 0);

        public Vec2()
        {
            X = 0;
            Y = 0;
        }
        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vec2 Copy()
        {
            return new Vec2(X, Y);
        }

        public static Vec2 operator +(Vec2 a) => a;
        public static Vec2 operator -(Vec2 a) => new Vec2(-a.X, -a.Y);
        
        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
        public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.X * b.X, a.Y * b.Y);
        public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.X / b.X, a.Y / b.Y);

        public static Vec2 operator *(Vec2 a, int c) => new Vec2(a.X * c, a.Y * c);
        public static Vec2 operator *(Vec2 a, float c) => new Vec2(a.X * c, a.Y * c);
        public static Vec2 operator /(Vec2 a, int c) => new Vec2(a.X / c, a.Y / c);
        public static Vec2 operator /(Vec2 a, float c) => new Vec2(a.X / c, a.Y / c);
        public static Vec2 operator +(Vec2 a, float c) => new Vec2(a.X + c, a.Y + c);
        public static Vec2 operator -(Vec2 a, float c) => new Vec2(a.X - c, a.Y - c);

        public static explicit operator Vector2(Vec2 vec2) => new Vector2(vec2.X, vec2.Y);
        public static explicit operator Vec2Int(Vec2 vec2) => new Vec2Int((int)vec2.X, (int)vec2.Y);
    
    }
}
