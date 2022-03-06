using System;

namespace IsoEngine.Physics
{
    internal class Line : Shape
    {
        public Line(float x1, float y1, float x2, float y2)
        {
            Vertices = new Vec2[2];
            Vertices[0] = new Vec2(x1, y1);
            Vertices[1] = new Vec2(x2, y2);
            Dir = (Vertices[1] - Vertices[0]).Unit();
            Position = new Vec2((Vertices[0].X + Vertices[1].X) / 2, (Vertices[0].Y + Vertices[1].Y) / 2);
        }
    }
}
