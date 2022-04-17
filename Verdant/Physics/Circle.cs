using System;

namespace Verdant.Physics
{
    internal class Circle : Shape
    {

        internal float Radius { get; set; }

        /// <summary>
        /// Initialize a new Circle shape.
        /// </summary>
        /// <param name="x">The X component of the center of the Circle.</param>
        /// <param name="y">The Y component of the center of the Circle.</param>
        /// <param name="radius">The radius of the Circle.</param>
        public Circle(float x, float y, float radius)
        {
            Vertices = new Vec2[2];
            Position = new Vec2(x, y);
            Radius = radius;
        }

    }
}
