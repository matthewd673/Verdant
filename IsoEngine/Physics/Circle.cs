using System;

namespace IsoEngine.Physics
{
    internal class Circle : Shape
    {

        internal float Radius { get; set; }

        public Circle(float x, float y, float r)
        {
            Vertices = new Vec2[2];
            Position = new Vec2(x, y);
            Radius = r;
        }

    }
}
