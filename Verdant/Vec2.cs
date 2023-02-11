using System;
using Microsoft.Xna.Framework;

namespace Verdant
{
    /// <summary>
    /// A 2D vector data type.
    /// </summary>
    public class Vec2
    {
        // The X coordinate.
        public float X { get; set; }
        // The Y coordinate.
        public float Y { get; set; }

        // A Vec2 representing (0.0, 0.0)
        public static Vec2 Zero { get; } = new Vec2(0, 0);

        /// <summary>
        /// Initialize a new Vec2 with components (0, 0).
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

        /// <summary>
        /// Deep equality check.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the object is equal to this Vec2 (deep).</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(Vec2))
                return false;
            Vec2 o = (Vec2)obj;
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

        /// <summary>
        /// Calculate the magnitude of this Vec2.
        /// </summary>
        /// <returns>The magnitude.</returns>
        public float Magnitude()
        {
            return (float) Math.Sqrt(X*X + Y*Y);
        }

        /// <summary>
        /// Calculate the direction of this Vec2.
        /// </summary>
        /// <returns>The direction.</returns>
        public float Direction()
        {
            return (float) Math.Atan2(Y, X);
        }

        /// <summary>
        /// Calculate the opposite direction of this Vec2 (direction + Math.PI).
        /// </summary>
        /// <returns>The opposite direction.</returns>
        public float DirectionOpposite()
        {
            return (float) Math.PI + Direction();
        }

        /// <summary>
        /// Calculate the unit vector of this Vec2.
        /// </summary>
        /// <returns>A new Vec2 of the unit vector.</returns>
        public Vec2 Unit()
        {
            float mag = Magnitude();
            if (mag == 0)
                return new Vec2(0, 0);
            return new Vec2(X / mag, Y / mag);
        }

        /// <summary>
        /// Calculate the normal vector of this Vec2.
        /// </summary>
        /// <returns>A new Vec2 of the normal vector.</returns>
        public Vec2 Normal()
        {
            return new Vec2(-Y, X).Unit();
        }

        /// <summary>
        /// Calculate the dot product of two Vec2s.
        /// </summary>
        /// <param name="a">The first Vec2.</param>
        /// <param name="b">The second Vec2.</param>
        /// <returns>The dot product.</returns>
        public static float Dot(Vec2 a, Vec2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        /// Calculate the cross product of two Vec2s.
        /// </summary>
        /// <param name="a">The first Vec2.</param>
        /// <param name="b">The second Vec2.</param>
        /// <returns>The cross product.</returns>
        public static float Cross(Vec2 a, Vec2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vec2 operator +(Vec2 a) => a;
        public static Vec2 operator -(Vec2 a) => new Vec2(-a.X, -a.Y);
        
        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
        public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.X * b.X, a.Y * b.Y);
        public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.X / b.X, a.Y / b.Y);

        public static Vec2 operator *(Vec2 a, float c) => new Vec2(a.X * c, a.Y * c);
        public static Vec2 operator /(Vec2 a, float c) => new Vec2(a.X / c, a.Y / c);
        public static Vec2 operator +(Vec2 a, float c) => new Vec2(a.X + c, a.Y + c);
        public static Vec2 operator -(Vec2 a, float c) => new Vec2(a.X - c, a.Y - c);

        public static explicit operator Vector2(Vec2 vec2) => new Vector2(vec2.X, vec2.Y);
        public static explicit operator Vec2Int(Vec2 vec2) => new Vec2Int((int)vec2.X, (int)vec2.Y);
    
    }
}
