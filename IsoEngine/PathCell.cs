using System;

namespace IsoEngine
{
    public class PathCell
    {

        public int x;
        public int y;

        public int f;
        public int g;
        public int h;

        public PathCell parent;

        public PathCell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
