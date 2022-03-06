using System;

namespace IsoEngine.Physics
{
    internal class Wall : Body
    {

        public Wall(float x1, float y1, float x2, float y2) : base()
        {
            Components = new Shape[] { new Line(x1, y1, x2, y2) };
        }
        
    }
}
