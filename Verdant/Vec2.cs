using System;
using Microsoft.Xna.Framework;

namespace Verdant
{

    [Serializable]
    public class Vec2
    {

        public float X { get; set; }
        public float Y { get; set; }

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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(Vec2))
                return false;
            Vec2 o = (Vec2)obj;
            return (X == o.X && Y == o.Y);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public float Magnitude()
        {
            return (float) Math.Sqrt(X*X + Y*Y);
        }

        public Vec2 Unit()
        {
            float mag = Magnitude();
            if (mag == 0)
                return new Vec2(0, 0);
            return new Vec2(X / mag, Y / mag);
        }

        public Vec2 Normal()
        {
            //a faster way to achieve: return new Vec2(-Y, X).Unit();
            //float mag = Magnitude();
            //if (mag == 0)
            //    return new Vec2(0, 0);
            //return new Vec2(-Y / mag, X / mag);
            return new Vec2(-Y, X).Unit();
        }

        public static float Dot(Vec2 a, Vec2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static float Cross(Vec2 a, Vec2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        //public static bool operator ==(Vec2 a, Vec2 b) => a.Equals(b);
        //public static bool operator !=(Vec2 a, Vec2 b) => !a.Equals(b);

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
