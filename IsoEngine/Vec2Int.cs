using System;

namespace IsoEngine
{
    public class Vec2Int
    {
        public int x;
        public int y;

        public Vec2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vec2(Vec2Int vec2Int) => new Vec2(vec2Int.x, vec2Int.y);

    }
}
