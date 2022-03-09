using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine.Physics
{
    public class Body
    {

        public Shape[] Components { get; set; } = new Shape[0];
        public Vec2 Position
        {
            get
            {
                if (Components.Length > 0) //the important one is always at index 0
                    return Components[0].Position;
                return null;
            }
            set
            {
                if (Components.Length == 1)
                    Components[0].Position = value;
            }
        }

        private float _mass;
        public float Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                if (value == 0)
                    InvMass = 0;
                else
                    InvMass = 1 / value;
            }
        }
        internal float InvMass { get; private set; }

        private float _inertia;
        public float Inertia
        {
            get { return _inertia; }
            set
            {
                _inertia = value;
                if (value == 0)
                    InvInertia = 0;
                else
                    InvInertia = 1 / value;
            }
        }
        internal float InvInertia { get; private set; }

        public Vec2 Velocity { get; set; } = new Vec2(0, 0);
        public Vec2 Acceleration { get; set; } = new Vec2(0, 0);

        public float Speed { get; set; }
        public float Friction { get; set; }
        public float AngleSpeed { get; set; }
        public float AngleFriction { get; set; }

        public float Elasticity { get; set; } = 1f;

        public bool Trigger { get; set; }
        public Entity BodyParent { get; protected set; }
        public Color BodyColor { get; set; } = Color.Yellow;

        public Body() { }

        public virtual void Move()
        {
            Acceleration = Acceleration.Unit() * Speed;
            Velocity += Acceleration;
            Velocity *= 1 - Friction;

            Components[0].Position += Velocity;
        }
        
        public virtual void DrawBody(SpriteBatch spriteBatch)
        {
            foreach (Shape s in Components)
            {
                if (s.GetType() == typeof(Rectangle))
                {
                    Rectangle r = (Rectangle)s;

                    Renderer.DrawLine(spriteBatch,
                        r.Vertices[0],
                        r.Vertices[1],
                        BodyColor
                        );
                    Renderer.DrawLine(spriteBatch,
                        r.Vertices[1],
                        r.Vertices[2],
                        BodyColor
                        );
                    Renderer.DrawLine(spriteBatch,
                        r.Vertices[2],
                        r.Vertices[3],
                        BodyColor
                        );
                    Renderer.DrawLine(spriteBatch,
                        r.Vertices[3],
                        r.Vertices[0],
                        BodyColor
                        );
                }
                else if (s.GetType() == typeof(Circle))
                {
                    Circle c = (Circle)s;

                    spriteBatch.Draw(
                        Renderer.GenerateCircleSprite(c.Radius * Renderer.Scale, Color.White).Get(),
                        Renderer.Camera.GetRenderBounds(
                            c.Position.X - c.Radius,
                            c.Position.Y - c.Radius,
                            (int)(c.Radius * 2),
                            (int)(c.Radius * 2)),
                        BodyColor
                        );
                }
                else if (s.GetType() == typeof(Line))
                {
                    Line l = (Line)s;

                    Renderer.DrawLine(spriteBatch,
                        l.Vertices[0],
                        l.Vertices[1],
                        BodyColor
                        );
                }
            }
        }

    }
}
