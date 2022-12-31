using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Physics
{
    
    public class PhysicsEntity : Entity
    {
        public Shape[] Components { get; set; } = new Shape[0];
        public override Vec2 Position
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

        public float Speed { get; set; } = 1f;
        public float Friction { get; set; }
        public float AngleSpeed { get; set; }
        public float AngleFriction { get; set; }

        public float Elasticity { get; set; } = 1f;

        public bool Trigger { get; set; } = false;

        public Color BodyColor { get; set; } = Color.Yellow;

        internal List<PhysicsEntity> Colliding { get; private set; } = new();

        /// <summary>
        /// Initialize a new PhysicsEntity. By default, it will have a mass but no Components.
        /// In most cases, it is more appropriate to use an extension like a BallEntity or BoxEntity.
        /// </summary>
        public PhysicsEntity(RenderObject sprite, Vec2 position, float width, float height, float mass)
                : base(sprite, position, (int)width, (int)height)
        {
            Mass = mass;
        }

        /// <summary>
        /// Perform physics movement for the Body.
        /// </summary>
        public virtual void Move()
        {
            Acceleration = Acceleration.Unit() * Speed;
            Velocity += Acceleration;
            Velocity *= 1 - Friction;

            Components[0].Position += Velocity;
        }

        /// <summary>
        /// Get a list of all PhysicsEntities of a given type currently colliding with this Entity.
        /// </summary>
        /// <typeparam name="TPhysicsEntity">The type of PhysicsEntity to check for.</typeparam>
        /// <param name="includeTriggers">Check against PhysicsEntities that are triggers.</param>
        /// <param name="includeSolids">Check against PhysicsEntities that are not triggers.</param>
        /// <returns>A list containing all colliding Entities.</returns>
        public List<TPhysicsEntity> GetColliding<TPhysicsEntity>(bool includeTriggers = true, bool includeSolids = true) where TPhysicsEntity : PhysicsEntity // largely copied from GetAllColliding
        {
            List<TPhysicsEntity> colliding = new List<TPhysicsEntity>();

            foreach (PhysicsEntity p in Colliding)
            {
                if (!p.IsType(typeof(TPhysicsEntity)))
                    continue;

                if (includeTriggers && p.Trigger)
                {
                    colliding.Add((TPhysicsEntity)p);
                    continue;
                }

                if (includeSolids && !p.Trigger)
                {
                    colliding.Add((TPhysicsEntity)p);
                    continue;
                }

                colliding.Add((TPhysicsEntity)p);
            }

            return colliding;
        }

        public List<PhysicsEntity> GetColliding(bool includeTriggers = true, bool includeSolids = true)
        {
            return GetColliding<PhysicsEntity>();
        }

        /// <summary>
        /// Visualize the Components of the Body according to the BodyColor.
        /// The default implementation is only intended for debug purposes as
        /// some draw calls, particularly for Circles are highly inefficient.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public virtual void DrawBody(SpriteBatch spriteBatch)
        {
            foreach (Shape s in Components)
            {
                s.Draw(spriteBatch, Manager.Scene.Camera, BodyColor);
            }
        }

    }
}
