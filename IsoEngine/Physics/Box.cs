using System;

namespace IsoEngine.Physics
{
    public class Box : Body
    {

        public Box(float x1, float y1, float x2, float y2, float w, float h, float m) : base()
        {
            Rectangle rectangle1 = new Rectangle(x1, y1, x2, y2, w);

            Mass = m;
            Inertia = Mass * (
                (float) Math.Pow(rectangle1.Width, 2) +
                (float) Math.Pow(rectangle1.Length, 2)) / 12;

            Components = new Shape[] { rectangle1 };
        }

        public override void Move()
        {
            base.Move();

            ((Rectangle)Components[0]).Position += Velocity;
            
            AngleSpeed *= (1 - AngleFriction);
            ((Rectangle)Components[0]).Angle += AngleSpeed;
            ((Rectangle)Components[0]).CalculateVertices();
        }

    }
}
