using System;
using Microsoft.Xna.Framework;

namespace IsoEngine
{
    public class Vec2
    {

        public float X { get; set; }
        public float Y { get; set; }

        public static Vec2 Zero { get; } = new Vec2(0, 0);

        /// <summary>
        /// Initialize a Vec2 equal to (0f, 0f).
        /// </summary>
        public Vec2()
        {
            X = 0;
            Y = 0;
        }
        /// <summary>
        /// Initialize a new Vec2.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Create a new Vec2 with the same x and y as this Vec2.
        /// </summary>
        /// <returns>A new Vec2, equal to this Vec2.</returns>
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
