using System;

namespace IsoEngine.Physics
{
    internal class Ball : Body
    {

        public Ball(float x, float y, float r, float mass) : base()
        {
            Components = new Shape[] { new Circle(x, y, r) };
            Position = new Vec2(x, y);

            Mass = mass;
        }

        public override void Move()
        {
            base.Move();

            ((Circle)Components[0]).Position += Velocity;
        }

    }
}
