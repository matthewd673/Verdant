using System;

namespace IsoEngine.Physics
{
    internal class Capsule : Body
    {

        public Capsule(float x1, float y1, float x2, float y2, float r, float m) : base()
        {
            Circle circle1 = new Circle(x1, y1, r);
            Circle circle2 = new Circle(x2, y2, r);

            Vec2 recVec1 = circle2.Position + ((
                circle2.Position - circle1.Position).Unit().Normal() * r);
            Vec2 recVec2 = circle1.Position + ((
                circle2.Position - circle1.Position).Unit().Normal() * r);

            Rectangle rectangle1 = new Rectangle(recVec1.X, recVec1.Y, recVec2.X, recVec2.Y, 2 * r);

            Mass = m;
            Inertia = Mass * (
                (float)Math.Pow(2 * rectangle1.Width, 2) +
                (float)Math.Pow(rectangle1.Length + 2 * rectangle1.Width, 2)) / 12;

            Components = new Shape[]
            {
                rectangle1,
                circle1,
                circle2,
            };
        }

        public override void Move()
        {
            base.Move();

            ((Rectangle)Components[0]).Position = ((Rectangle)Components[0]).Position + Velocity;

            AngleSpeed *= (1 - AngleFriction);
            ((Rectangle)Components[0]).Angle += AngleSpeed;
            ((Rectangle)Components[0]).CalculateVertices();

            ((Circle)Components[1]).Position = ((Rectangle)Components[0]).Position +
                ((Rectangle)Components[0]).Dir * -((Rectangle)Components[0]).Length / 2;
            ((Circle)Components[2]).Position = ((Rectangle)Components[0]).Position +
                ((Rectangle)Components[0]).Dir * ((Rectangle)Components[0]).Length / 2;
        }

    }
}
