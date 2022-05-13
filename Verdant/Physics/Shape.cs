using System;

namespace Verdant.Physics
{
    [Serializable]
    public class Shape
    {

        internal Vec2[] Vertices { get; set; }
        internal Vec2 Position { get; set; }
        public Vec2 Dir { get; set; }

    }
}
