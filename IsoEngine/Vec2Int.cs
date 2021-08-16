using System;
using Microsoft.Xna.Framework;

namespace IsoEngine
{
    public class Vec2Int
    {

        public int X { get; set; }
        public int Y { get; set; }

        public static Vec2Int Zero { get; } = new Vec2Int(0, 0);

        /// <summary>
        /// Initialize a Vec2Int equal to (0, 0).
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

        public static explicit operator Vector2(Vec2Int vec2Int) => new Vector2(vec2Int.X, vec2Int.Y);
        public static implicit operator Vec2(Vec2Int vec2Int) => new Vec2(vec2Int.X, vec2Int.Y);

    }
}
