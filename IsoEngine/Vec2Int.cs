using System;
using Microsoft.Xna.Framework;

namespace IsoEngine
{
    public class Vec2Int
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Vec2Int Zero { get; } = new Vec2Int(0, 0);

        public Vec2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static explicit operator Vector2(Vec2Int vec2Int) => new Vector2(vec2Int.X, vec2Int.Y);
        public static implicit operator Vec2(Vec2Int vec2Int) => new Vec2(vec2Int.X, vec2Int.Y);

    }
}
