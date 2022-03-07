using System;

namespace IsoEngine.Physics
{
    public class Body
    {

        public Shape[] Components { get; set; }
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
                if (Components == null)
                    return;
                if (Components.Length == 1)
                    Components[0].Position = value;
            }
        }

        private float _mass;
        public float Mass
        {
            get { return _mass; }
            protected set
            {
                _mass = value;
                if (value == 0)
                    InvMass = 0;
                else
                    InvMass = 1 / value;
            }
        }
        internal float InvMass { get; set; }

        private float _inertia;
        public float Inertia
        {
            get { return _inertia; }
            protected set
            {
                _inertia = value;
                if (value == 0)
                    InvInertia = 0;
                else
                    InvInertia = 1 / value;
            }
        }
        internal float InvInertia { get; set; }

        public Vec2 Velocity { get; set; } = new Vec2(0, 0);
        public Vec2 Acceleration { get; set; } = new Vec2(0, 0);

        public float Speed { get; set; }
        public float Friction { get; set; }
        public float AngleSpeed { get; set; }
        public float AngleFriction { get; set; }

        public float Elasticity { get; set; } = 1;

        public Body() { }
        
        public virtual void Move()
        {
            Acceleration = Acceleration.Unit() * Speed;
            Velocity += Acceleration;
            Velocity *= 1 - Friction;
        }

    }
}
