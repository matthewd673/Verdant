using System;
using Microsoft.Xna.Framework;

namespace Verdant
{
    /// <summary>
    /// A 2D vector data type that stores only integer values.
    /// </summary>
    public class Vec2Int
    {

        // The X coordinate.
        public int X { get; set; }
        // The Y coordinate.
        public int Y { get; set; }

        // A Vec2Int representing (0, 0)
        public static Vec2Int Zero { get; } = new Vec2Int(0, 0);

        /// <summary>
        /// Initialize a new Vec2Int with components (0, 0).
        /// </summary>
        public Vec2Int()
        {
            X = 0;
            Y = 0;
        }
        /// <summary>
        /// Initialize a new Vec2Int.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        public Vec2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Deep equality check.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the object is equal to this Vec2Int (deep).</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(Vec2Int))
                return false;
            Vec2Int o = (Vec2Int)obj;
            return (X == o.X && Y == o.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public static Vec2Int operator +(Vec2Int a) => a;
        public static Vec2Int operator -(Vec2Int a) => -a;

        public static Vec2Int operator +(Vec2Int a, Vec2Int b) => new Vec2Int(a.X + b.X, a.Y + b.Y);
        public static Vec2Int operator -(Vec2Int a, Vec2Int b) => new Vec2Int(a.X - b.X, a.Y - b.Y);
        public static Vec2Int operator *(Vec2Int a, Vec2Int b) => new Vec2Int(a.X * b.X, a.Y * b.Y);
        public static Vec2Int operator /(Vec2Int a, Vec2Int b) => new Vec2Int(a.X / b.X, a.Y / b.Y);

        public static Vec2Int operator *(Vec2Int a, int c) => new Vec2Int(a.X * c, a.Y * c);
        public static Vec2Int operator /(Vec2Int a, int c) => new Vec2Int(a.X / c, a.Y / c);
        public static Vec2Int operator +(Vec2Int a, int c) => new Vec2Int(a.X + c, a.Y + c);
        public static Vec2Int operator -(Vec2Int a, int c) => new Vec2Int(a.X - c, a.Y - c);

        public static explicit operator Vector2(Vec2Int vec2Int) => new Vector2(vec2Int.X, vec2Int.Y);
        public static explicit operator Vec2(Vec2Int vec2Int) => new Vec2(vec2Int.X, vec2Int.Y);

    }
}
